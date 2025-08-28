using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : State<Stalker>
{
    private float speedBeforeEnteringLookAroundState = 0.0f;

    public void Enter(Stalker stalker)
    {
        stalker.currentStalkerState = "LookingAround";
        stalker.animator.SetTrigger("ArrievedAtNoicePosition");
        speedBeforeEnteringLookAroundState = stalker.agentMovement.speed;
        stalker.agentMovement.speed = 0.0f;
    }
    public void Update(Stalker stalker)
    {
      
    }
    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "LookingAround";
        stalker.animator.ResetTrigger("ArrievedAtNoicePosition");
        stalker.agentMovement.speed = speedBeforeEnteringLookAroundState;

    }


}
