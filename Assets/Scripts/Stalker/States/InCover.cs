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
        stalker.agentMovement.speed = 0.0f;

        stalker.animator.SetBool("isCrouchIdleMirror", stalker.coversPositions[stalker.currentCoverIndex].mirrored);

        stalker.gameObject.transform.position = stalker.coversPositions[stalker.currentCoverIndex].position; // TODO: namesti samo za x i z osu

        Vector3 currentRotation = stalker.gameObject.transform.eulerAngles;
        currentRotation.y = stalker.coversPositions[stalker.currentCoverIndex].rotation;    
        stalker.gameObject.transform.eulerAngles = currentRotation;

        stalker.agentMovement.Disable();
    }

    public void Update(Stalker stalker)
    {
        if (!isTimerStarted) return;

        coverTimer += Time.deltaTime;

        if (coverTimer >= stalker.secondsInCover)
        {
            isTimerStarted = false;

            stalker.currentCoverIndex = (stalker.currentCoverIndex + 1) % stalker.coversPositions.Count;

            stalker.stateMachine.ChangeState(stalker.stateMachine.relocatingState);
        }
    }

    public void Exit(Stalker stalker)
    {
        isTimerStarted = false;
        coverTimer = 0f;
        stalker.previousStalkerState = "InCover";
        stalker.animator.SetTrigger("ExitCover");
        stalker.agentMovement.Enable();
    }
}
