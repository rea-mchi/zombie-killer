using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{

    [SerializeField] private List<string> _resetTriggers;

    private GunController _gunController = null; // TODO: late init?

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        ResetTriggers(animator);
        if (_gunController == null)
        {
            animator.transform.TryGetComponent<GunController>(out _gunController);
        }
        _gunController.CanInput = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        ResetTriggers(animator);
        _gunController.CanInput = false;
    }

    private void ResetTriggers(Animator animator) {
        foreach (string trigger in _resetTriggers) {
            animator.ResetTrigger(trigger);
        }
    }


}
