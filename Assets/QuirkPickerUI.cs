using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkPickerUI : MonoBehaviour
{
    private QuirkManager quirkManager;

    private bool isFadingIn = false;
    private bool isFadingOut = false;

    public GameObject quirkUIParent;
    public GameObject quirk1UI;
    public GameObject quirk2UI;
    public GameObject quirk3UI;
    private void Awake()
    {
        quirkManager = this.gameObject.GetComponent<QuirkManager>();

        SetChildrenAlpha(0, quirk1UI);
        SetChildrenAlpha(0, quirk2UI);
        SetChildrenAlpha(0, quirk3UI);

        StartCoroutine(nameof(FadeInIEnumerator));

    }

    public void PickQuirk1Button()
    {
        quirkManager.SetCurrentQuirk(quirkManager.QuirkChoices[0].quirkID);
        BeginFadeOutUI();
    }

    public void PickQuirk2Button()
    {
        quirkManager.SetCurrentQuirk(quirkManager.QuirkChoices[1].quirkID);
        BeginFadeOutUI();
    }

    public void PickQuirk3Button()
    {
        quirkManager.SetCurrentQuirk(quirkManager.QuirkChoices[2].quirkID);
        BeginFadeOutUI();
    }

    public void BeginFadeInUI()
    {
        StartCoroutine(nameof(FadeInIEnumerator));
    }

    public void BeginFadeOutUI()
    {
        StartCoroutine(nameof(FadeOutIEnumerator));
    }

    IEnumerator FadeInIEnumerator()
    {
        yield return new WaitForSeconds(0.5f);
        
        isFadingIn = true;

        for (float targetAlpha = quirk1UI.GetComponent<CanvasRenderer>().GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            SetChildrenAlpha(targetAlpha, quirk1UI);
            SetChildrenAlpha(targetAlpha, quirk2UI);
            SetChildrenAlpha(targetAlpha, quirk3UI);
            yield return new WaitForSeconds(0.1f);
        }

        isFadingIn = false;
        StopCoroutine(nameof(FadeInIEnumerator));
    }

    IEnumerator FadeOutIEnumerator()
    {
        isFadingOut = true;

        for (float targetAlpha = quirk1UI.GetComponent<CanvasRenderer>().GetAlpha(); targetAlpha > -0.1; targetAlpha -= 0.1f)
        {
            SetChildrenAlpha(targetAlpha, quirk1UI);
            SetChildrenAlpha(targetAlpha, quirk2UI);
            SetChildrenAlpha(targetAlpha, quirk3UI);
            yield return new WaitForSeconds(0.1f);
        }

        isFadingOut = false;
        Invoke(nameof(TurnOffUI), 0.25f);
        StopCoroutine(nameof(FadeInIEnumerator));
    }

    void SetChildrenAlpha(float targetAlpha, GameObject targetQuirkUI)
    {
        foreach (CanvasRenderer canvasRend in targetQuirkUI.GetComponentsInChildren<CanvasRenderer>())
        {
            canvasRend.SetAlpha(targetAlpha);
        }
    }

    void TurnOffUI()
    {
        quirkUIParent.SetActive(false);
    }
}
