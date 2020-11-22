using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuirkPickerUI : MonoBehaviour
{
    private QuirkManager quirkManager;

    public GameObject quirkUIParent;
    public GameObject quirk1UI;
    public GameObject quirk2UI;
    public GameObject quirk3UI;

    private float yPosTarget = 600f;
    private float yPosOffScreen = 1500f;

    private RoomProgress currentRoomProgress;
    private void Awake()
    {
        quirkManager = this.gameObject.GetComponent<QuirkManager>();
        AssignCurrentRoomReference();

        SetChildrenAlpha(0, quirk1UI);
        SetChildrenAlpha(0, quirk2UI);
        SetChildrenAlpha(0, quirk3UI);
        
        MoveUIOffScreen();
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

    public void SetUIText()
    {
        quirk1UI.GetComponentInChildren<TMP_Text>().SetText(quirkManager.QuirkChoices[0].quirkName);
        quirk2UI.GetComponentInChildren<TMP_Text>().SetText(quirkManager.QuirkChoices[1].quirkName);
        quirk3UI.GetComponentInChildren<TMP_Text>().SetText(quirkManager.QuirkChoices[2].quirkName);
    }
    public void BeginFadeInUI()
    {
        TurnOnUI();
        StartCoroutine(nameof(FadeInIEnumerator));
    }

    public void BeginFadeOutUI()
    {
        StartCoroutine(nameof(FadeOutIEnumerator));
    }

    IEnumerator FadeInIEnumerator()
    {
        yield return new WaitForSeconds(0.5f);

        for (float targetAlpha = quirk1UI.GetComponent<CanvasRenderer>().GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            SetChildrenAlpha(targetAlpha, quirk1UI);
            SetChildrenAlpha(targetAlpha, quirk2UI);
            SetChildrenAlpha(targetAlpha, quirk3UI);
            yield return new WaitForSeconds(0.1f);
        }

        StopCoroutine(nameof(FadeInIEnumerator));
    }

    IEnumerator FadeOutIEnumerator()
    {

        for (float targetAlpha = quirk1UI.GetComponent<CanvasRenderer>().GetAlpha(); targetAlpha > -0.1; targetAlpha -= 0.1f)
        {
            SetChildrenAlpha(targetAlpha, quirk1UI);
            SetChildrenAlpha(targetAlpha, quirk2UI);
            SetChildrenAlpha(targetAlpha, quirk3UI);
            yield return new WaitForSeconds(0.1f);
        }

        currentRoomProgress.ChangeRoomState(RoomProgress.RoomState.Active);
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

    private void TurnOffUI()
    {
        MoveUIOffScreen();
        quirkUIParent.SetActive(false);
    }

    private void TurnOnUI()
    {
        quirkUIParent.SetActive(true);
        MoveUIOnScreen();
        
    }

    void MoveUIOffScreen()
    {
        quirk1UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk1UI.GetComponent<RectTransform>().position.x, yPosOffScreen, quirk1UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk2UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk2UI.GetComponent<RectTransform>().position.x, yPosOffScreen, quirk2UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk3UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk3UI.GetComponent<RectTransform>().position.x, yPosOffScreen, quirk3UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
    }

    void MoveUIOnScreen()
    {
        quirk1UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk1UI.GetComponent<RectTransform>().position.x, yPosTarget, quirk1UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk2UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk2UI.GetComponent<RectTransform>().position.x, yPosTarget, quirk2UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk3UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk3UI.GetComponent<RectTransform>().position.x, yPosTarget, quirk3UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
    }

    public void AssignCurrentRoomReference()
    {
        currentRoomProgress = FindObjectOfType<RoomProgress>();
    }
}
