using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public bool isCollided = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Unisti");
        isCollided = true;
    }
}
