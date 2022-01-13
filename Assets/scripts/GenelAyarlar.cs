using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using TMPro;
using UnityEngine.SceneManagement;


public class GenelAyarlar : MonoBehaviour
{
    public GameObject[] Araclar;
    public GameObject[] YapayZekaSpawnPoint;
    public GameObject[] YapayZekaAraclar;
    public List<GameObject>  OlusanAraclar= new List<GameObject>() ;
    public TextMeshProUGUI gerisay�m;
    float saniye = 3f;
    bool sayac = true;
    Coroutine sayac�m;
    public AudioSource[] sesler;
    public GameObject OyunSonuPanel;
    
    //oyun m�zi�ini oynat
    
    public GameObject SpawnPoint;

    private CameraControl camControl;


    void Start()
    {
        sayac�m = StartCoroutine(SayacKontrol());
        gerisay�m.text = saniye. ToString();
        camControl = FindObjectOfType<CameraControl>();
        GameObject arabam = Instantiate(Araclar[PlayerPrefs.GetInt("SecilenArac")], SpawnPoint.transform.position, SpawnPoint.transform.rotation);


        // GameObject.Find("Main Camera").GetComponent<CameraControl>().target[0] = arabam.transform.Find("PozisyonAl");
        // GameObject.Find("Main Camera").GetComponent<CameraControl>().target[1] = arabam.transform.Find("Pivot");
        //
        // GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[1] = arabam.transform.Find("Kameralar/OnKaput").gameObject;
        // GameObject.Find("OyunKontrol").GetComponent<KameraGecisKontrol>().kameralar[2] = arabam.transform.Find("Kameralar/Aracici").gameObject;

        // for (int i = 0; i < 2; i++)  // 2 s�zgelimi yaz�lm�st�r
        // {
        //   int randomdeger = Random.Range(0, YapayZekaAraclar.Length - 1);
        //  GameObject OlusanArac= Instantiate(YapayZekaAraclar[randomdeger], YapayZekaSpawnPoint[i].transform.position, YapayZekaSpawnPoint[i].transform.rotation);
        //    OlusanArac.GetComponent<YapayZekaController>().SpawnPointIndex = i;
        //  }
        //}
        for(int i = 0; i < 3; i++)
        {
            int randomdeger = Random.Range(0, YapayZekaAraclar.Length - 1);

           GameObject OlusanArac = Instantiate(YapayZekaAraclar[randomdeger], YapayZekaSpawnPoint[i].transform.position, YapayZekaSpawnPoint[i]. transform.rotation);
            OlusanArac.GetComponent<YapayZekaController>().SpawnPointIndex = i;

        }



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
        OyunSonuPanel.transform.Find("Panel/s�ra").GetComponent<TextMeshProUGUI>().text=pozisyon.ToString()+". Bitirdin.";
        OyunSonuPanel.SetActive(true);

    }
    public void SahneDegis(int deger)
    {
        StartCoroutine(GecisYap(deger));
    }
    public void c�k()
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
        while( sayac)
        {
            yield return new WaitForSeconds(1f);
            saniye --;
            gerisay�m.text = Mathf.Round(saniye).ToString();
            if (saniye < 0)
            {

                foreach (var araba in OlusanAraclar)
                {
                    if (araba.gameObject.name == "biz")
                    {
                        araba.GetComponentInParent<CarUserControl>().enabled = true;
                    }
                    else
                    {
                        araba.GetComponentInParent<CarAIControl>().m_Driving = true;
                    }
                }
                gerisay�m.enabled = false;
                sayac = false;
                StopCoroutine(sayac�m);
            }
    }
        

        /* private void Update()
         {
             if (sayac)
             {
                 saniye -= Time.deltaTime;
                 gerisay�m.text = Mathf.Round(saniye).ToString();



             } */

    }



}