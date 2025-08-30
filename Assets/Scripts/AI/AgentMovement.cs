using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [HideInInspector]
    public PathSolver pathSolver;
    public Transform target;
    public List<Node> previousPath = null;
    private List<Vector3> nodesPositions = new List<Vector3>();
    public int currentNodeIndex = 0;


    public float speed = 2.0f;
    public float baseOffset = 0.0f;
    private float previousBaseOffset = 0.0f;
    public float stoppingDistance = 0.0f;
    public float rotationSpeed = 1.0f;

    // Debugging

    public bool isUsingAStarDebug = false;

    private void Awake()
    {
        pathSolver = GetComponent<PathSolver>();

    }

    void Start()
    {
        pathSolver.SetSeeker(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // Setting new base offset
        if (previousBaseOffset != baseOffset)
            SetNewBaseOffset();

        // Settting new path
        if (previousPath != pathSolver.path)
            SetNewPath();


        if (Vector3.Distance(transform.position, target.position) <= stoppingDistance - 0.6f) // 0.6f offset 
            return;


        // Move agent
        Vector3 direction = pathSolver.grid.NodeFromWorldPoint(target.position).worldPosition - transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, pathSolver.grid.unwalkableMask))
        {
            isUsingAStarDebug = true;
            pathSolver.canFindPath = true;
            UsePathfinding();
        }
        else
        {
            isUsingAStarDebug = false;
            pathSolver.canFindPath = false;

            GoStraightToTarget(direction);
        }

    }

    private void UsePathfinding()
    {
        if (nodesPositions == null || currentNodeIndex >= nodesPositions.Count)
            return;

        Vector3 targetPos = nodesPositions[currentNodeIndex];
        Vector3 direction = targetPos - transform.position;

        // Ako smo dovoljno blizu, pređi na sledeći waypoint
        if (direction.sqrMagnitude <= 0.25f) // 0.5f^2
        {
            currentNodeIndex++;
            return;
        }

        // Rotacija
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, smoothRotation.eulerAngles.y, 0);
        }

        // Kretanje
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }


    private void GoStraightToTarget(Vector3 direction)
    {
        Vector3 targetPosition = pathSolver.grid.NodeFromWorldPoint(target.position).worldPosition;
        targetPosition.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Rotate towards player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, smoothRotation.eulerAngles.y, 0);
        }
    }

    private void SetNewBaseOffset()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y + baseOffset - previousBaseOffset, transform.position.z);

        if (nodesPositions != null)
            for (int i = 0; i < nodesPositions.Count; i++)
            {
                float offsetDelta = baseOffset - previousBaseOffset;
                Vector3 pos = nodesPositions[i];
                pos.y += offsetDelta;
                nodesPositions[i] = pos;
            }

        previousBaseOffset = baseOffset;

    }

    private void SetNewPath()
    {
        previousPath = pathSolver.path;
        currentNodeIndex = 0;
        nodesPositions.Clear();

        foreach (Node n in pathSolver.path)
        {
            // Adding base offset to nodes in path
            Vector3 position = n.worldPosition;
            position.y += baseOffset;
            nodesPositions.Add(position);
        }
    }

    public void SetTarget(Transform target) { this.target = target; pathSolver.canFindPath = true; pathSolver.SetTarget(target); }

}
