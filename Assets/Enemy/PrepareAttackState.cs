using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareAttackState : StateMachineBehaviour
{
    [SerializeField] private List<string> _triggers;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       ResetTriggers(animator);
       animator.transform.TryGetComponent<EnemyAI>(out var ai);
       ai.IsAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       ResetTriggers(animator);
       animator.transform.TryGetComponent<EnemyAI>(out var ai);
       ai.IsAttacking = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void ResetTriggers(Animator animator) {
        foreach (string trigger in _triggers)
        {
            animator.ResetTrigger(trigger);
        }
    }
}
