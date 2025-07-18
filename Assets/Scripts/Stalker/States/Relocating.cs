using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relocating : State<Stalker>
{

    private float stoppingDistance = 2.0f;

    public void Enter(Stalker stalker)
    {
        stalker.agentMovement.SetTarget(stalker.coversPositions[stalker.currentCoverIndex]);
        stalker.agentMovement.speed = stalker.relocatingSpeed;
        stalker.currentStalkerState = "Relocating";
    }
    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.coversPositions[stalker.currentCoverIndex].position, stalker.transform.position) <= stoppingDistance)
            stalker.stateMachine.ChangeState(stalker.inCoverState);
    }
    public void Exit(Stalker entity)
    {

    }
}
