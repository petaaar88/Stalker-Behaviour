using System.Collections;
using System.Collections.Generic;
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
            if(stalker.stateMachine.GetCurrentState() == stalker.recoveringState)
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

        

        Vector3 directionToPlayer = stalker.player.position - stalker.transform.position;

        if (directionToPlayer.magnitude <= stalker.viewDistance)
        {
            float angle = Vector3.Angle(stalker.transform.forward, directionToPlayer.normalized);

            if (angle <= stalker.viewAngle / 2)
            {
                if (Physics.Raycast(stalker.transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, stalker.obstacleMask))
                {
                    stalker.canSeePlayer = false;
                }
                else
                {
                    stalker.canSeePlayer = true;
                    sawPlayer = true;
                }
            }
            else
                stalker.canSeePlayer = false;
        }
        else
            stalker.canSeePlayer = false;
    }

    public void Exit(Stalker entity)
    {
        chaseDelayTimer = 0.0f;
    }
}
