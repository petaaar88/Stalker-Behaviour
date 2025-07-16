using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSolver : MonoBehaviour
{
    private Grid grid;
    private Transform target;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
        
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node tagetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        if(target != null)
            FindPath(transform.position, target.position);
    }


    public void SetTarget(Transform target) { this.target = target; }
}
