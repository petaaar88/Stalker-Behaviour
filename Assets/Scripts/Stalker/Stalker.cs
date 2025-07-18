using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    [HideInInspector]
    public AgentMovement agentMovement;
    [HideInInspector]
    public Animator animator;
    public string currentStalkerState;

    public StateMachine<Stalker> stateMachine;
    public Transform player;

    [Header("Cover Settings")]
    public List<Transform> coversPositions;
    public int currentCoverIndex = 0;
    public bool showCovers;
    public float secondsInCover;
    public float relocatingSpeed = 8.5f;

    [Header("Visibility")]
    public float viewAngle = 0.0f;
    public float viewDistance = 0.0f;
    public LayerMask obstacleMask;
    public bool showVisibility;
    [HideInInspector]
    public bool canSeePlayer = false;

    [Header("Chase")]
    public float chaseTime;
    [HideInInspector]
    public bool canChasePlayer = true;
    public float chaseDelay = 1.0f;
    public float chaseSpeed = 5.35f;

    [Header("Sound Detection")]
    public float loudSoundDetectionRange;
    public float subtlesoundDetecitonRange;
    public bool showSoundDetectionRange;

    // States
    public Relocating relocatingState;
    public InCover inCoverState;
    public Chase chaseState;
    public Recovering recoveringState;
    public Investigating investigatingState;
    public GlobalStalkerState globalState;

    void Start()
    {
        animator = GetComponent<Animator>();
        agentMovement = GetComponent<AgentMovement>();

        relocatingState = new Relocating();
        inCoverState = new InCover();
        chaseState = new Chase();
        recoveringState = new Recovering();
        investigatingState = new Investigating();
        globalState = new GlobalStalkerState();

        stateMachine = new StateMachine<Stalker>(this);
        stateMachine.SetCurrentState(relocatingState);
        relocatingState.Enter(this);

        stateMachine.SetGlobalState(globalState);
    }


    void Update()
    {
        stateMachine.Update();
    }

    private void OnDrawGizmos()
    {
        if (showCovers)
        {
            Gizmos.color = Color.cyan;

            foreach (Transform coverPosition in coversPositions)
                Gizmos.DrawWireSphere(coverPosition.position, 2);
        }

        Vector3 eyePosition = transform.position;
        eyePosition.y += 1.4f;

        if (showSoundDetectionRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(eyePosition, loudSoundDetectionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(eyePosition, subtlesoundDetecitonRange);

        }

        if (!showVisibility)
            return;

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(eyePosition, (player.position - transform.position).normalized * 5);
        }


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePosition, viewDistance);

        // Desni krajni ugao (halfAngle desno)
        Vector3 rightDirection = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        // Levi krajni ugao (halfAngle levo)
        Vector3 leftDirection = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePosition, eyePosition + rightDirection * viewDistance);
        Gizmos.DrawLine(eyePosition, eyePosition + leftDirection * viewDistance);

    }
}
