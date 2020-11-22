using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackOne : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement playerMovement = animator.GetComponentInParent<PlayerMovement>();
        PlayerMovement.instance.restrictMovement = true;
    }
}
