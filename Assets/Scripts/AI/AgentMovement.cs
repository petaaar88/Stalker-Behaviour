using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    private PathSolver pathSolver;
    public Transform target;
    private List<Node> previousPath = null;
    private List<Vector3> nodesPositions = new List<Vector3>();
    public float speed = 2.0f;
    public float baseOffset = 0.0f;
    private float previousBaseOffset = 0.0f;
    public float stoppingDistance = 0.0f;
    public float rotationSpeed = 1.0f;

    private int currentNodeIndex = 0;

    void Start()
    {
        pathSolver = GetComponent<PathSolver>();
        pathSolver.SetSeeker(transform);
        pathSolver.SetTarget(target);
    }

    // Update is called once per frame
    void Update()
    {
        // Setting new base offset
        if (previousBaseOffset != baseOffset)
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

        // Settting new path
        if (previousPath != pathSolver.path)
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

        
 


        if (Vector3.Distance(transform.position, target.position) <= stoppingDistance)
            return;

        // Move agent
        if (nodesPositions != null)
            if (nodesPositions.Count != currentNodeIndex)
            {
                transform.position = Vector3.MoveTowards(transform.position, nodesPositions[currentNodeIndex], speed * Time.deltaTime);

                Vector3 direction = nodesPositions[currentNodeIndex] - transform.position;

                // Ako postoji neka razdaljina
                if (direction != Vector3.zero)
                {
                    // Izračunaj željeni ugao
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    // Postepeno rotiraj ka cilju
                    Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, smoothRotation.eulerAngles.y, 0);
                }


                if (transform.position == nodesPositions[currentNodeIndex])
                    currentNodeIndex++;
            }

    }

    public void SetTarget(Transform target) { this.target = target; pathSolver.SetTarget(target); }
    
}
