using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss_LeftHand : MonoBehaviour
{
    private BossEnemy bossEnemy;
    private Boss_AI boss_AI;

    public enum BossHands { vulnerable, fist, gun };
    public BossHands currentHand = BossHands.vulnerable;

    private GameObject spriteGameObject;

    private int actionsThreshold = 2;

    private float fistImpactRadius = 3f;

    /// <summary>
    /// Sprite Related Variables
    /// </summary>
    private SpriteRenderer spriteRenderer;
    public Sprite VulnerableSprite;
    public Sprite FistSprite;
    public Sprite GunSprite;

    /// <summary>
    /// Tranform Variables
    /// </summary>
    private Transform topPosition;
    private Transform bottomPosition;


    private UnityEvent changedHand;

    void Awake()
    {
        bossEnemy = this.gameObject.GetComponentInParent<BossEnemy>();
        boss_AI = this.gameObject.GetComponentInParent<Boss_AI>();
        spriteGameObject = this.gameObject.transform.GetChild(2).gameObject;
        spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();

        topPosition = this.gameObject.transform.GetChild(0);
        bottomPosition = this.gameObject.transform.GetChild(1);

        if (changedHand == null)
        {
            changedHand = new UnityEvent();
            changedHand.AddListener(HandFuntcionality);
        }

        changedHand.Invoke();
    }

    #region Multi-Use Functions
    IEnumerator ChooseNewHandCoroutine()
    {
        float travelled = 0f;

        for (float currentDistance = CalcDistanceToTopPos(); currentDistance > 0; currentDistance = CalcDistanceToTopPos())
        {
            Vector3 currentPosition = spriteGameObject.transform.position;
            spriteGameObject.transform.position = Vector3.Lerp(currentPosition, topPosition.position, (travelled + 0.01f));
            travelled += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        ChooseNewHand();
        StopCoroutine(nameof(ChooseNewHandCoroutine));
    }

    private void ChooseNewHand()
    {
        int chosenIndex = 0;

        if (boss_AI.CurrentPhase == Boss_AI.BossPhases.phase1)
        {
            chosenIndex = 1;
        }
        else if (boss_AI.CurrentPhase == Boss_AI.BossPhases.phase2)
        {
            chosenIndex = Mathf.RoundToInt(Random.Range(1, 2));
        }

        if (chosenIndex == 1)
        {
            currentHand = BossHands.fist;
            changedHand.Invoke();
        }
        else if (chosenIndex == 2)
        {
            currentHand = BossHands.gun;
            //changedHand.Invoke();
        }
        else
        {
            return;
        }
    }

    private void HandFuntcionality()
    {
        switch (currentHand)
        {
            case BossHands.vulnerable:
                StartCoroutine(nameof(VulnerableCoroutine));
                break;

            case BossHands.fist:
                spriteRenderer.sprite = FistSprite;
                StartCoroutine(nameof(FistCoroutine));
                break;

            case BossHands.gun:
                spriteRenderer.sprite = GunSprite;
                break;

            default:
                break;
        }
    }

    private float CalcDistanceToTopPos()
    {
        float distance = Vector3.Distance(spriteGameObject.transform.position, topPosition.position);
        return distance;
    }

    private float CalcDistanceToBottomPos()
    {
        float distance = Vector3.Distance(spriteGameObject.transform.position, bottomPosition.position);
        return distance;
    }
    #endregion

    #region Vulnerable Functions
    public void BecomeVulnerable()
    {
        currentHand = BossHands.vulnerable;
        StopAllCoroutines();
        changedHand.Invoke();
    }

    private void VulnerableFlash()
    {
        if (spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = Color.cyan;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    IEnumerator VulnerableCoroutine()
    {
        float travelled = 0f;

        for (float currentDistance = CalcDistanceToBottomPos(); currentDistance > 0; currentDistance = CalcDistanceToBottomPos())
        {
            Vector3 currentPosition = spriteGameObject.transform.position;
            spriteGameObject.transform.position = Vector3.Lerp(currentPosition, bottomPosition.position, (travelled + 0.01f));
            travelled += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        spriteRenderer.sprite = VulnerableSprite;

        for (int flashedTimes = 0; flashedTimes < 16; flashedTimes++)
        {
            VulnerableFlash();
            yield return new WaitForSeconds(0.35f);
        }

        StartCoroutine(nameof(ChooseNewHandCoroutine));
        StopCoroutine(nameof(VulnerableCoroutine));
    }
    #endregion

    #region Fist Functions

    IEnumerator FistCoroutine()
    {
        for (int actionsPerformed = 0; actionsPerformed < actionsThreshold; actionsPerformed++)
        {
            float travelled = 0f;

            for (float currentDistance = CalcDistanceToBottomPos(); currentDistance > 0; currentDistance = CalcDistanceToBottomPos())
            {
                Vector3 currentPosition = spriteGameObject.transform.position;
                spriteGameObject.transform.position = Vector3.Lerp(currentPosition, bottomPosition.position, (travelled + 0.01f));
                travelled += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            PerformImpact();
            travelled = 0f;

            for (float currentDistance = CalcDistanceToTopPos(); currentDistance > 0; currentDistance = CalcDistanceToTopPos())
            {
                Vector3 currentPosition = spriteGameObject.transform.position;
                spriteGameObject.transform.position = Vector3.Lerp(currentPosition, topPosition.position, (travelled + 0.01f));
                travelled += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        BecomeVulnerable();
        StopCoroutine(nameof(FistCoroutine));
    }

    private void PerformImpact()
    {
        RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(spriteGameObject.transform.position, fistImpactRadius, Vector2.up);

        if (raycastHits.Length > 0)
        {
            foreach (RaycastHit2D hit in raycastHits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<PlayerStats>().TakeDamage(bossEnemy.BossEnemyClass.currentDamage);
                }
            }
        }
    }

    #endregion


}
