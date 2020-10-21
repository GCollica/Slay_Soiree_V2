﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;

    public GameObject arrowPrefab;

    [HideInInspector]
    public int playerIndex = 0;

    private Vector2 lookDirection;

    [SerializeField]
    [Space]
    [Header("Player interact range")]
    private float interactRange;

    public GameObject interactPoint;
    public Transform attackPoint;

    [Space]
    [Header("Layer that player can attack")]
    public LayerMask enemyLayers;

    [Space]
    [Header("Layer that player can interact with")]
    public LayerMask interactableLayers;

    private Animator animator;

    [Space]
    [Header("Ranged Combat")]
    public float crosshairOffset;
    public GameObject crosshair;
    public bool isAiming;
    public float arrowSpeed;

    [SerializeField]
    private bool ranged;

    public bool aiming;
    private Vector2 crosshairPos;

    //public float attackRange = 0.5f;
    //public float lightAttackDamage = 3f;
    //public float heavyAttackDamage = 5f;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        #region For Testing Purposes only
        ranged = true;
        #endregion

        crosshair.SetActive(false);
        
    }

    void Update()
    {
		if (isAiming)
		{
			Debug.Log("RefreshCheck");
            playerMovement.isAiming = true;
            Aim();
        }
    }

    public void SetInputAimVector(Vector2 direction)
    {
        lookDirection = direction;

        Debug.Log("Vector Set!");
    }

    private void Aim()
    {
        // Crosshair placement
		Debug.Log("Aiming");
        crosshair.SetActive(true);
        crosshair.transform.localPosition = lookDirection;
    }

    public void LightAttack()
    {
        //Debug.Log("Light Attack!");
        
        #region Hit Check
        if (!ranged)
        {
            // Play attack animation
            animator.Play("Player_Sword_Attack");
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.playerClass.currentAttackRange, enemyLayers);

            // Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                //Debug.Log("We hit " + enemy.name + " with a light attack!");

                var impactEnemy = enemy.GetComponent<BasicEnemy1>();
                var impactTotem = enemy.GetComponent<DamageTotem>();

                if (impactEnemy != null)
                {
                    impactEnemy.TakeDamage(gameObject, "Light");
                    continue;
                }

                if (impactTotem != null)
                {
                    Debug.Log("Damage totem!");
                    impactTotem.TotemTakeDamage(playerStats.playerClass.currentLightDamage);
                    continue;
                }
            }
        }
        #endregion
    }

    public void Fire()
	{

		if (lookDirection != Vector2.zero && playerMovement.isMoving == false)
        {			
			Debug.Log("Loose!");
            // Play attack animation

            Vector2 fireDirection = crosshair.transform.localPosition;
            fireDirection.Normalize();

            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = lookDirection * arrowSpeed;
            arrow.transform.Rotate(0, 0, Mathf.Atan2(-lookDirection.y, -lookDirection.x) * Mathf.Rad2Deg);
            Destroy(arrow, 2f);
			Invoke("ResetMovement", .25f);
			crosshair.SetActive(false);
			isAiming = false;
		}
		else
		{
			playerMovement.isAiming = false;
			ResetMovement();
			crosshair.SetActive(false);
			isAiming = false;
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
		playerMovement.isAiming = false;
	}

    public void Interact()
    {
        //Debug.Log("Interacted!");

        // Detect enemies in range of attack
        Collider2D[] hitInteractables = Physics2D.OverlapCircleAll(interactPoint.transform.position, interactRange, interactableLayers);

        foreach (Collider2D interactable in hitInteractables)
        {
            Debug.Log("Interacted with " + interactable.name);

            if (interactable.CompareTag("ItemPedistool"))
            {
                playerStats.UpdateCurrentItem(interactable);
                interactable.gameObject.GetComponent<ItemPedistool>().ClearPedistool();
                Debug.Log("Item Changed");
                continue;
            }
        }

    }

    public void ActiveItem()
    {
        Debug.Log("Used active item!");
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
