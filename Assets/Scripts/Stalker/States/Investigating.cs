using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigating : State<Stalker>
{

    public void Enter(Stalker stalker)
    {
        stalker.agentMovement.speed = stalker.investigatingSpeed;
        stalker.agentMovement.SetTarget(stalker.noice);
        stalker.animator.SetTrigger("HeardSubtleNoice");
        stalker.currentStalkerState = "Investigating";

    }
    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.noice.position, stalker.transform.position) <= 2.0f)
            stalker.stateMachine.ChangeState(stalker.stateMachine.lookingAroundState);
    }
    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "Investigating";
    }
}
