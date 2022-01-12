using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SahneYonetim : MonoBehaviour
{
    public Animator GecisAnimatoru;
   public void SahneDegistir(int deger)
    {
        StartCoroutine(GecisYap(deger));
    }
    public void cýkýs()
    {
        Application.Quit();
    }
    IEnumerator GecisYap(int deger)
    {
        GecisAnimatoru.SetTrigger("gecis");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(deger);
    }
}
