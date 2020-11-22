using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTransitionTwoBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat playerCombat = animator.GetComponentInParent<PlayerCombat>();
        playerCombat.canRecieveInput = true;
        playerCombat.canKnockback = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        PlayerCombat playerCombat = animator.GetComponentInParent<PlayerCombat>();
        if (playerCombat.inputRecieved)
        {            
            animator.SetTrigger("AttackThree");
            playerCombat.MeleeAttack();
            playerCombat.InputManager();
            playerCombat.inputRecieved = false;
        }
    }
}
