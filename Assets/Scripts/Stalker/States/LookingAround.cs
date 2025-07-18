using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : State<Stalker>
{
    public void Enter(Stalker stalker)
    {
        stalker.animator.SetTrigger("ArrievedAtNoicePosition");
    }
    public void Update(Stalker stalker)
    {
      
    }
    public void Exit(Stalker stalker)
    {
       
    }

   
}
