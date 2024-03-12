using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndSkill : StateMachineBehaviour
{
    Player player;
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }
        //Debug.Log("Enter");
        player.ShowWeaponEffect(true);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        player.ShowWeaponEffect(false);
        animator.ResetTrigger(Attack_Hash);
        //Debug.Log("Exit");
    }
}
