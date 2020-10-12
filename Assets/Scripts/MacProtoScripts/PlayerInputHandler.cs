using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInputMap controls;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerCount playerCount;
    private PlayerStats playerStats;
    private SceneHandler sceneHandler;

    private float cooldownTime = 0.4f;
    private float nextAttackTime = 0f;
    private bool canAttack = true;

    public bool attacking;

    void Awake()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();
        playerStats = FindObjectOfType<PlayerStats>();
        playerCount = FindObjectOfType<PlayerCount>();
        playerInput = GetComponent<PlayerInput>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        controls = new PlayerInputMap();

        playerCount.AddPlayer(gameObject);

        // Creates an array of PlayerMovement scripts for each player
        var playerControllers = FindObjectsOfType<PlayerMovement>();
        var playerCombats = FindObjectsOfType<PlayerCombat>();

        // Creates variable that holds the index of the player
        var index = playerInput.playerIndex;

        // Takes the first controller to give input and assigns it to index 0, continues till 4 players have joined with the final index "3"
        playerMovement = playerControllers.FirstOrDefault(m => m.GetPlayerIndex() == index);
        playerCombat = playerCombats.FirstOrDefault(m => m.GetPlayerIndex() == index);
        Debug.Log(index);
    }

    void AddToPlayerList()
    {
        playerCount.AddPlayer(gameObject);
    }

    public void OnMove(CallbackContext movementContext)
    {
        if(playerMovement != null)
        {
            // Takes the input from object this script is attached to (PlayerInputController)
            playerMovement.SetInputVector(movementContext.ReadValue<Vector2>());
        }
    }

    // What happens for each input press

    // These functions are called by the input system with context to what has happened with the input
    public void Interact(CallbackContext context)
    {
        // Checks if the input has been pressed
        if (context.performed && playerMovement != null)
        {
            // If true then call Interact function
            playerCombat.Interact();
        }
    }

    void FixedUpdate()
    {
        if(Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + cooldownTime;
            canAttack = true;
        }
    }

    public void LightAttack(CallbackContext context)
    {
        if (context.performed && playerMovement != null && canAttack)
        {
            playerCombat.LightAttack();
            canAttack = false;
        }
    }

    public void HeavyAttack(CallbackContext context)
    {
        if (context.performed && playerMovement != null)
        {
            playerCombat.HeavyAttack();
        }
    }

    public void ActiveItem(CallbackContext context)
    {
        if (context.performed && playerMovement != null)
        {
            playerCombat.ActiveItem();
        }
    }

    public void NextScene(CallbackContext context)
    {
        if (context.performed && playerMovement != null)
        {
            sceneHandler.ChangeScene();
        }
    }
}
