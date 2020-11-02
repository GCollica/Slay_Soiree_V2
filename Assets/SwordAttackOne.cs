using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackOne : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement.instance.restrictMovement = true;
    }
}
