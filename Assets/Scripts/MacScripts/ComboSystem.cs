﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public enum MeleeState { Idle, Moving, Dodging, Attack, Attack1_Transition, Attack2_Transition, Ranged };
    public MeleeState stateMelee;

    public enum RangedState { RestrictMovement, NormalMovement, Melee };
    public RangedState stateRanged;

    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    private Animator animator;

    private SoundManager soundManager;

    private void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        animator = GetComponent<Animator>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        #region Melee States
        switch (stateMelee)
        {
            case MeleeState.Idle:
                if (playerCombat.ranged)
                {
                    stateMelee = MeleeState.Ranged;
                }
            
                playerMovement.restrictMovement = false;
                playerCombat.canKnockback = false;
                stateRanged = RangedState.Melee;

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
                stateRanged = RangedState.Melee;
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
                stateRanged = RangedState.Melee;
                break;

            case MeleeState.Attack:
                stateRanged = RangedState.Melee;
                playerMovement.restrictMovement = true;
                break;

            case MeleeState.Attack1_Transition:
                stateRanged = RangedState.Melee;
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
                stateRanged = RangedState.Melee;
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
            case MeleeState.Ranged:
                break;
        }
        #endregion

        #region Ranged States
        switch (stateRanged)
        {
            case RangedState.NormalMovement:
                if (!playerCombat.ranged)
                {
                    stateRanged = RangedState.Melee;
                }

                stateMelee = MeleeState.Ranged;
                playerMovement.restrictMovement = false;
                break;
            case RangedState.RestrictMovement:
                stateMelee = MeleeState.Ranged;
                playerMovement.restrictMovement = true;
                break;
            case RangedState.Melee:
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

    #region Audio
    public void AttackWiff1()
    {
        soundManager.Play("Sword Swing 1");
    }

    public void AttackWiff2()
    {
        soundManager.Play("Sword Swing 2");
    }

    public void AttackWiff3()
    {
        soundManager.Play("Sword Swing 3");
    }

    #endregion
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