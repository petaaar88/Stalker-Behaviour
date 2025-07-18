using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Stalker>
{
    private float chaseTimer = 0f;
    private bool isTimerStarted = false;

    public void Enter(Stalker stalker)
    {
        stalker.agentMovement.SetTarget(stalker.player);
        stalker.agentMovement.speed = stalker.chaseSpeed;
        stalker.animator.SetTrigger("StartChase");
        stalker.currentStalkerState = "Chase";

        chaseTimer = 0f;
        isTimerStarted = true;
    }

    public void Update(Stalker stalker)
    {
        if (!isTimerStarted) return;

        chaseTimer += Time.deltaTime;

        if (chaseTimer >= stalker.chaseTime)
        {
            isTimerStarted = false;
           
            stalker.stateMachine.ChangeState(stalker.recoveringState);
        }
    }

    public void Exit(Stalker stalker)
    {
        isTimerStarted = false;
        chaseTimer = 0f;
    }
}
