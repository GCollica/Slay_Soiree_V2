using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public enum MeleeState { Idle, Moving, Dodging, Attack, Attack1_Transition, Attack2_Transition };
    public MeleeState stateMelee;

    public enum RangedState { RestrictMovement, NormalMovement };
    public RangedState stateRanged;

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
        #region Melee States
        switch (stateMelee)
        {
            case MeleeState.Idle:
                playerMovement.restrictMovement = false;
                playerCombat.canKnockback = false;

                if (playerCombat.inputRecieved)
                {
                    //Debug.Log("Attack Animation");
                    animator.SetTrigger("AttackOne");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                    stateMelee = MeleeState.Attack;
                }
                break;

            case MeleeState.Moving:
                if (playerCombat.inputRecieved)
                {
                    Debug.Log("Attack Animation");
                    animator.SetTrigger("AttackOne");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                }
                break;

            case MeleeState.Dodging:               
                break;

            case MeleeState.Attack:
                PlayerMovement.instance.restrictMovement = true;
                break;

            case MeleeState.Attack1_Transition:
                playerCombat.canRecieveInput = true;

                if (playerCombat.inputRecieved)
                {
                    animator.SetTrigger("AttackTwo");
                    playerCombat.MeleeAttack();
                    playerCombat.InputManager();
                    playerCombat.inputRecieved = false;
                }
                break;

            case MeleeState.Attack2_Transition:
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
        #endregion

        #region
        switch (stateRanged)
        {
            case RangedState.NormalMovement:
                playerMovement.restrictMovement = false;
                break;
            case RangedState.RestrictMovement:
                playerMovement.restrictMovement = true;
                break;
        }
        #endregion
    }

    #region Melee Events
    public void SetIdle()
    {
        stateMelee = MeleeState.Idle;
    }

    public void SetMoving()
    {
        stateMelee = MeleeState.Moving;
    }

    public void SetDodging()
    {
        stateMelee = MeleeState.Dodging;
        playerMovement.Dodge();
    }

    public void SetAttack()
    {
        stateMelee = MeleeState.Attack;
    }

    public void SetAttack1_Transition()
    {
        stateMelee = MeleeState.Attack1_Transition;
    }

    public void SetAttack2_Transition()
    {
        stateMelee = MeleeState.Attack2_Transition;
    }
    #endregion

    #region Ranged Events
    public void SetNormal()
    {
        stateRanged = RangedState.NormalMovement;
    }

    public void SetRestrict()
    {
        stateRanged = RangedState.RestrictMovement;
    }
    #endregion
}
