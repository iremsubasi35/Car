using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OyunBaslangıc : MonoBehaviour
{
    AudioSource menusesi;
    private static OyunBaslangıc oyunbaslangıc;

    private void Awake()
    {
    }
    void Start()
    {
        menusesi = GetComponent<AudioSource>();
        

        

        if (PlayerPrefs.HasKey("menuses"))
        {
            menusesi.volume = PlayerPrefs.GetFloat("menuses");
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Kalite"));

        }
        else
        {   // oyun ilk kez açılıyorsa
            PlayerPrefs.SetFloat("menuses", 1);
            PlayerPrefs.SetFloat("oyunses", 1);
            PlayerPrefs.SetInt("kalite", 3);
            QualitySettings.SetQualityLevel(3);

        }
    }

    
    void Update()
    {
        
    }
}
