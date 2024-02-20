using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBlendTree : StateMachineBehaviour
{
    Player player;

    private void OnEnable()
    {
        player = GameManager.Instance.Player;
    }

    // OnStateExit는 트랜지션이 끝날 때나 이 상태머신이 종료될 때 실행
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.RestoreSpeed();
    }
}
