using UnityEngine;

public class CrowEnemy : MonoBehaviour
{
    /*Script that will be attached to each crow enemy gameobject throughout the game. Holds individual values for damage, resistance, health & movement speed and feeds that into it's own instance of the CrowEnemyClass. */
    public BasicEnemyClass basicEnemyClass;
    private CrowEnemy_AI crowEnemyAI;
    private QuirkManager quirkManager;
    private WaveManager waveManager;
    private SkeletonEnemy_Animation animationComponent;
    private CrowEnemyAIPathfinding pathfindingComponent;
    private SoundManager soundManager;

    public SpriteRenderer spriteRenderer;

    public float startingDamage = 5f;
    public float startingResistance = 10f;
    public float startingHealth = 25f;
    public float startingMovespeed = 10f;
    public int goldDrop = 2;

    private GameObject rewardPlayer;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        crowEnemyAI = this.gameObject.GetComponent<CrowEnemy_AI>();
        animationComponent = this.gameObject.transform.GetComponentInChildren<SkeletonEnemy_Animation>();
        pathfindingComponent = this.gameObject.transform.GetComponentInChildren<CrowEnemyAIPathfinding>();
        
        if (this.gameObject.transform.GetChild(0) != null)
        {
            spriteRenderer = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
        basicEnemyClass = new BasicEnemyClass(startingDamage, startingResistance, startingHealth, startingMovespeed, goldDrop);
    }

    /*private void SetSortingLayers()
    {
        spriteRenderer.sortingLayerName = Mathf.RoundToInt(this.gameObject.transform.position.y).ToString();
        spriteRenderer.sortingOrder = Mathf.RoundToInt(this.gameObject.transform.position.x) * 10;
    }*/

    public void TakeDamage(GameObject player, string attackType)
    {

        if (attackType == "Heavy")
        {
            basicEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentHeavyDamage);
            //basicEnemyAI.Knockback();

        }
        else if (attackType == "Light")
        {
            basicEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentLightDamage);
            //basicEnemyAI.Knockback();
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
        soundManager.Play("Bird Death");
        EnemyDead();
    }

}
