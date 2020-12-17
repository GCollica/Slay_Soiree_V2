using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss_RightHand : MonoBehaviour
{
    private BossEnemy bossEnemy;
    private Boss_AI boss_AI;

    public enum BossHands { vulnerable, fist, gun };
    public BossHands currentHand = BossHands.vulnerable;

    private GameObject spriteGameObject;
    public GameObject BulletRight_Prefab;
    public GameObject bulletParent;
    private ShootPositions shootPositions;

    private int actionsThreshold = 2;

    private float fistImpactRadius = 5f;

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
        shootPositions = this.gameObject.transform.GetChild(2).gameObject.GetComponent<ShootPositions>();
        spriteGameObject = this.gameObject.transform.GetChild(3).gameObject;
        spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();

        topPosition = this.gameObject.transform.GetChild(0);
        bottomPosition = this.gameObject.transform.GetChild(1);

        if (changedHand == null)
        {
            changedHand = new UnityEvent();
            changedHand.AddListener(HandFunctionality);
        }
    }

    #region Multi-Use Functions

    public void BeginEncounter()
    {
        StartCoroutine(nameof(OpeningCoroutine));
    }

    IEnumerator OpeningCoroutine()
    {
        for (int i = 0; i < 9; i++)
        {
            if (spriteRenderer.sprite == FistSprite)
            {
                spriteRenderer.sprite = VulnerableSprite;
            }
            else if (spriteRenderer.sprite == VulnerableSprite)
            {
                spriteRenderer.sprite = GunSprite;
            }
            else if (spriteRenderer.sprite == GunSprite)
            {
                spriteRenderer.sprite = FistSprite;
            }

            yield return new WaitForSeconds(.25f);

        }

        StartCoroutine(nameof(ChooseNewHandCoroutine));
        StopCoroutine(nameof(OpeningCoroutine));

    }

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
            chosenIndex = Mathf.RoundToInt(Random.Range(1, 3));
        }

        if (chosenIndex == 1)
        {
            currentHand = BossHands.fist;
            changedHand.Invoke();
        }
        else if (chosenIndex == 2)
        {
            currentHand = BossHands.gun;
            changedHand.Invoke();
        }
        else
        {
            return;
        }
    }

    private void HandFunctionality()
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
                StartCoroutine(nameof(GunCoroutine));
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
                spriteGameObject.transform.position = Vector3.Lerp(currentPosition, bottomPosition.position, (travelled + 0.03f));
                travelled += 0.03f;
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

        float travelledFinal = 0f;

        for (float currentDistance = CalcDistanceToBottomPos(); currentDistance > 0; currentDistance = CalcDistanceToBottomPos())
        {
            Vector3 currentPosition = spriteGameObject.transform.position;
            spriteGameObject.transform.position = Vector3.Lerp(currentPosition, bottomPosition.position, (travelledFinal + 0.03f));
            travelledFinal += 0.03f;
            yield return new WaitForSeconds(0.01f);
        }

        PerformImpact();

        yield return new WaitForSeconds(0.5f);

        BecomeVulnerable();
        StopCoroutine(nameof(FistCoroutine));
    }

    private void PerformImpact()
    {
        //Debug.Log("Performed Attack");

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

    #region Gun Functions;
    IEnumerator GunCoroutine()
    {
        float travelled = 0f;

        for (float currentDistance = CalcDistanceToBottomPos(); currentDistance > 0; currentDistance = CalcDistanceToBottomPos())
        {
            Vector3 currentPosition = spriteGameObject.transform.position;
            spriteGameObject.transform.position = Vector3.Lerp(currentPosition, bottomPosition.position, (travelled + 0.01f));
            travelled += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(.25f);

        for (int actionsPerformed = 0; actionsPerformed < actionsThreshold; actionsPerformed++)
        {
            int chosenPos = ChooseShootPos();
            travelled = 0f;

            for (float currentDistance = CalcDistanceToShootPos(chosenPos); currentDistance > 0; currentDistance = CalcDistanceToShootPos(chosenPos))
            {
                Vector3 currentPosition = spriteGameObject.transform.position;
                spriteGameObject.transform.position = Vector3.Lerp(currentPosition, shootPositions.shootPositions[chosenPos].position, (travelled + 0.02f));
                travelled += 0.02f;
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(0.5f);
            ShootBullet();
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(nameof(ChooseNewHandCoroutine));
        StopCoroutine(nameof(GunCoroutine));
    }

    private int ChooseShootPos()
    {
        int chosenPos = Random.Range(0, shootPositions.shootPositions.Length - 1);
        return chosenPos;
    }

    private float CalcDistanceToShootPos(int chosenShootPos)
    {
        float distance = Vector3.Distance(spriteGameObject.transform.position, shootPositions.shootPositions[chosenShootPos].position);
        return distance;
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(BulletRight_Prefab, bulletParent.transform);
    }
    #endregion;
}
