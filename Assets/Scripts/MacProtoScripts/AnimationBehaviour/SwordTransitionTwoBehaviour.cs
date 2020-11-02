using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTransitionTwoBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat.instance.canRecieveInput = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (PlayerCombat.instance.inputRecieved)
        {
            animator.SetTrigger("AttackThree");
            PlayerCombat.instance.InputManager();
            PlayerCombat.instance.inputRecieved = false;
        }
    }
}
