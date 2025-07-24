using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relocating : State<Stalker>
{

    private float stoppingDistance =2.0f;
    

    public void Enter(Stalker stalker)
    {
        
        stalker.agentMovement.SetTarget(stalker.coversPositions[stalker.currentCoverIndex]);
        stalker.agentMovement.speed = stalker.relocatingSpeed;
        stalker.currentStalkerState = "Relocating";
        stalker.animator.applyRootMotion = false;

    }

    public void Update(Stalker stalker)
    {
        if (Vector3.Distance(stalker.coversPositions[stalker.currentCoverIndex].position, stalker.transform.position) <= stoppingDistance)
        {

           // stalker.transform.LookAt(stalker.coversPositions[stalker.currentCoverIndex].GetChild(0));
                
           // if(Vector3.Distance(stalker.transform.position,stalker.coversPositions[stalker.currentCoverIndex].GetComponentInChildren<Transform>().position) <= 2)
                stalker.stateMachine.ChangeState(stalker.stateMachine.inCoverState);

        }
    }
    public void Exit(Stalker stalker)
    {
        stalker.animator.applyRootMotion = true;

        stalker.previousStalkerState = "Relocating";
        stalker.previousLoudSubtlePosition = NoiceListener.Instance.subtleNoicePosition;
        stalker.previousLoudNoicePosition = NoiceListener.Instance.loudNoicePosition;
    }
}
