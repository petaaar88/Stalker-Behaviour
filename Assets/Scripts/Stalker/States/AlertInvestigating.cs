using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlertInvestigating : State<Stalker>
{
    public void Enter(Stalker stalker)
    {
        stalker.currentStalkerState = "AlertInvestigating";
        stalker.agentMovement.speed = stalker.relocatingSpeed;
        stalker.agentMovement.SetTarget(stalker.noice);
        if (stalker.stateMachine.GetPreviousState() == stalker.stateMachine.lookingAroundState)
            stalker.animator.SetTrigger("InvestigationEnd");
        else
            stalker.animator.SetTrigger("ExitCover");
    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.noice.position, stalker.transform.position) <= 2.0f)
            stalker.stateMachine.ChangeState(stalker.stateMachine.lookingAroundState);
    }

    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "AlertInvestigating";
    }
}
