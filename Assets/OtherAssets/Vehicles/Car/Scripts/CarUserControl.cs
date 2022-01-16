using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (YapayZekaController))]
    public class CarUserControl : MonoBehaviour
    {
        private YapayZekaController m_Car; // the car controller we want to use
        bool canMove = false;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<YapayZekaController>();
            CameraControl.playerController = m_Car;
        }

        private void OnEnable()
        {
            EventManager.OnStartGame += onStartGame;
        }

        private void OnDisable()
        {
            EventManager.OnStartGame -= onStartGame;
        }

        void onStartGame()
        {
            canMove = true;
        }


        private void FixedUpdate()
        {
            if (!canMove) return;
            
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
