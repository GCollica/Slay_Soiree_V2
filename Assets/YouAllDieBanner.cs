using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouAllDieBanner : MonoBehaviour
{
    public Animator bannerAnimator;

    public void ShowLostBanner()
    {
        Invoke("FadeBanner", 1);
        Invoke("Reset", 9);
    }

    public void FadeBanner()
    {
        bannerAnimator.SetTrigger("ShowLoseBanner");
    }

    public void Reset()
    {
        SceneManager.LoadScene("Title Scene");
    }
}
