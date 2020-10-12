using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;

    [HideInInspector]
    public int playerIndex = 0;

    [SerializeField]
    private float interactRange;
    public GameObject interactPoint;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public LayerMask interactableLayers;

    private Animator animator;

    public GameObject crosshair;

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
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // Testing purposes only
    }

    void FixedUpdate()
    {
        //Vector2 looDir = playerMovement.m - gameObject.position;
    }

    public void LightAttack()
    {
        //Debug.Log("Light Attack!");
        // Play attack animation
        animator.Play("Player_Sword_Attack");

        if (!ranged)
        {
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.playerClass.currentAttackRange, enemyLayers);

            // Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name + " with a light attack!");

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
    }

    public void HeavyAttack()
    {
        //Debug.Log("Heavy Attack!");
        // Play attack animation

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.playerClass.currentAttackRange, enemyLayers);

        // Interactions for each enemy hit by the attack
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name + "with a heavy attack!");

            var impactEnemy = enemy.GetComponent<BasicEnemy1>();
            var impactTotem = enemy.GetComponent<DamageTotem>();

            if (impactEnemy != null)
            {
                impactEnemy.TakeDamage(gameObject, "Heavy");
                continue;
            }
            else if (impactTotem != null)
            {
                impactTotem.TotemTakeDamage(playerStats.playerClass.currentHeavyDamage);
                continue;
            }
        }     
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
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null || interactPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, playerStats.playerClass.currentAttackRange);
        Gizmos.DrawWireSphere(interactPoint.transform.position, interactRange);
    }
}
