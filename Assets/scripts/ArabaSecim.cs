using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArabaSecim : MonoBehaviour
{
    public GameObject[] Arabalar;
    public Text ArabaAd;
    int aktifaracindex = 0;
    void Start()
    {
        Arabalar[aktifaracindex].SetActive(true);
        ArabaAd.text = Arabalar[aktifaracindex].GetComponent<AracBilgileri>().aracadi;
    }
    public void ileri()
    {
        if(aktifaracindex != Arabalar.Length - 1)
        {
            Arabalar[aktifaracindex].SetActive(false);
            aktifaracindex++;
            Arabalar[aktifaracindex].SetActive(true);
            ArabaAd.text = Arabalar[aktifaracindex].GetComponent<AracBilgileri>().aracadi;
        }
        else
        {
            Arabalar[aktifaracindex].SetActive(false);
            aktifaracindex = 0;
            Arabalar[aktifaracindex].SetActive(true);
        }
    }
    public void geri()
    {
        if (aktifaracindex != 0)
        {
            Arabalar[aktifaracindex].SetActive(false);
            aktifaracindex--;
            Arabalar[aktifaracindex].SetActive(true);
            ArabaAd.text = Arabalar[aktifaracindex].GetComponent<AracBilgileri>().aracadi;
        }
        else
        {
            Arabalar[aktifaracindex].SetActive(false);
            aktifaracindex = Arabalar.Length - 1;
            Arabalar[aktifaracindex].SetActive(true);
        }
    }

    // Update is called once per frame
   public void basla()
    {
        PlayerPrefs.SetInt("SecilenArac",aktifaracindex);
        SceneManager.LoadScene("CityMap");
    }
}
