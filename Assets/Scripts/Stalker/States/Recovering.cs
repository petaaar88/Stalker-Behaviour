
using UnityEngine;

public class Recovering : State<Stalker>
{
    private float stoppingDistance = 2.0f;

    public void Enter(Stalker stalker)
    {
        stalker.canChasePlayer = false;
        stalker.currentStalkerState = "Recovering";
        stalker.agentMovement.SetTarget(stalker.coversPositions[stalker.currentCoverIndex]);
        stalker.agentMovement.speed = stalker.relocatingSpeed;
        stalker.animator.SetTrigger("ChaseEnd");

        stalker.animator.applyRootMotion = false;
    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.coversPositions[stalker.currentCoverIndex].position, stalker.transform.position) <= stoppingDistance)
            stalker.stateMachine.ChangeState(stalker.stateMachine.inCoverState);
    }

    public void Exit(Stalker stalker)
    {
        stalker.animator.applyRootMotion = true;

        stalker.canChasePlayer = true;
        stalker.previousStalkerState = "Recovering";
        stalker.previousLoudSubtlePosition = NoiceListener.Instance.subtleNoicePosition;
        stalker.previousLoudNoicePosition = NoiceListener.Instance.loudNoicePosition;
    }
}