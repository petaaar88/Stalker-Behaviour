using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Stalker>
{
    

    public void Enter(Stalker stalker)
    {
        stalker.agentMovement.SetTarget(stalker.player);
        stalker.agentMovement.speed = stalker.chaseSpeed;
        if (stalker.stateMachine.GetPreviousState() == stalker.stateMachine.waitingToAttackState)
            stalker.animator.SetTrigger("PlayerMovedAway");
        else
            stalker.animator.SetTrigger("StartChase");

        stalker.currentStalkerState = "Chase";
        
    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.transform.position, stalker.player.transform.position ) <= stalker.agentMovement.stoppingDistance)
            stalker.stateMachine.ChangeState(stalker.stateMachine.waitingToAttackState);
    }

    public void Exit(Stalker stalker)
    {
        

        stalker.previousStalkerState = "Chase";
    }
}
