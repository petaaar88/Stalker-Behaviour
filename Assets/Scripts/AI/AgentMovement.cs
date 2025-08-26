using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [HideInInspector]
    public PathSolver pathSolver;
    private Transform target;
    private List<Node> previousPath = null;
    private List<Vector3> nodesPositions = new List<Vector3>();
    private int currentNodeIndex = 0;
    

    public float speed = 2.0f;
    public float baseOffset = 0.0f;
    private float previousBaseOffset = 0.0f;
    public float stoppingDistance = 0.0f;
    public float rotationSpeed = 1.0f;

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
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, pathSolver.grid.unwalkableMask))
        {
            pathSolver.canFindPath = true;
            UsePathfinding();
        }
        else
        {
            pathSolver.canFindPath = false;
            if (pathSolver.path != null)
            {
                previousPath.Clear();
                nodesPositions.Clear();
                pathSolver.path.Clear();
                currentNodeIndex = 0;
            }
            GoStraightToTarget(direction);
        }

    }

    private void UsePathfinding()
    {

        if (nodesPositions != null)
            if (nodesPositions.Count != currentNodeIndex)
            {
                transform.position = Vector3.MoveTowards(transform.position, nodesPositions[currentNodeIndex], speed * Time.deltaTime);

                Vector3 direction = nodesPositions[currentNodeIndex] - transform.position;

                // Rotate towards next waypoint
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, smoothRotation.eulerAngles.y, 0);
                }


                if (transform.position == nodesPositions[currentNodeIndex])
                    currentNodeIndex++;
            }
    }

    private void GoStraightToTarget(Vector3 direction)
    {
        Vector3 targetPosition = target.position;
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
