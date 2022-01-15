using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Siralama : MonoBehaviour
{
    public int AktıfYonSırası = 1;
    SiralamaManager sıralama;
    GenelAyarlar genelayarlar;
    
    public int pozisyon;


    void Start()
    {
        sıralama = GameObject.FindWithTag("OyunKontrol").GetComponent<SiralamaManager>();
        sıralama.kendinigonder(gameObject,AktıfYonSırası);

        genelayarlar = GameObject.FindWithTag("OyunKontrol").GetComponent<GenelAyarlar>();
        genelayarlar.kendinigonder(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YonBul"))
        {
            AktıfYonSırası =int.Parse( other.transform.gameObject.name);
            sıralama.SıralamaGuncelle(gameObject,AktıfYonSırası);
        }
        if (gameObject.name=="biz")
        {
            if (other.CompareTag("Finish"))
            {
                genelayarlar.OyunSonu(pozisyon);
            }

        }
       
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
