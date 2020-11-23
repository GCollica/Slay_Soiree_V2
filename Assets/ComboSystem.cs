using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public enum State {Idle, Moving, Dodging, Attack, Attack1_Transition, Attack2_Transition};
    public State state;

    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                playerMovement.restrictMovement = false;
                playerCombat.canKnockback = false;

                if (playerCombat.inputRecieved)
                {
                    //Debug.Log("Attack Animation");
                    animator.SetTrigger("AttackOne");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                    state = State.Attack;
                }
                break;

            case State.Moving:
                if (playerCombat.inputRecieved)
                {
                    Debug.Log("Attack Animation");
                    animator.SetTrigger("AttackOne");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                }
                break;

            case State.Dodging:               
                break;

            case State.Attack:
                PlayerMovement.instance.restrictMovement = true;
                break;

            case State.Attack1_Transition:
                playerCombat.canRecieveInput = true;

                if (playerCombat.inputRecieved)
                {
                    animator.SetTrigger("AttackTwo");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                }
                break;

            case State.Attack2_Transition:
                playerCombat.canRecieveInput = true;
                playerCombat.canKnockback = true;

                if (playerCombat.inputRecieved)
                {
                    animator.SetTrigger("AttackThree");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                }
                break;
        }
    }

    public void SetIdle()
    {
        state = State.Idle;
    }

    public void SetMoving()
    {
        state = State.Moving;
    }

    public void SetDodging()
    {
        state = State.Dodging;
        playerMovement.Dodge();
    }

    public void SetAttack()
    {
        state = State.Attack;
    }

    public void SetAttack1_Transition()
    {
        state = State.Attack1_Transition;
    }

    public void SetAttack2_Transition()
    {
        state = State.Attack2_Transition;
    }

}
