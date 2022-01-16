using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;
using Random = UnityEngine.Random;


public class GenelAyarlar : MonoBehaviour
{
    public GameObject[] Araclar;
    public GameObject[] YapayZekaSpawnPoint;
    public GameObject[] YapayZekaAraclar;
    public List<GameObject> OlusanAraclar = new List<GameObject>();
    public TextMeshProUGUI gerisayacText;
    float saniye = 3f;
    bool sayac = true;
    Coroutine sayaxRoutine;
    public AudioSource[] sesler;
    public GameObject OyunSonuPanel;

    public static GenelAyarlar master;

    //oyun müziðini oynat

    public GameObject SpawnPoint;

    private CameraControl camControl;

    public GameObject TersYonObject;

    private void OnEnable()
    {
        master = this;
    }

    void Start()
    {
        sayaxRoutine = StartCoroutine(SayacKontrol());
        gerisayacText.text = saniye.ToString();
        camControl = FindObjectOfType<CameraControl>();
        var clonedCar = Instantiate(Araclar[PlayerPrefs.GetInt("SecilenArac")], SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        clonedCar.SetActive(true);
        TersYonObject = GameObject.FindWithTag("TersYonPanel");
    }

    public void kendinigonder(GameObject gelenobje)
    {
        OlusanAraclar.Add(gelenobje);

        //  if (arabalar.Count == 4)
        // {
        //    siralamakontrolet();
    }

    public void OyunSonu(int pozisyon)
    {
        OyunSonuPanel.transform.Find("SonucPanel/Panel/sıra").GetComponent<TextMeshProUGUI>().text = pozisyon.ToString() + ". Bitirdin.";
        OyunSonuPanel.SetActive(true);
    }

    public void SahneDegis(int deger)
    {
        StartCoroutine(GecisYap(deger));
    }

    public void cık()
    {
        Application.Quit();
    }

    IEnumerator GecisYap(int deger)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(deger);
    }

    IEnumerator SayacKontrol()
    {
        while (sayac)
        {
            yield return new WaitForSeconds(1f);
            saniye--;
            gerisayacText.text = Mathf.Round(saniye).ToString();
            if (saniye < 0)
            {
                gerisayacText.enabled = false;
                gerisayacText.transform.parent.gameObject.SetActive(false);
                sayac = false;
                StopCoroutine(sayaxRoutine);
                
                // start cars currently in the level
                EventManager.OnStartGame?.Invoke();
            }
        }


        /* private void Update()
         {
             if (sayac)
             {
                 saniye -= Time.deltaTime;
                 gerisayým.text = Mathf.Round(saniye).ToString();



             } */
    }
}