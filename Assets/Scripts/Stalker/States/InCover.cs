using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCover : State<Stalker>
{
    private float coverTimer = 0f;
    private bool isTimerStarted = false;

    public void Enter(Stalker stalker)
    {
        stalker.animator.SetTrigger("EnterCover");
        coverTimer = 0f;
        isTimerStarted = true;
        stalker.currentStalkerState = "InCover";
    }

    public void Update(Stalker stalker)
    {
        if (!isTimerStarted) return;

        coverTimer += Time.deltaTime;

        if (coverTimer >= stalker.secondsInCover)
        {
            isTimerStarted = false;
            stalker.animator.SetTrigger("ExitCover");

            stalker.currentCoverIndex = (stalker.currentCoverIndex + 1) % stalker.coversPositions.Count;

            stalker.stateMachine.ChangeState(stalker.relocatingState);
        }
    }

    public void Exit(Stalker stalker)
    {
        isTimerStarted = false;
        coverTimer = 0f;
    }
}
