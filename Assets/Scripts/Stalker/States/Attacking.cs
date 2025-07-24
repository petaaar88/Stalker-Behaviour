using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : State<Stalker>
{
    public void Enter(Stalker stalker)
    {
        stalker.animator.SetTrigger("Attack");
        stalker.animator.SetBool("isAttackMirrored", Random.Range(0, 2) == 0);
        stalker.currentStalkerState = "Attacking";
    }

    public void Update(Stalker stalker)
    {

    }

    public void Exit(Stalker stalker)
    {
        stalker.previousStalkerState = "Attacking";
        stalker.canAttack = false;
        MessageBroker.Instance.canChooseStalkerForAttacking = true;

    }
}
