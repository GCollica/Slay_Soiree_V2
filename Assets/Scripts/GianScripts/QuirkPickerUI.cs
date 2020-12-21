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
    public GameObject pickQuirkText;

    public Collider2D collider1;
    public Collider2D collider2;
    public Collider2D collider3;

    //public GameObject cardSelectColliders;

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
        SetChildrenAlpha(0, pickQuirkText);

        //cardSelectColliders.SetActive(false);
        ColliderOff();

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
            //cardSelectColliders.SetActive(true);

            SetChildrenAlpha(targetAlpha, quirk1UI);
            SetChildrenAlpha(targetAlpha, quirk2UI);
            SetChildrenAlpha(targetAlpha, quirk3UI);
            SetChildrenAlpha(targetAlpha, pickQuirkText);
            yield return new WaitForSeconds(0.05f);
        }

        ColliderOn();
        StopCoroutine(nameof(FadeInIEnumerator));
    }

    IEnumerator FadeOutIEnumerator()
    {
        ColliderOff();

        for (float targetAlpha = quirk1UI.GetComponent<CanvasRenderer>().GetAlpha(); targetAlpha > -0.1; targetAlpha -= 0.1f)
        {
            //cardSelectColliders.SetActive(false);

            SetChildrenAlpha(targetAlpha, quirk1UI);
            SetChildrenAlpha(targetAlpha, quirk2UI);
            SetChildrenAlpha(targetAlpha, quirk3UI);
            SetChildrenAlpha(targetAlpha, pickQuirkText);
            yield return new WaitForSeconds(0.05f);
        }

        currentRoomProgress.ChangeRoomState(RoomProgress.RoomState.Active);
        Invoke(nameof(TurnOffUI), 0.1f);
        StopCoroutine(nameof(FadeInIEnumerator));

    }

    void ColliderOn()
    {
        collider1.enabled = true;
        collider2.enabled = true;
        collider3.enabled = true;
    }

    void ColliderOff()
    {
        collider1.enabled = false;
        collider2.enabled = false;
        collider3.enabled = false;
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
        SetChildrenAlpha(0f, quirk1UI);
        SetChildrenAlpha(0f, quirk2UI);
        SetChildrenAlpha(0f, quirk3UI);
        ColliderOff();
        MoveUIOnScreen();
        
    }

    void MoveUIOffScreen()
    {
        quirk1UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk1UI.GetComponent<RectTransform>().position.x, yPosOffScreen, quirk1UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk2UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk2UI.GetComponent<RectTransform>().position.x, yPosOffScreen, quirk2UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk3UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk3UI.GetComponent<RectTransform>().position.x, yPosOffScreen, quirk3UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        pickQuirkText.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(pickQuirkText.GetComponent<RectTransform>().position.x, yPosTarget + 350f, pickQuirkText.GetComponent<RectTransform>().position.z), Quaternion.identity);
    }

    void MoveUIOnScreen()
    {
        quirk1UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk1UI.GetComponent<RectTransform>().position.x, yPosTarget, quirk1UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk2UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk2UI.GetComponent<RectTransform>().position.x, yPosTarget, quirk2UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        quirk3UI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(quirk3UI.GetComponent<RectTransform>().position.x, yPosTarget, quirk3UI.GetComponent<RectTransform>().position.z), Quaternion.identity);
        pickQuirkText.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(pickQuirkText.GetComponent<RectTransform>().position.x, yPosTarget + 350f, pickQuirkText.GetComponent<RectTransform>().position.z), Quaternion.identity);
    }

    public void AssignCurrentRoomReference()
    {
        currentRoomProgress = FindObjectOfType<RoomProgress>();
    }
}
