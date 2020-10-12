using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCombat playerCombat;

    private Vector2 left, right;

    private Animator animator;

    [SerializeField]
    private int playerIndex = 0;

    //Starting speed for player
    [SerializeField]
    private float baseSpeed = 3;

    //Speed stat that changes with items
    public float playerSpeed;

    //Unity auto-generated input script
    private PlayerInputMap controls;

    //For calculating movement for character
    Vector2 move;
    private Vector2 targetVelocity;
    //Acceleration rat
    public float forceMult;

    private Vector2 m;

    private Rigidbody2D rb;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerInputMap();

        left = new Vector2(1f, 1f);
        right = new Vector2(-1f, 1f);
    }

    public void SetInputVector(Vector2 direction)
    {
        //Sets the Vector2 value taken from the PlayerInputHandler script
        move = direction;
    }

    void Update()
    {
        TurnPlayer();

        animator.SetFloat("Speed", m.sqrMagnitude);
    }

    void TurnPlayer()
    {
        // Player moves left, flip character left
        if (move.x <= -0.1)
        {
            gameObject.transform.localScale = left;
        }

        // Player moves right, flip character right
        if (move.x >= 0.1)
        {
            gameObject.transform.localScale = right;
        }
    }

    void FixedUpdate()
    {
        //Assigns "m" to the Vector2 value of the left joystick axes
        m = new Vector2(move.x, move.y);

        //Sets the velocity to accelerate to
        targetVelocity = m * ((baseSpeed + playerSpeed) * 100) * Time.fixedDeltaTime;

        //Calculates the amount of force delivered each frame
        Vector2 force = (targetVelocity - rb.velocity) * forceMult;

        //Moves player forwards
        rb.AddForce(force);

        //Move(m);
    }

    public int GetPlayerIndex()
    {
        //Returns the index of the player Index 0-3 (Player 1-4) 
        return playerIndex;
    }

    void Move(Vector2 direction)
    {
        Debug.Log("Moving!" + direction);
    }
}
