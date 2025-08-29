using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigating : State<Stalker>
{
    private bool isArrivedAtNoiseOriginPosition = false; // TODO: rename
    private Vector3 noiseOriginPosition;

    public void Enter(Stalker stalker)
    {
        stalker.agentMovement.speed = stalker.investigatingSpeed;
        stalker.agentMovement.SetTarget(stalker.noice);
        stalker.animator.SetTrigger("HeardSubtleNoice");
        stalker.currentStalkerState = "Investigating";

        NoiseBroker.Instance.AddStalkerToInspectNoiseOrigin(stalker.noice.position, stalker);
        noiseOriginPosition = stalker.noice.position;
        isArrivedAtNoiseOriginPosition = false;
    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.noice.position, stalker.transform.position) <= stalker.agentMovement.stoppingDistance)
        {
            isArrivedAtNoiseOriginPosition = true;
            stalker.stateMachine.ChangeState(stalker.stateMachine.lookingAroundState);
        }
    }

    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "Investigating";
        stalker.animator.ResetTrigger("HeardSubtleNoice");

        if (!isArrivedAtNoiseOriginPosition)
            NoiseBroker.Instance.RemoveStalkerFromInspectingNoiseOrigin(noiseOriginPosition, stalker);
    }
}
