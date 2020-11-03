using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFadeHandler : MonoBehaviour
{
    public Image fadeImage;

    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private RoomProgress roomProgress;

    private void Awake()
    {
        fadeImage = GetComponentInChildren<Image>();
        roomProgress = FindObjectOfType<RoomProgress>();
        SetAlphaOne();
    }

    public void SetAlphaZero()
    {
        fadeImage.canvasRenderer.SetAlpha(0);
    }

    public void SetAlphaOne()
    {
        fadeImage.canvasRenderer.SetAlpha(1);
    }

    public void FadeIn()
    {
        StartCoroutine(nameof(FadeInIEnumerator));
    }

    public void FadeOut()
    {
        StartCoroutine(nameof(FadeOutIEnumerator));
    }
    IEnumerator FadeInIEnumerator()
    {
        isFadingIn = true;

        for (float targetAlpha = fadeImage.canvasRenderer.GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            fadeImage.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.1f);
        }

        isFadingIn = false;
        StopCoroutine(nameof(FadeInIEnumerator));
    }

    IEnumerator FadeOutIEnumerator()
    {
        isFadingOut = true;

        for (float targetAlpha = fadeImage.canvasRenderer.GetAlpha(); targetAlpha > -0.1; targetAlpha -= 0.1f)
        {
            fadeImage.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.1f);
        }

        isFadingOut = false;
        roomProgress.ChangeRoomState(RoomProgress.RoomState.QuirkChoice);
        StopCoroutine(nameof(FadeInIEnumerator));
    }

    /*IEnumerator LoadLevel(int levelIndex)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }*/
}
