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
    public string previousStalkerState;

    public StalkerStateMachine stateMachine;
    [HideInInspector]
    public Transform player;
    public Transform eyes;
    public GameObject playerGameObject;
    public Transform playerSpotPoint;

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
    [HideInInspector]
    public bool canChasePlayer = true;
    public float chaseDelay = 1.0f;
    public float chaseSpeed = 5.35f;

    [Header("Noice Detection")]
    public float loudNoiceDetectionRange;
    public float subtleNoiceDetecitonRange;
    public bool showNoiceDetectionRange;
    [HideInInspector]
    public Vector3 previousLoudNoicePosition;
    [HideInInspector]
    public Vector3 previousLoudSubtlePosition;
    [HideInInspector]

    [Header("Investigating")]
    public float investigatingSpeed = 3.0f;
    [HideInInspector]
    public Transform noice;
    [HideInInspector]
    public Transform investigationNoise;

    [Header("Attacking")]
    public bool canAttack = false;

    //Engage To Player State
    public bool isEngagingToPlayer;

    public Health health;

    void Start()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        agentMovement = GetComponent<AgentMovement>();
        player = playerGameObject.GetComponent<Transform>();

        noice = new GameObject().transform;
        investigationNoise = new GameObject().transform;
        previousLoudSubtlePosition = Vector3.positiveInfinity;
        previousLoudNoicePosition = Vector3.positiveInfinity;

        animator.applyRootMotion = true;

        stateMachine = new StalkerStateMachine(this);
        stateMachine.SetCurrentState(stateMachine.relocatingState);
        stateMachine.relocatingState.Enter(this);

        stateMachine.SetGlobalState(stateMachine.globalState);
    }


    void Update()
    {
        stateMachine.Update();
    }

    public void StartEngageToPlayer()
    {
        isEngagingToPlayer = true;
        stateMachine.engagingPlayerState.Enter(this);
    }

    public void SubtleNoiceDetection()
    {
        // Subtile Noice Detectoin
        if (previousLoudSubtlePosition != NoiceListener.Instance.subtleNoicePosition)
        {

            if (Vector3.Distance(transform.position, NoiceListener.Instance.subtleNoicePosition) <= subtleNoiceDetecitonRange
            && stateMachine.GetCurrentState() != stateMachine.relocatingState
            && !isEngagingToPlayer
            && stateMachine.GetCurrentState() != stateMachine.alertInvestigatingState)
            {
                previousLoudSubtlePosition = noice.position;
                noice.position = NoiceListener.Instance.subtleNoicePosition;
                stateMachine.ChangeState(stateMachine.investigatingState);
            }
        }
    }
    public void LoudNoiceDetection()
    {
        // Loud Noice Detection
        if (previousLoudNoicePosition != NoiceListener.Instance.loudNoicePosition)
        {

            if (Vector3.Distance(transform.position, NoiceListener.Instance.loudNoicePosition) <= loudNoiceDetectionRange
                && stateMachine.GetCurrentState() != stateMachine.relocatingState
                && !isEngagingToPlayer) 
            {
                previousLoudNoicePosition = noice.position;
                noice.position = NoiceListener.Instance.loudNoicePosition; 
                stateMachine.ChangeState(stateMachine.alertInvestigatingState);

            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showCovers)
        {
            Gizmos.color = Color.cyan;

            foreach (Transform coverPosition in coversPositions)
                Gizmos.DrawWireSphere(coverPosition.position, 2);
        }

        Vector3 eyePosition = eyes.position;

        if (showNoiceDetectionRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(eyePosition, loudNoiceDetectionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(eyePosition, subtleNoiceDetecitonRange);

        }

        if (!showVisibility)
            return;

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(eyePosition, playerSpotPoint.position - eyePosition);
        }


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePosition, viewDistance);

        // Desni krajni ugao (halfAngle desno)
        Vector3 rightDirection = Quaternion.Euler(0, viewAngle / 2, 0) * eyes.forward;
        // Levi krajni ugao (halfAngle levo)
        Vector3 leftDirection = Quaternion.Euler(0, -viewAngle / 2, 0) * eyes.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePosition, eyePosition + rightDirection * viewDistance);
        Gizmos.DrawLine(eyePosition, eyePosition + leftDirection * viewDistance);

    }

    // For animtion triggers
    public void EndInvestigation()
    {
        animator.SetTrigger("InvestigationEnd");
        stateMachine.ChangeState(stateMachine.relocatingState);
    }
    public void EndDeath()
    {
        Destroy(gameObject);
    }
    public void EndAttack()
    {
        if(!MessageBroker.Instance.isEngagementOver)
            stateMachine.ChangeState(stateMachine.waitingToAttackState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
            health.TakeDamage(10);
    }
}
