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

    private int actionsPerformed;
    private int actionsThreshold = 3;

    /// <summary>
    /// Sprite Related Variables
    /// </summary>
    private SpriteRenderer spriteRenderer;
    public Sprite VulnerableSprite;
    public Sprite FistSprite;
    public Sprite GunSprite;

    /// <summary>
    /// Tranforms
    /// </summary>
    private Transform topPosition;
    private Transform bottomPosition;


    private UnityEvent changedHand;

    // Start is called before the first frame update
    void Awake()
    {
        bossEnemy = this.gameObject.GetComponentInParent<BossEnemy>();
        boss_AI = this.gameObject.GetComponentInParent<Boss_AI>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        topPosition = this.gameObject.transform.GetChild(0).transform;
        bottomPosition = this.gameObject.transform.GetChild(1).transform;

        if(changedHand == null)
        {
            changedHand = new UnityEvent();
            changedHand.AddListener(HandFuntcionality);
        }
    }

    // Update is called once per frame
    void Update() 
    {
        

    }

    private void ChooseNewHand()
    {
        int chosenIndex = 0;

        if(boss_AI.CurrentPhase == Boss_AI.BossPhases.phase1)
        {
            chosenIndex = 1;
        }
        else if(boss_AI.CurrentPhase == Boss_AI.BossPhases.phase2)
        {
            chosenIndex = Mathf.RoundToInt(Random.Range(1, 2));
        }
        
        if(chosenIndex == 1)
        {
            currentHand = BossHands.fist;
            changedHand.Invoke();
        }
        else if(chosenIndex == 2)
        {
            currentHand = BossHands.gun;
            changedHand.Invoke();
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
                spriteRenderer.sprite = VulnerableSprite;
                StartCoroutine(nameof(VulnerableCoroutine));
                break;

            case BossHands.fist:
                spriteRenderer.sprite = FistSprite;
                break;

            case BossHands.gun:
                spriteRenderer.sprite = GunSprite;
                break;

            default:
                break;
        }
    }

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
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator VulnerableCoroutine()
    {
        actionsPerformed = 0;

        /*for (float currentTime = 0; currentTime < ; currentTime++)
        {

        }*/

        for (int flashedTimes = 0; flashedTimes == 9; flashedTimes ++)
        {
            VulnerableFlash();
            yield return new WaitForSeconds(0.5f);
        }

        ChooseNewHand();
        StopCoroutine(nameof(VulnerableCoroutine));
    }
    #endregion

    #region Pinch Functions



    #endregion

    #region Fist Functions


    #endregion


}
