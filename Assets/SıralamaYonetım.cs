using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class araba
{
    public GameObject gelenobje;
    public int pozisyon;
    public araba(GameObject disgelenobje,int dispozisyon)
    {
        gelenobje = disgelenobje;
        pozisyon = dispozisyon;
    }

}
public class SıralamaYonetım : MonoBehaviour
{
    public List<araba> arabalar = new List<araba>();
    public TextMeshProUGUI sira;

    public void kendinigonder(GameObject gelenobje, int aktifyonu)
    {

        arabalar.Add(new araba(gelenobje,aktifyonu));
        if (arabalar.Count==3)
        {
            siralamakontrolet();
        }
    }
    public void SıralamaGuncelle (GameObject gelenaraba,int pozisyon )
    {
        for(int i =0; i < arabalar.Count; i++)
        {
            if(arabalar[i].gelenobje== gelenaraba)
            {
                arabalar[i].pozisyon = pozisyon;
            }
        }
    }
    public void siralamakontrolet()
    {
        foreach(var araba in arabalar)
        {
            sira.text += araba.gelenobje.name + "-" + araba.pozisyon + "<br>" ;
        }

    }

    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
