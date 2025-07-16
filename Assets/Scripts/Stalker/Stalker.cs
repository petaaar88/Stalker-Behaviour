using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    private PathSolver pathSolver;
    public Transform target;

    void Start()
    {
        pathSolver = GetComponent<PathSolver>();
        pathSolver.SetTarget(target);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
