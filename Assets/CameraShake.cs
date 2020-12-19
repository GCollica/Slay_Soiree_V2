using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float xIntensity = .05f;
    private float yIntensity = .05f;
    private float duration = 0.5f;

    public bool isShaking = false;

    // Start is called before the first frame update
    void Awake()
    {
        iTween.Init(this.gameObject);
    }

    public void BeginScreenShake()
    {
        if (isShaking == true)
        {
            return;
        }

        Debug.Log("Shaking Screen");
        StartCoroutine(nameof(ShakeScreen));
    }

    IEnumerator ShakeScreen()
    {
        isShaking = true;

        iTween.ShakePosition(this.gameObject, new Vector3(xIntensity, yIntensity, 0f), duration);

        yield return new WaitForSeconds(duration);

        isShaking = false;
        StopCoroutine(nameof(ShakeScreen));
    }

}
