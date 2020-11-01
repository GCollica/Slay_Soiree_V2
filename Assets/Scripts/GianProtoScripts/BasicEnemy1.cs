using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy1 : MonoBehaviour
{
    /*Script that will be attached to each basic enemy 1 gameobject throughout the game. Holds individual values for damage, resistance, health & movement speed and feeds that into it's own instance of the BasicEnemyClass. */
    public BasicEnemyClass basicEnemyClass;
    private Enemy_AI basicEnemyAI;
    private QuirkManager quirkManager;
    private WaveManager waveManager;
    private Enemy_Animation animationComponent;
    private EnemyAIPathfinding pathfindingComponent;

    public SpriteRenderer spriteRenderer;

    public float startingDamage = 5f;
    public float startingResistance = 10f;
    public float startingHealth = 25f;
    public float startingMovespeed = 10f;
    public int goldDrop = 2;

    private GameObject rewardPlayer;
    
    void Awake()
    {
        basicEnemyAI = this.gameObject.GetComponent<Enemy_AI>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<Enemy_Animation>();
        pathfindingComponent = this.gameObject.transform.GetComponentInChildren<EnemyAIPathfinding>();
        if(this.gameObject.transform.GetChild(1) != null)
        {
            spriteRenderer = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        if (gameObject.name == "BOSS")
        {
            InitaliseBossClassInstance();
        }
        else
        {
            InitialiseClassInstance();
        }

        waveManager = FindObjectOfType<WaveManager>();
        waveManager.AddActiveEnemy(this.gameObject);
        quirkManager = FindObjectOfType<QuirkManager>();
        
        if(quirkManager.CurrentQuirk.quirkID == 2)
        {
            int chosenModifier = quirkManager.RandomiseMSModifier();
            if(chosenModifier == 0)
            {
                basicEnemyClass.currentMovementSpeed = (basicEnemyClass.currentMovementSpeed / 2);
            }
            if(chosenModifier == 1)
            {
                basicEnemyClass.currentMovementSpeed = (basicEnemyClass.currentMovementSpeed * 2);
            }
        }

        //SetSortingLayers();

        //InvokeRepeating("SetSortingLayers", 0.5f, 0.5f);
    }

    //Initialises an instance of the Basic Enemy Class, feeding it values for damage, resistance, health, movespeed as the constructor requires.
    private void InitialiseClassInstance()
    {
        basicEnemyClass = new BasicEnemyClass(startingDamage, startingResistance, startingHealth, startingMovespeed, goldDrop);
    }

    /*private void SetSortingLayers()
    {
        spriteRenderer.sortingLayerName = Mathf.RoundToInt(this.gameObject.transform.position.y).ToString();
        spriteRenderer.sortingOrder = Mathf.RoundToInt(this.gameObject.transform.position.x) * 10;
    }*/

    private void InitaliseBossClassInstance()
    {
        startingDamage = 25f;
        startingResistance = 12f;
        startingHealth = 125f;
        startingMovespeed = 10f;

        basicEnemyClass = new BasicEnemyClass(startingDamage, startingResistance, startingHealth, startingMovespeed, goldDrop);
    }

    public void TakeDamage(GameObject player, string attackType)
    {
        Debug.Log("Calculating damage");

        if (attackType == "Heavy")
        {
            basicEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentHeavyDamage);
            basicEnemyAI.Knockback();
            
        }
        else if (attackType == "Light")
        {
            basicEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentLightDamage);
            basicEnemyAI.Knockback();
        }

        // If enemy health drops below zero
        if (basicEnemyClass.currentHealth <= 0)
        {
            // Death animation here.

            // Invoke death for animation duration or call it when animation finishes
            pathfindingComponent.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            BeginEnemyDeath(player);
        }
    }

    public void EnemyDead()
    {
        rewardPlayer.GetComponent<PlayerStats>().playerClass.GainGold(basicEnemyClass.currentGoldDrop);
        waveManager.RemoveActiveEnemy(this.gameObject);
        Destroy(gameObject);
    }

    public void BeginEnemyDeath(GameObject rewardPlayerInput)
    {
        rewardPlayer = rewardPlayerInput;
        animationComponent.SetAnimBool("Walking", false);
        animationComponent.SetAnimBool("Dying", true);
    }
}
