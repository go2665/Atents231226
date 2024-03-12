using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNotShow : StateMachineBehaviour
{
    Player player = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }
        player.ShowWeaponAndShield(false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.ShowWeaponAndShield(true);
    }
}
