using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GlobalStalkerState : State<Stalker>
{
    private float chaseDelayTimer = 0f;
    private bool sawPlayer = false;

    public void Enter(Stalker stalker)
    {

    }

    public void Update(Stalker stalker)
    {
        // Delaying chasing
        if (sawPlayer)
        {
            if (stalker.stateMachine.GetCurrentState() == stalker.recoveringState)
            {
                sawPlayer = false;
                chaseDelayTimer = 0;
                return;
            }

            chaseDelayTimer += Time.deltaTime;

            if (chaseDelayTimer >= stalker.chaseDelay)
            {
                chaseDelayTimer = 0.0f;
                sawPlayer = false;
                stalker.stateMachine.ChangeState(stalker.chaseState);

            }
        }

        // mora da bude ispod delaying chase
        if (!stalker.canChasePlayer)
            return;




        Vector3 directionToPlayer = stalker.playerSpotPoint.position - stalker.eyes.position;

        // Visibility 
        if (directionToPlayer.magnitude <= stalker.viewDistance)
        {
            float angle = Vector3.Angle(stalker.eyes.transform.forward, directionToPlayer.normalized);

            if (angle <= stalker.viewAngle / 2)
                if (!Physics.Raycast(stalker.eyes.transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, stalker.obstacleMask))
                    sawPlayer = true;
        }

        // Allowing to show line from stalker to player
        if (stalker.stateMachine.GetCurrentState() == stalker.chaseState)
            stalker.canSeePlayer = true;
        else
            stalker.canSeePlayer = false;

        // Subtile Noice Detectoin
        if (Vector3.Distance(stalker.transform.position, stalker.player.position) <= stalker.subtleNoiceDetecitonRange
            && stalker.playerStates.currentState == PlayerStates.States.WALKING
            && stalker.stateMachine.GetCurrentState() != stalker.relocatingState
            && stalker.stateMachine.GetCurrentState() != stalker.chaseState
            && stalker.stateMachine.GetCurrentState() != stalker.alertInvestigatingState)
        {
            stalker.noice.position = stalker.player.position;
            stalker.stateMachine.ChangeState(stalker.investigatingState);
        }

        // Loud Noice Detection
        if (Vector3.Distance(stalker.transform.position, stalker.player.position) <= stalker.loudNoiceDetectionRange
            && stalker.playerStates.currentState == PlayerStates.States.SPRINTING
            && stalker.stateMachine.GetCurrentState() != stalker.relocatingState
            && stalker.stateMachine.GetCurrentState() != stalker.chaseState
             && stalker.stateMachine.GetCurrentState() != stalker.investigatingState) // ovde dodaj i bacanje vlase i borbu
        {
            stalker.noice.position = stalker.player.position; // prepravi ovo da se koriste zvukovi i za flasu i fight
            stalker.stateMachine.ChangeState(stalker.alertInvestigatingState);
        }

    }

    public void Exit(Stalker entity)
    {
        chaseDelayTimer = 0.0f;
    }
}
