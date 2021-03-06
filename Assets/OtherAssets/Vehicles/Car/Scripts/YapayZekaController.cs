using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

#pragma warning disable 649
namespace UnityStandardAssets.Vehicles.Car
{
    internal enum CarDriveTypeYapayZeka
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    internal enum SpeedTypeYapayZeka
    {
        MPH,
        KPH
    }

    public class YapayZekaController : MonoBehaviour
    {
        [SerializeField] private CarDriveTypeYapayZeka m_CarDriveType = CarDriveTypeYapayZeka.FourWheelDrive;
        [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
        [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
        [SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
        [SerializeField] private Vector3 m_CentreOfMassOffset;
        [SerializeField] private float m_MaximumSteerAngle;
        [Range(0, 1)] [SerializeField] private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
        [SerializeField] private float m_FullTorqueOverAllWheels;
        [SerializeField] private float m_ReverseTorque;
        [SerializeField] private float m_MaxHandbrakeTorque;
        [SerializeField] private float m_Downforce = 100f;
        [SerializeField] private SpeedTypeYapayZeka m_SpeedType;
        [SerializeField] private float m_Topspeed = 200;
        [SerializeField] private static int NoOfGears = 5;
        [SerializeField] private float m_RevRangeBoundary = 1f;
        [SerializeField] private float m_SlipLimit;
        [SerializeField] private float m_BrakeTorque;


        private Quaternion[] m_WheelMeshLocalRotations;
        private Vector3 m_Prevpos, m_Pos;
        private float m_SteerAngle;
        private int m_GearNum;
        private float m_GearFactor;
        private float m_OldRotation;
        private float m_CurrentTorque;
        private Rigidbody m_Rigidbody;
        private const float k_ReversingThreshold = 0.01f;

        public GameObject[] ArkaFar;
        public GameObject[] OnFar;
        bool OnFarAcikmi;


        public bool Skidding { get; private set; }
        public float BrakeInput { get; private set; }

        public float CurrentSteerAngle
        {
            get { return m_SteerAngle; }
        }

        public float CurrentSpeed
        {
            get { return m_Rigidbody.velocity.magnitude * 2.23693629f; }
        }

        public float MaxSpeed
        {
            get { return m_Topspeed; }
        }

        public float Revs { get; private set; }
        public float AccelInput { get; private set; }

        public int SpawnPointIndex;

        // H???Z KADRAN??? DE?????????KENLER???
        public Text mevcuthiz;
        public Text mevcutvites;
        int SonHiz;

        public GameObject Kadran;

        // N???TRO KADRAN DEG???SKENLER???
        public Image Nitroslider;
        public Text NitrodegerText;
        float nitrodeger = 100;
        bool NitroPressed = false;

        // EGZOZ ??????LEMLER???
        public GameObject NitroEfekt;
        public AudioSource[] Sesler;
        public bool IsCurrentPlayer = false;

        public Transform[] Target;

        [FormerlySerializedAs("G??d??sYonuIs??n")]
        public Transform RaycastForward;

        public int YonG??d??sIndex = 1;

        [SerializeField] private WaypointCircuit _waypointCircuit;
        private float checkpointTimer = 0.1f;
        public Transform LastCheckPoint;
        private bool canMove = false;

        // Store checkpoints
        public List<Transform> CheckpointTouched = new List<Transform>();
        public float FinishTimer = 0f;


        // Use this for initialization
        private void Start()
        {
            if (_waypointCircuit == null)
            {
                _waypointCircuit = FindObjectOfType<WaypointCircuit>();
            }

            if (GetComponent<CarUserControl>())
            {
                IsCurrentPlayer = true;
                CameraControl.playerController = this;
            }

            m_WheelMeshLocalRotations = new Quaternion[4];
            for (int i = 0; i < 4; i++)
            {
                m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }

            m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

            m_MaxHandbrakeTorque = float.MaxValue;
            m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
            //GameObject.Find("Canvas/HizKadran/speedometer/hiz").GetComponent<Text>();

            mevcuthiz = GameObject.FindWithTag("MevcutH??z").GetComponent<Text>();
            mevcutvites = GameObject.FindWithTag("Vites").GetComponent<Text>();
            Kadran = GameObject.FindWithTag("Kadran");
            Nitroslider = GameObject.FindWithTag("NitroSlider").GetComponent<Image>();
            NitrodegerText = GameObject.FindWithTag("NitroDeger").GetComponent<Text>();

            NitrodegerText.text = Mathf.CeilToInt(nitrodeger).ToString();

            if (IsCurrentPlayer)
                activateCamera();
        }

        bool tersYon = false;

        void Update()
        {
            CheckIfCarFlipped();
            OnFarKontrol();
            FrenYap();
            HizKadranKontrol();
            NitroKullan();
            AraciDuzelt();
            CamraToggle();
            CheckRightSideForDirection();

            if (IsCurrentPlayer)
            {
            }
        }

        private void CheckRightSideForDirection()
        {
            var raycast = new []
            {
                transform.right , transform.right + transform.forward, transform.right + -transform.forward, transform.right + (transform.up / 3f)
            };

            int totalhit = 0;
            foreach (var rayCalc in raycast)
            {
                var checkRightside = Physics.Raycast(transform.position, rayCalc, 60, 1 << 20);
                if (checkRightside) totalhit++;
                Debug.DrawLine(transform.position,transform.position + rayCalc * 60f, Color.green);
            }
            
            GenelAyarlar.master.TersYonObject.SetActive(totalhit == 0);
        }

        private void CamraToggle()
        {
            if (Input.GetKeyDown("c"))
            {
                NextCamera();
            }
        }

        private bool resetNextTime = false;
        private float flipTimer = 1f;
        public Transform FloorChecker;

        private void CheckIfCarFlipped()
        {
            if (IsCurrentPlayer) return;

            flipTimer -= Time.deltaTime;
            if (flipTimer < 0f)
            {
                flipTimer = Random.Range(1f, 3f);
                var collidedRoad = Physics.OverlapSphere(FloorChecker.position, 0.5f, 1 << 10);

                if (collidedRoad.Length == 0 && !resetNextTime)
                {
                    resetNextTime = true;
                }
                else if (resetNextTime && collidedRoad.Length == 0)
                {
                    m_Rigidbody.Sleep();
                    var randFloat = Random.Range(-4f, 4f);
                    transform.position = LastCheckPoint.position + new Vector3(randFloat, 1f, 0);
                    transform.rotation = LastCheckPoint.rotation;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (FloorChecker)
                Gizmos.DrawSphere(FloorChecker.position, 0.5f);
        }

        void AraciDuzelt()
        {
            if (Input.GetKey(KeyCode.R) && IsCurrentPlayer)
            {
                m_Rigidbody.Sleep();
                var randFloat = Random.Range(-4f, 4f);
                var pos = LastCheckPoint.position + new Vector3(randFloat, 1f, 0);
                transform.position = pos;
                transform.rotation = LastCheckPoint.rotation;
                // transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        private void OnEnable()
        {
            m_Rigidbody = GetComponent<Rigidbody>();

            EventManager.OnPlayerFinish += onPlayerFinished;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerFinish -= onPlayerFinished;
        }

        private void onPlayerFinished()
        {
            canMove = false;
        }

        private void GearChanging()
        {
            float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
            float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
            float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

            if (m_GearNum > 0 && f < downgearlimit)
            {
                m_GearNum--;
                UserInterface.master.GearText.text = m_GearNum.ToString();
            }

            if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
            {
                m_GearNum++;
                UserInterface.master.GearText.text = m_GearNum.ToString();
            }

            if (SonHiz == 0)
            {
                UserInterface.master.GearText.text = "P";
            }

            if (SonHiz > 0)
            {
                if (m_GearNum == 0)
                {
                    UserInterface.master.GearText.text = "1";
                }
                else
                {
                    UserInterface.master.GearText.text = m_GearNum.ToString();
                }
            }

            if (Input.GetAxis("Vertical") == -1)
            {
                UserInterface.master.GearText.text = "R";
            }
        }


        // simple function to add a curved bias towards 1 for a value in the 0-1 range
        private static float CurveFactor(float factor)
        {
            return 1 - (1 - factor) * (1 - factor);
        }


        // unclamped version of Lerp, to allow value to exceed the from-to range
        private static float ULerp(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }


        private void CalculateGearFactor()
        {
            float f = (1 / (float)NoOfGears);
            // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
            // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
            var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
            m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
        }


        private void CalculateRevs()
        {
            // calculate engine revs (for display / sound)
            // (this is done in retrospect - revs are not used in force/power calculations)
            CalculateGearFactor();
            var gearNumFactor = m_GearNum / (float)NoOfGears;
            var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
            var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
            Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
        }


        public void Move(float steering, float accel, float footbrake, float handbrake)
        {
            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 position;
                m_WheelColliders[i].GetWorldPose(out position, out quat);
                m_WheelMeshes[i].transform.position = position;
                m_WheelMeshes[i].transform.rotation = quat;
            }

            //clamp input values
            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);
            BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
            handbrake = Mathf.Clamp(handbrake, 0, 1);

            //Set the steer on the front wheels.
            //Assuming that wheels 0 and 1 are the front wheels.
            m_SteerAngle = steering * m_MaximumSteerAngle;
            m_WheelColliders[0].steerAngle = m_SteerAngle;
            m_WheelColliders[1].steerAngle = m_SteerAngle;

            SteerHelper();
            ApplyDrive(accel, footbrake);
            CapSpeed();

            //Set the handbrake.
            //Assuming that wheels 2 and 3 are the rear wheels.
            if (handbrake > 0f)
            {
                var hbTorque = handbrake * m_MaxHandbrakeTorque;
                m_WheelColliders[2].brakeTorque = hbTorque;
                m_WheelColliders[3].brakeTorque = hbTorque;
            }


            CalculateRevs();
            GearChanging();

            AddDownForce();
            CheckForWheelSpin();
            TractionControl();
        }

        void OnFarKontrol()
        {
            if (!IsCurrentPlayer) return;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnFarAcikmi = !OnFarAcikmi;
                foreach (var isiklar in OnFar)
                {
                    isiklar.SetActive(OnFarAcikmi);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("YonBul"))
            {
                if (!CheckpointTouched.Contains(other.transform))
                {
                    CheckpointTouched.Add(other.transform);
                }

                LastCheckPoint = other.transform;
            }

            if (other.CompareTag("FinishLine") && CheckpointTouched.Count > 30)
            {
                if (IsCurrentPlayer)
                {
                    EventManager.OnPlayerFinish?.Invoke();
                }

                if (CheckpointTouched.Count > GenelAyarlar.master.CheckpointItems.Length / 2)
                {
                    FinishTimer = Time.time;
                }
            }
        }

        void NitroKullan()
        {
            if (!IsCurrentPlayer || nitrodeger <= 0f) return;

            if (Input.GetKeyDown(KeyCode.V))
            {
                NitroPressed = true;
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                NitroEfekt.SetActive(false);
                NitroPressed = false;
            }

            if (NitroPressed)
            {
                NitroEfekt.SetActive(true);
                m_Rigidbody.velocity += 0.2f * m_Rigidbody.velocity.normalized;
                nitrodeger -= 1;
                NitrodegerText.text = "HAZIR";

                Nitroslider.fillAmount = nitrodeger / 100;
                if (!Sesler[0].isPlaying)
                {
                    Sesler[0].Play();
                }

                NitrodegerText.text = Mathf.CeilToInt(nitrodeger).ToString();
            }

            if (nitrodeger <= 0)
            {
                NitroEfekt.SetActive(false);
                nitrodeger = 0;
                NitroPressed = false;
                NitrodegerText.text = Mathf.CeilToInt(nitrodeger).ToString();
                return;
            }
        }

        #region Camera Controller

        private int activeCameraId = 0;

        void activateCamera()
        {
            if (!IsCurrentPlayer) return;

            foreach (var cam in Target)
                cam.gameObject.SetActive(false);

            Target[activeCameraId].gameObject.SetActive(true);
        }

        public void NextCamera()
        {
            if (!IsCurrentPlayer) return;

            activeCameraId = (activeCameraId + 1) % Target.Length;
            activateCamera();
        }

        #endregion

        void FrenYap()
        {
            if (!IsCurrentPlayer) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ArkaFar.GetComponent<MeshRenderer>().material = ArkaFarMateryaller[1];

                foreach (var isiklar in ArkaFar)
                {
                    isiklar.SetActive(true);
                }

                for (int i = 0; i < 4; i++)
                {
                    m_WheelColliders[i].GetComponent<WheelCollider>().brakeTorque = m_BrakeTorque;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (var isiklar in ArkaFar)
                {
                    isiklar.SetActive(true);
                }

                for (int i = 0; i < 4; i++)
                {
                    m_WheelColliders[i].GetComponent<WheelCollider>().brakeTorque = 0;
                }
            }
        }

        void HizKadranKontrol()
        {
            if (!IsCurrentPlayer) return;

            SonHiz = (int)CurrentSpeed;
            UserInterface.master.CurrentSpeed.text = ((int)CurrentSpeed).ToString();
            if (CurrentSpeed == 0)
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                UserInterface.master.SpeedoMeter.transform.localRotation = rotation;
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, -CurrentSpeed * 1.3f));
                UserInterface.master.SpeedoMeter.transform.localRotation = rotation;
            }
        }


        private void CapSpeed()
        {
            float speed = m_Rigidbody.velocity.magnitude;
            switch (m_SpeedType)
            {
                case SpeedTypeYapayZeka.MPH:

                    speed *= 2.23693629f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.velocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.velocity.normalized;
                    break;

                case SpeedTypeYapayZeka.KPH:
                    speed *= 3.6f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.velocity = (m_Topspeed / 3.6f) * m_Rigidbody.velocity.normalized;
                    break;
            }
        }


        private void ApplyDrive(float accel, float footbrake)
        {
            float thrustTorque;
            switch (m_CarDriveType)
            {
                case CarDriveTypeYapayZeka.FourWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 4f);
                    for (int i = 0; i < 4; i++)
                    {
                        m_WheelColliders[i].motorTorque = thrustTorque;
                    }

                    break;

                case CarDriveTypeYapayZeka.FrontWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
                    break;

                case CarDriveTypeYapayZeka.RearWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
                    break;
            }

            for (int i = 0; i < 4; i++)
            {
                if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
                {
                    m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
                }
                else if (footbrake > 0)
                {
                    m_WheelColliders[i].brakeTorque = 0f;
                    m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
                }
            }
        }


        private void SteerHelper()
        {
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelhit;
                m_WheelColliders[i].GetGroundHit(out wheelhit);
                if (wheelhit.normal == Vector3.zero)
                    return; // wheels arent on the ground so dont realign the rigidbody velocity
            }

            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
            }

            m_OldRotation = transform.eulerAngles.y;
        }


        // this is used to add more grip in relation to speed
        private void AddDownForce()
        {
            m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_Downforce *
                                                           m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
        }


        // checks if the wheels are spinning and is so does three things
        // 1) emits particles
        // 2) plays tiure skidding sounds
        // 3) leaves skidmarks on the ground
        // these effects are controlled through the WheelEffects class
        private void CheckForWheelSpin()
        {
            // loop through all wheels
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelHit;
                m_WheelColliders[i].GetGroundHit(out wheelHit);

                // is the tire slipping above the given threshhold
                if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
                {
                    m_WheelEffects[i].EmitTyreSmoke();

                    // avoiding all four tires screeching at the same time
                    // if they do it can lead to some strange audio artefacts
                    if (!AnySkidSoundPlaying())
                    {
                        m_WheelEffects[i].PlayAudio();
                    }

                    continue;
                }

                // if it wasnt slipping stop all the audio
                if (m_WheelEffects[i].PlayingAudio)
                {
                    m_WheelEffects[i].StopAudio();
                }

                // end the trail generation
                m_WheelEffects[i].EndSkidTrail();
            }
        }

        // crude traction control that reduces the power to wheel if the car is wheel spinning too much
        private void TractionControl()
        {
            WheelHit wheelHit;
            switch (m_CarDriveType)
            {
                case CarDriveTypeYapayZeka.FourWheelDrive:
                    // loop through all wheels
                    for (int i = 0; i < 4; i++)
                    {
                        m_WheelColliders[i].GetGroundHit(out wheelHit);

                        AdjustTorque(wheelHit.forwardSlip);
                    }

                    break;

                case CarDriveTypeYapayZeka.RearWheelDrive:
                    m_WheelColliders[2].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);

                    m_WheelColliders[3].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                    break;

                case CarDriveTypeYapayZeka.FrontWheelDrive:
                    m_WheelColliders[0].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);

                    m_WheelColliders[1].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                    break;
            }
        }


        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
            {
                m_CurrentTorque -= 10 * m_TractionControl;
            }
            else
            {
                m_CurrentTorque += 10 * m_TractionControl;
                if (m_CurrentTorque > m_FullTorqueOverAllWheels)
                {
                    m_CurrentTorque = m_FullTorqueOverAllWheels;
                }
            }
        }


        private bool AnySkidSoundPlaying()
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelEffects[i].PlayingAudio)
                {
                    return true;
                }
            }

            return false;
        }
    }
}