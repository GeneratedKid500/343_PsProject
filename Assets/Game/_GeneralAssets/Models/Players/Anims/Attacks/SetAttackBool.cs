using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackBool : StateMachineBehaviour
{
    public string attackID;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("MovingTo", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(attackID, false);
    }
}
