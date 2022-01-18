using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Car;


public class araba
{
    public GameObject gelenobje;
    public int pozisyon;
    public WaypointProgressTracker ProgressTracker;
    public bool IsPlayer = false;
    public YapayZekaController mController;
    public int TotalCheckpointTick => mController.CheckpointTouched.Count;
    public float Completion => (mController.CheckpointTouched.Count * 1f) / (GenelAyarlar.master.CheckpointItems.Length * 1f);

    public araba(GameObject disgelenobje, int dispozisyon)
    {
        gelenobje = disgelenobje;
        pozisyon = dispozisyon;
        ProgressTracker = disgelenobje.transform.parent.GetComponent<WaypointProgressTracker>();
        mController = disgelenobje.transform.parent.GetComponent<YapayZekaController>();
        IsPlayer = disgelenobje.name.Equals("biz");
    }
}

public class SiralamaManager : MonoBehaviour
{
    public List<araba> arabalar = new List<araba>();
    public TextMeshProUGUI sira;

    public void kendinigonder(GameObject gelenobje, int aktifyonu)
    {
        arabalar.Add(new araba(gelenobje, aktifyonu));
        // if (arabalar.Count == 4)
        // {
        //     siralamakontrolet();
        // }
    }

    private float timer = 0.5f;
    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0f)
        {
            timer = 0.5f;
            // var playerObject = arabalar.Find(x => x.IsPlayer);
            arabalar = arabalar.OrderBy(x => x.mController.FinishTimer).ThenByDescending(x => x.Completion).ToList();
            var playerIndex = arabalar.FindIndex(x => x.IsPlayer);
            sira.text = $"{playerIndex+1}/{arabalar.Count}";
        }
    }

    public void SıralamaGuncelle(GameObject gelenaraba, int pozisyon)
    {
        for (int i = 0; i < arabalar.Count; i++)
        {
            if (arabalar[i].gelenobje == gelenaraba)
            {
                arabalar[i].pozisyon = pozisyon;
            }
        }
    }

    public void siralamakontrolet()
    {
        arabalar = arabalar.OrderBy(w => w.pozisyon).ToList();
        sira.text = "";

        for (int i = 0; i < arabalar.Count; i++)
        {
            switch (i)
            {
                case 0:
                    if (arabalar[i].gelenobje.name == "biz")
                    {
                        sira.text = "4/4";
                        arabalar[i].gelenobje.GetComponent<Siralama>().pozisyon = 4;
                    }

                    break;
                case 1:
                    if (arabalar[i].gelenobje.name == "biz")
                    {
                        sira.text = "3/4";
                        arabalar[i].gelenobje.GetComponent<Siralama>().pozisyon = 3;
                    }

                    break;
                case 2:
                    if (arabalar[i].gelenobje.name == "biz")
                    {
                        sira.text = "2/4";
                        arabalar[i].gelenobje.GetComponent<Siralama>().pozisyon = 2;
                    }

                    break;
                case 3:
                    if (arabalar[i].gelenobje.name == "biz")
                    {
                        sira.text = "1/4";
                        arabalar[i].gelenobje.GetComponent<Siralama>().pozisyon = 1;
                    }

                    break;
            }
        }
    }
}