using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingToAttack : State<Stalker>
{
    public void Enter(Stalker stalker)
    {
        stalker.currentStalkerState = "WaitingToAttack";
        stalker.animator.SetTrigger("NearPlayer");
        stalker.agentMovement.speed = 0.0f;
    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.transform.position, stalker.player.transform.position) >= stalker.agentMovement.stoppingDistance + 2)
            stalker.stateMachine.ChangeState(stalker.stateMachine.chaseState);
    }

    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "WaitingToAttack";
    }
}
