using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraGecisKontrol : MonoBehaviour
{
    public GameObject[] kameralar;
    int AktifKamera=0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            KameralariKapat();
            AktifKamera++;

            if (AktifKamera > kameralar.Length-1)
            {
                AktifKamera = 0;
            }
            kameralar[AktifKamera].SetActive(true);
        }
    }
    void KameralariKapat()
    {
        foreach(var cam in kameralar)
        {
            cam.SetActive(false);
        }
    }
}
