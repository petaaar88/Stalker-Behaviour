using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlertInvestigating : State<Stalker>
{
    private bool isArrivedAtNoiseOriginPosition = false; // TODO: rename
    private Vector3 noiseOriginPosition;

    public void Enter(Stalker stalker)
    {
        stalker.currentStalkerState = "AlertInvestigating";
        stalker.agentMovement.speed = stalker.relocatingSpeed;
        stalker.agentMovement.SetTarget(stalker.noice);
        if (stalker.stateMachine.GetPreviousState() == stalker.stateMachine.lookingAroundState)
            stalker.animator.SetTrigger("InvestigationEnd");
        else
            stalker.animator.SetTrigger("ExitCover");

        NoiseBroker.Instance.AddStalkerToInspectNoiseOrigin(stalker.noice.position, stalker);
        noiseOriginPosition = stalker.noice.position;
        isArrivedAtNoiseOriginPosition = false;

    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.agentMovement.pathSolver.grid.NodeFromWorldPoint(stalker.noice.position).worldPosition, stalker.transform.position) <= stalker.agentMovement.stoppingDistance)
        {
            stalker.previousLoudSubtlePosition = NoiceListener.Instance.subtleNoicePosition;
            isArrivedAtNoiseOriginPosition = true;
            stalker.stateMachine.ChangeState(stalker.stateMachine.lookingAroundState);
        }
    }

    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "AlertInvestigating";
        stalker.animator.ResetTrigger("InvestigationEnd");
       
        stalker.animator.ResetTrigger("ExitCover");

        if (!isArrivedAtNoiseOriginPosition)
            NoiseBroker.Instance.RemoveStalkerFromInspectingNoiseOrigin(noiseOriginPosition, stalker);

    }
}
