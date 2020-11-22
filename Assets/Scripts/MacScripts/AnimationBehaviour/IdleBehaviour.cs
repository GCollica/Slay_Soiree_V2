using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat playerCombat = animator.GetComponentInParent<PlayerCombat>();
        PlayerMovement.instance.restrictMovement = false;
        playerCombat.canKnockback = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat playerCombat = animator.GetComponentInParent<PlayerCombat>();
        if (playerCombat.inputRecieved)
        {
            //PlayerCombat playerCombat = animator.GetComponentInParent<PlayerCombat>();
            //Debug.Log("Attack Animation");
            animator.SetTrigger("AttackOne");
            playerCombat.MeleeAttack();
            playerCombat.InputManager();
            playerCombat.inputRecieved = false;
        }
    }
}
