using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    public BossEnemy_Class BossEnemyClass;

    public float startingDamage = 30f;
    public float startingResistance = 50f;
    public float startingHealth = 100f;
    public float startingMovespeed = 50f;

    public Transform HealthbarPosition;
    public GameObject HealthbarGameObject;
    public Image HealthBarBorder_Image;
    public Image HealthBarFill_Image;

    private void Awake()
    {
        InitialiseClassInstance();
        StartCoroutine(nameof(FadeInUICoroutine));
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

        // If enemy health drops below zero
        if (BossEnemyClass.currentHealth <= 0)
        {
            // Death animation here.
            BeginEnemyDeath();
        }
    }

    private void BeginEnemyDeath()
    {
        //WinState
    }

    IEnumerator FadeInUICoroutine()
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

        StopCoroutine(nameof(FadeInUICoroutine));
    }
}
