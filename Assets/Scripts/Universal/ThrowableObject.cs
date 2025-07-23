using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public bool isCollided = false;
    private void OnTriggerEnter(Collider other)
    {
        NoiceListener.Instance.RegisterLoudNoice(gameObject.transform.position);
        isCollided = true;
    }
}
