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

    private QuirkManager quirkManager;
    private WaveManager waveManager;
    private SoundManager soundManager;

    public SpriteRenderer spriteRenderer;
    public GameObject ParticleSystem;

    private float startingDamage = 5f;
    private float startingResistance = 10f;
    private float startingHealth = 25f;
    private float startingMovespeed = 400f;
    private int goldDrop = 2;

    private GameObject rewardPlayer;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        skeletonEnemyAI = this.gameObject.GetComponent<SkeletonEnemy_AI>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Animation>();
        pathfindingComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Pathfinding>();

        if (this.gameObject.transform.GetChild(1) != null)
        {
            spriteRenderer = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        InitialiseClassInstance();

        waveManager = FindObjectOfType<WaveManager>();
        waveManager.AddActiveEnemy(this.gameObject);
        quirkManager = FindObjectOfType<QuirkManager>();

        if (quirkManager.CurrentQuirk.quirkID == 2)
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
        basicEnemyClass = new BasicEnemy_Class(startingDamage, startingResistance, startingHealth, startingMovespeed, goldDrop);
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
        Destroy(gameObject);
    }

    public void BeginEnemyDeath(GameObject rewardPlayerInput)
    {
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
        Vector3 facingVector = (this.gameObject.transform.position - player.transform.position).normalized;
        particleGO.transform.forward = facingVector;
    }
}
