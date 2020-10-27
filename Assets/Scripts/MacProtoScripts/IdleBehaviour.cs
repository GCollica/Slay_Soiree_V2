﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerCombat.instance.inputRecieved)
        {
            animator.SetTrigger("AttackOne");
            PlayerCombat.instance.InputManager();
            PlayerCombat.instance.inputRecieved = false;
        }
    }
}
