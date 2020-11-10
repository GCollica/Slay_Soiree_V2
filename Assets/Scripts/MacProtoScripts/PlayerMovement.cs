using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCombat playerCombat;

    private Vector2 left, right;

    private Animator animator;

    [HideInInspector]
    public bool restrictMovement;

    public bool isMoving;

    [SerializeField]
    private GameObject playerSprite;

    [SerializeField]
    private int playerIndex = 0;

    [Space]
    [Header("Player Speed Stats")]
    //Starting speed for player
    [SerializeField]
    private float baseSpeed = 3;
    //Speed stat that changes with items
    public float playerSpeed;
	public float startSpeed;
	public float attackMoveSpeed;

    //Unity auto-generated input script
    private PlayerInputMap controls;

    //For calculating movement for character
    Vector2 move;
    private Vector2 targetVelocity;
    //Acceleration rate
    public float forceMult;

    private Vector2 m;

    private Rigidbody2D rb;

    public static PlayerMovement instance;

    private enum State { Normal, Rolling}
    private Vector2 rollDir;
    private State state;
    private float rollSpeed;

    void Awake()
    {
        instance = this;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerInputMap();

        left = new Vector2(1f, 1f);
        right = new Vector2(-1f, 1f);
    }

	void Start()
	{
		startSpeed = baseSpeed;
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

        if (state == State.Rolling)
        {
            float rollSpeedDecelerator = 5f;
            rollSpeed -= rollSpeed * rollSpeedDecelerator * Time.deltaTime;

            float rollSpeedMin = 10f;
            if(rollSpeed < rollSpeedMin)
            {
                state = State.Normal;
            }
        }
    }

    void TurnPlayer()
    {
        // Player moves left, flip character left
        if (move.x <= -0.1 && !restrictMovement)
        {
            playerSprite.transform.localScale = left;
        }

        // Player moves right, flip character right
        if (move.x >= 0.1 && !restrictMovement)
        {
            playerSprite.transform.localScale = right;
        }
    }

    // ToDo: Disable movement when aiming 

    void FixedUpdate()
    {
        switch (state) {
            case State.Normal:
                if (!restrictMovement)
                {
					//Assigns "m" to the Vector2 value of the left joystick axes
					// m = new Vector2(move.x, move.y);
					baseSpeed = startSpeed;	
                }
                else
                {
					//m = new Vector2(0, 0);
					baseSpeed = attackMoveSpeed;
				}

				m = new Vector2(move.x, move.y);
				//Sets the velocity to accelerate to
				targetVelocity = m * ((baseSpeed + playerSpeed) * 100) * Time.fixedDeltaTime;

                //Calculates the amount of force delivered each frame
                Vector2 force = (targetVelocity - rb.velocity) * forceMult;

                //Moves player forwards
                rb.AddForce(force);
                break;
            case State.Rolling:
                rb.velocity = rollDir * rollSpeed;
                m = new Vector2(move.x, move.y);
                break;
        }		
    }

    public int GetPlayerIndex()
    {
        //Returns the index of the player Index 0-3 (Player 1-4) 
        return playerIndex;
    }

    public void Dodge()
    {
        Debug.Log("Dodging");

        rollDir = m;
        rollSpeed = 25f;
        state = State.Rolling;
    }
}
