using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public enum WeaponState { Sword, BowAndArrow };
    public WeaponState stateWeaponSwap;

    public GameObject swordSprites;
    public GameObject bowAndArrowSprites;

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private PlayerInput playerInput;
    private PlayerSpawner playerSpawner;
    private SoundManager soundManager;
    private ItemManager itemManager;

    public GameObject arrowPrefab;

    [HideInInspector]
    public int playerIndex = 0;
    private int spawnIndex = 0;

    private Vector2 lookDirection;

    [SerializeField]
    private float interactRange;

    public GameObject interactPoint;
    public Transform attackPoint;

    public LayerMask enemyLayers;
    public LayerMask interactableLayers;

    [Space]
    [Header("Ranged Combat")]
    public float crosshairOffset;
    public GameObject crosshair;
    public bool isAiming;
    [HideInInspector]
    public bool shotReady;
    public float arrowSpeed;

    [Space]
    [Header("Combo System")]
    public static PlayerCombat instance;
    public bool canRecieveInput;
    public bool inputRecieved;

    public Animator meleeAnimator;
    public Animator rangedAnimator;

    public bool canKnockback;

    public bool ranged;

    public bool aiming;
    private Vector2 crosshairPos;

    //public float attackRange = 0.5f;
    //public float lightAttackDamage = 3f;
    //public float heavyAttackDamage = 5f;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        soundManager = FindObjectOfType<SoundManager>();
        itemManager = FindObjectOfType<ItemManager>();

        playerInput = GetComponentInChildren<PlayerInput>();

        instance = this;

        gameObject.transform.position = playerSpawner.spawnPoints[playerSpawner.spawnIndex].transform.position;
        playerSpawner.SetIndex();
    }

    void Start()
    {
        #region For Testing Purposes only
        ranged = false;
        #endregion

        crosshair.SetActive(false);

        //OnDrawGizmosSelected();

        canKnockback = false;
        canRecieveInput = true;

        //Debug.Log(playerInput.playerIndex);
    }

    void Update()
    {
        switch (stateWeaponSwap)
        {
            case WeaponState.Sword:
                ranged = false;
                swordSprites.SetActive(true);
                bowAndArrowSprites.SetActive(false);
                break;
            case WeaponState.BowAndArrow:
                ranged = true;
                swordSprites.SetActive(false);
                bowAndArrowSprites.SetActive(true);
                break;
        }

        if (isAiming && ranged)
		{
			Debug.Log("RefreshCheck");
            //playerMovement.restrictMovement = true;
            playerMovement.isAiming = true;
            crosshair.SetActive(true);
            Aim();
        }
    }

    public void WeaponSwap()
    {
        if (ranged)
        {
            stateWeaponSwap = WeaponState.Sword;
            return;
        }

        stateWeaponSwap = WeaponState.BowAndArrow;
    }

    public void SetInputAimVector(Vector2 direction)
    {
        lookDirection = direction;
        lookDirection *= crosshairOffset;
        //Debug.Log("Vector Set!");
    }

    private void Aim()
    {
        // Crosshair placement
		Debug.Log("Aiming");       
        crosshair.transform.localPosition = lookDirection;
        rangedAnimator.SetTrigger("Nock");
    }

    public void ReadyShot()
    {
        shotReady = true;
    }

    public void MeleeAttack()
    {
        //playerMovement.restrictMovement = true;

        #region Hit Check
        if (!ranged && canRecieveInput)
        {
            //Debug.Log("Player " + playerInput.playerIndex + " attacking");

            //Debug.Log("Attack!");
            canRecieveInput = false;

                // Play attack animation
                //animator.Play("Player_Sword_Attack");
                // Detect enemies in range of attack
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.playerClass.currentAttackRange, enemyLayers);

            // Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                //Debug.Log("We hit " + enemy.name + " with a light attack!");

                var impactEnemy = enemy.GetComponent<SkeletonEnemy>();
                var impactBird = enemy.GetComponent<CrowEnemy>();
                var impactTotem = enemy.GetComponent<DamageTotem>();
                var impactBoss = enemy.transform.parent.transform.parent.GetComponentInParent<BossEnemy>();

                //Checks if player hit any enemies
                if (impactEnemy != null)
                {
                    if (canKnockback)
                    {                       
                        enemy.GetComponent<SkeletonEnemy_AI>().Knockback();
                        soundManager.Play("Enemy Knockback");
                        impactEnemy.TakeDamage(gameObject, "Light");
                    }
                    else
                    {
                        soundManager.Play("Enemy Impact");
                        impactEnemy.TakeDamage(gameObject, "Light");
                    }
                    
                    continue;
                }

                //Checks if player hit any totems
                if (impactTotem != null)
                {
                    if (canKnockback)
                    {
                        Debug.Log("Damage totem!");
                        soundManager.Play("Enemy Knockback");
                        impactTotem.TotemTakeDamage(playerStats.playerClass.currentLightDamage);
                    }
                    else
                    {
                        soundManager.Play("Enemy Impact");
                        impactTotem.TotemTakeDamage(playerStats.playerClass.currentLightDamage);
                    }

                    continue;
                }

                if (impactBird!= null)
                {
                    if (canKnockback)
                    {
                        Debug.Log("Damage Bird Boy!");
                        soundManager.Play("Enemy Knockback");
                        impactBird.TakeDamage(gameObject, "Light");
                    }
                    else
                    {
                        soundManager.Play("Enemy Impact");
                        impactBird.TakeDamage(gameObject, "Light");
                    }
                    
                    continue;
                }

                if(impactBoss != null)
                {
                    if (canKnockback)
                    {
                        Debug.Log("Damaged boss boy!");
                        soundManager.Play("Enemy Knockback");
                        impactBoss.TakeDamage(gameObject, "Light");
                    }
                    else
                    {
                        soundManager.Play("Enemy Impact");
                        impactBoss.TakeDamage(gameObject, "Light");
                    }

                    continue;
                }
            }
        }
        else
        {
            return;
        }
        #endregion

        //playerMovement.restrictMovement = false;
    }

    public void Fire()
	{
		if (lookDirection != Vector2.zero && playerMovement.isMoving == false && ranged && shotReady == true)
        {			
			Debug.Log("Loose!");
            // Play attack animation

            Vector2 fireDirection = crosshair.transform.localPosition;
            fireDirection.Normalize();

            rangedAnimator.SetTrigger("Loose");

            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrow.GetComponent<Projectile>().bulletMaster = gameObject;
            arrow.GetComponent<Rigidbody2D>().velocity = lookDirection * arrowSpeed;
            arrow.transform.Rotate(0, 0, Mathf.Atan2(-lookDirection.y, -lookDirection.x) * Mathf.Rad2Deg);
            Destroy(arrow, 2f);
			Invoke("ResetMovement", .25f);
			crosshair.SetActive(false);
			isAiming = false;
            playerMovement.isAiming = false;
            rangedAnimator.ResetTrigger("Nock");
            shotReady = false;
        }
		else
		{
            rangedAnimator.SetTrigger("NoFire");
            rangedAnimator.ResetTrigger("Nock");
            //playerMovement.restrictMovement = false;
			//ResetMovement();
            playerMovement.isAiming = false;
            crosshair.SetActive(false);
			isAiming = false;
            shotReady = false;
			return;
		}

		#region Heavy Attack
		// Detect enemies in range of attack
		//Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.playerClass.currentAttackRange, enemyLayers);
		//if (!ranged)
		//{
		//    // Interactions for each enemy hit by the attack
		//    foreach (Collider2D enemy in hitEnemies)
		//    {
		//        Debug.Log("We hit" + enemy.name + "with a heavy attack!");

		//        var impactEnemy = enemy.GetComponent<BasicEnemy1>();
		//        var impactTotem = enemy.GetComponent<DamageTotem>();

		//        if (impactEnemy != null)
		//        {
		//            impactEnemy.TakeDamage(gameObject, "Heavy");
		//            continue;
		//        }
		//        else if (impactTotem != null)
		//        {
		//            impactTotem.TotemTakeDamage(playerStats.playerClass.currentHeavyDamage);
		//            continue;
		//        }
		//    } 
		//}
		#endregion
	}

	public void ResetMovement()
	{
		playerMovement.restrictMovement = false;
	}

    public void Interact()
    {
        //Debug.Log("Interacted!");

        // Detect enemies in range of attack
        Collider2D[] hitInteractables = Physics2D.OverlapCircleAll(interactPoint.transform.position, interactRange, interactableLayers);

        foreach (Collider2D interactable in hitInteractables)
        {
            //Debug.Log("Interacted with " + interactable.name);

            if (interactable.CompareTag("ItemPedistool"))
            {
                //playerStats.UpdateCurrentItem(interactable);

                //This is speed up item.
                if (interactable.gameObject.GetComponent<ItemPedistool>().currentConsumableItem == itemManager.allConsumablesArray[0])
                {
                    playerStats.playerClass.currentMovementSpeed += itemManager.allConsumablesArray[0].effectModifier;
                }
                //This is damage up item.
                else if (interactable.gameObject.GetComponent<ItemPedistool>().currentConsumableItem == itemManager.allConsumablesArray[1])
                {
                    playerStats.playerClass.currentLightDamage += itemManager.allConsumablesArray[1].effectModifier;
                }
                //This is resistance up item
                else if (interactable.gameObject.GetComponent<ItemPedistool>().currentConsumableItem == itemManager.allConsumablesArray[2])
                {
                    playerStats.playerClass.currentResistance += itemManager.allConsumablesArray[2].effectModifier;
                }
                //This is the potion item.
                else if (interactable.gameObject.GetComponent<ItemPedistool>().currentConsumableItem == itemManager.allConsumablesArray[3])
                {
                    playerStats.potCount += Mathf.RoundToInt(itemManager.allConsumablesArray[3].effectModifier);
                }

                interactable.gameObject.GetComponent<ItemPedistool>().ClearPedistool();

                //Debug.Log("Item Changed");

                continue;
            }
            if (interactable.CompareTag("ExitDoor"))
            {
                interactable.GetComponent<ExitDoorFunc>().ExitRoom();
            }
            if (interactable.CompareTag("QuirkCard"))
            {
                interactable.GetComponent<QuirkSelection>().ActivateQuirk();
            }
        }

    }

    public void InputManager()
    {
        if (!canRecieveInput)
        {
            canRecieveInput = true;
        }
        else
        {
            canRecieveInput = false;
        }
    }

    public void ActiveItem()
    {
        Debug.Log("Used active item!");

        playerStats.AddHealth();
    }

    public int GetPlayerIndex()
    {
        // Returns the index of the player (Index 0-3/Player 1-4) 
        return playerIndex;
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (attackPoint == null || interactPoint == null)
    //        return;

    //    Gizmos.DrawWireSphere(attackPoint.position, playerStats.playerClass.currentAttackRange);
    //    Gizmos.DrawWireSphere(interactPoint.transform.position, interactRange);
    //}
}
