using UnityEngine;

public class SetMovingFromBool : StateMachineBehaviour
{
    public bool state;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("MovingFrom", state);
    }
}
