
using UnityEngine;

public class EngagingPlayer : State<Stalker>
{
    private float engagementTimer = 0f;
    private bool isTimerStarted = false;

    public void Enter(Stalker stalker)
    {
        stalker.stateMachine.ChangeState(stalker.stateMachine.chaseState);
        engagementTimer = 0f;
        isTimerStarted = true;
        MessageBroker.Instance.AddStalkerToEngagement(stalker);
    }

    public void Update(Stalker stalker)
    {
        if (!isTimerStarted) return;

        engagementTimer += Time.deltaTime;

        if (engagementTimer >= stalker.engagementTime)
        {
            isTimerStarted = false;
            stalker.stateMachine.ChangeState(stalker.stateMachine.recoveringState);
            this.Exit(stalker);
        }
    }

    public void Exit(Stalker stalker)
    {
        isTimerStarted = false;
        engagementTimer = 0f;
        stalker.isEngagingToPlayer = false;
    }
}
