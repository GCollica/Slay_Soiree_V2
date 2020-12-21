using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    /*Script that will be attached to each basic enemy 1 gameobject throughout the game. Holds individual values for damage, resistance, health & movement speed and feeds that into it's own instance of the BasicEnemyClass. */
    public BasicEnemy_Class basicEnemyClass;
    private SkeletonEnemy_AI skeletonEnemyAI;
    private SkeletonEnemy_Animation animationComponent;
    private SkeletonEnemy_Pathfinding pathfindingComponent;
    private SkeletonEnemy_Attacking attackingComponent;

    private QuirkManager quirkManager;
    private WaveManager waveManager;
    private SoundManager soundManager;
    private PlayerCount playerCount;

    public SpriteRenderer spriteRenderer;
    public GameObject ParticleSystem;

    private float startingDamage = 7.5f;
    private float startingResistance = 10f;
    private float startingHealth = 25f;
    private float startingMovespeed = 400f;
    private int goldDrop = 2;

    private GameObject rewardPlayer;
    public List<GameObject> attachedCrows;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        skeletonEnemyAI = this.gameObject.GetComponent<SkeletonEnemy_AI>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Animation>();
        pathfindingComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Pathfinding>();
        attackingComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Attacking>();

        if (this.gameObject.transform.GetChild(1) != null)
        {
            spriteRenderer = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        waveManager = FindObjectOfType<WaveManager>();
        waveManager.AddActiveEnemy(this.gameObject);
        quirkManager = FindObjectOfType<QuirkManager>();
        playerCount = FindObjectOfType<PlayerCount>();

        InitialiseClassInstance();

        if (quirkManager.CurrentQuirk.quirkID == 3)
        {
            int chosenModifier = quirkManager.RandomiseMSModifier();
            if (chosenModifier == 0)
            {
                basicEnemyClass.currentMovementSpeed = (basicEnemyClass.currentMovementSpeed / 2);
            }
            if (chosenModifier == 1)
            {
                basicEnemyClass.currentMovementSpeed = (basicEnemyClass.currentMovementSpeed * 2);
            }
        }
    }

    //Initialises an instance of the Basic Enemy Class, feeding it values for damage, resistance, health, movespeed as the constructor requires.
    private void InitialiseClassInstance()
    {
        switch (playerCount.activePlayers.Count)
        {
            case 1:
                basicEnemyClass = new BasicEnemy_Class(startingDamage, startingResistance, startingHealth, startingMovespeed, goldDrop);
                break;
            case 2:
                basicEnemyClass = new BasicEnemy_Class((startingDamage * 1.1f), startingResistance, (startingHealth * 1.5f), startingMovespeed, goldDrop);
                break;
            case 3:
                basicEnemyClass = new BasicEnemy_Class((startingDamage * 1.2f), startingResistance, (startingHealth * 2f), startingMovespeed, goldDrop);
                break;
            case 4:
                basicEnemyClass = new BasicEnemy_Class((startingDamage * 1.3f), startingResistance, (startingHealth * 2.5f), startingMovespeed, goldDrop);
                break;
        }
        
    }

    public void TakeDamage(GameObject player, string attackType)
    {
        //Debug.Log("Calculating damage");

        if (attackType == "Heavy")
        {
            SpawnParticles(player);
            PlayFlinchAnim();
            basicEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentHeavyDamage);
            //basicEnemyAI.Knockback();
        }
        else if (attackType == "Light")
        {
            SpawnParticles(player);
            PlayFlinchAnim();
            basicEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentLightDamage);
            soundManager.Play("Enemy Impact");
        }

        // If enemy health drops below zero
        if (basicEnemyClass.currentHealth <= 0)
        {
            // Death animation here.

            // Invoke death for animation duration or call it when animation finishes
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            BeginEnemyDeath(player);
        }
    }

    public void EnemyDead()
    {
        rewardPlayer.GetComponent<PlayerStats>().playerClass.GainGold(basicEnemyClass.currentGoldDrop);
        waveManager.RemoveActiveEnemy(this.gameObject);
        DetatchCrows();
        Destroy(gameObject);
    }

    public void BeginEnemyDeath(GameObject rewardPlayerInput)
    {
        attackingComponent.ToggleAttackCollider(false);
        rewardPlayer = rewardPlayerInput;
        animationComponent.SetAnimBool("Walking", false);
        animationComponent.SetAnimBool("Attacking", false);
        pathfindingComponent.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        animationComponent.SetAnimBool("Dying", true);
    }

    public void PlayFlinchAnim()
    {
        animationComponent.SetAnimBool("Walking", false);
        animationComponent.SetAnimBool("Flinching", true);
    }

    public void SpawnParticles(GameObject player)
    {
        GameObject particleGO = Instantiate(ParticleSystem, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.transform.position.z), Quaternion.identity, this.gameObject.transform);
        Vector3 facingVector = (new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.transform.position.z) - player.transform.position).normalized;
        particleGO.transform.forward = facingVector;
    }

    public void AddAttachedCrow(GameObject crow)
    {
        attachedCrows.Add(crow);
    }

    public void DetatchCrows()
    {
        foreach (var crow in attachedCrows)
        {
            if(crow == null)
            {
                continue;
            }

            crow.GetComponent<CrowEnemy_AI>().currentAIState = CrowEnemy_AI.AIState.Idle;
        }

        attachedCrows.Clear();
    }
}
