using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ayarlar : MonoBehaviour
{
    public Slider menusesSlider;
    public Slider oyunsesSlider;
    AudioSource menusesi;
    public Dropdown kalitesecenekler;

    void Start()
    {
        menusesi = GameObject.Find("OyunKontrol").GetComponent<AudioSource>();
        menusesSlider.value = PlayerPrefs.GetFloat("menuses");
        oyunsesSlider.value = PlayerPrefs.GetFloat("oyunses");
        kalitesecenekler.value = PlayerPrefs.GetInt("Kalite");
    }

    
    void Update()
    {
        
    }
    public void MenuSesDegisim()
    {
        PlayerPrefs.SetFloat("menuses", menusesSlider.value);
        menusesi.volume = menusesSlider.value;

    }
    public void OyunSesDegisim()
    {
        PlayerPrefs.SetFloat("oyunses", oyunsesSlider.value);

    }
    public void KaliteSecim(int kalýtesecým)
    {
        PlayerPrefs.SetInt("Kalite", kalýtesecým);
        QualitySettings.SetQualityLevel(kalýtesecým);
    }
}
