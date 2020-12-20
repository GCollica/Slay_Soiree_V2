using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    public BossEnemy_Class BossEnemyClass;
    private Boss_AI bossEnemy_AI;
    private CameraShake cameraShake;
    private SoundManager soundManager;

    private float startingDamage = 30f;
    private float startingResistance = 50f;
    private float startingHealth = 300f;
    private float startingMovespeed = 50f;

    public Transform HealthbarPosition;
    public Transform WinBannerPosition;
    public GameObject HealthbarGameObject;
    public GameObject WinBannerGameObject;
    public Image HealthBarBorder_Image;
    public Image HealthBarFill_Image;
    public Image WinBanner_Image;

    public bool LeftHandDead = false;
    public bool RightHandDead = false;

    private void Awake()
    {
        InitialiseClassInstance();
        bossEnemy_AI = GetComponent<Boss_AI>();
        cameraShake = FindObjectOfType<CameraShake>();
        soundManager = FindObjectOfType<SoundManager>();
        StartCoroutine(nameof(FadeInHealthbarUICoroutine));
    }

    private void Update()
    {
        HealthBarFill_Image.fillAmount = BossEnemyClass.currentHealth / startingHealth;

        if(LeftHandDead == true && RightHandDead == true)
        {
            BossDead();
            LeftHandDead = false;
            RightHandDead = false;
        }
    }

    private void InitialiseClassInstance()
    {
        BossEnemyClass = new BossEnemy_Class(startingDamage, startingResistance, startingHealth, startingMovespeed);
    }

    public void TakeDamage(GameObject player, string attackType)
    {
        if (attackType == "Heavy")
        {
            BossEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentHeavyDamage);
        }
        else if (attackType == "Light")
        {
            BossEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentLightDamage);
        }

        if(BossEnemyClass.currentHealth / startingHealth <= 0.4f)
        {
            bossEnemy_AI.CurrentPhase = Boss_AI.BossPhases.phase2;
        }

        // If enemy health drops below zero
        if (BossEnemyClass.currentHealth <= 0)
        {
            bossEnemy_AI.CurrentPhase = Boss_AI.BossPhases.dead;
        }
    }

    private void BossDead()
    {
        StartCoroutine(nameof(FadeInWinBannerUICoroutine));
    }

    public void ShakeCamera()
    {
        cameraShake.BeginScreenShake();
    }

    public void PlayAudio(string clipName)
    {
        soundManager.Play(clipName);
    }

    IEnumerator FadeInHealthbarUICoroutine()
    {
        HealthBarBorder_Image.canvasRenderer.SetAlpha(0);
        HealthBarFill_Image.canvasRenderer.SetAlpha(0);

        HealthbarGameObject.transform.position = HealthbarPosition.position;

        for (float targetAlpha = HealthBarBorder_Image.canvasRenderer.GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            HealthBarBorder_Image.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.125f);
        }

        for (float targetAlpha = HealthBarFill_Image.canvasRenderer.GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            HealthBarFill_Image.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.1f);
        }

        StopCoroutine(nameof(FadeInHealthbarUICoroutine));
    }

    IEnumerator FadeInWinBannerUICoroutine()
    {
        WinBanner_Image.canvasRenderer.SetAlpha(0);

        WinBannerGameObject.transform.position = WinBannerPosition.position;

        for (float targetAlpha = WinBanner_Image.canvasRenderer.GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            WinBanner_Image.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.125f);
        }

        StopCoroutine(nameof(FadeInWinBannerUICoroutine));
    }
}
