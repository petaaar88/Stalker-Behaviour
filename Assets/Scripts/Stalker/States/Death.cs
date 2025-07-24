using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State<Stalker>
{
    public void Enter(Stalker stalker)
    {
        stalker.agentMovement.speed = 0.0f;
        stalker.GetComponent<CapsuleCollider>().enabled = false;
        stalker.animator.SetTrigger("IsDead");
    }

    public void Update(Stalker stalker)
    {

    }

    public void Exit(Stalker stalker)
    {

    }
}
