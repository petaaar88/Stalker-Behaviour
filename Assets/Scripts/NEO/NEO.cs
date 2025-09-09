using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NEO : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            NoiceListener.Instance.RegisterLoudNoice(transform.position);
            foreach (var col in GetComponents<Collider>())
            {
                if (col.isTrigger)
                    col.isTrigger = false;
            }
        }
    }
    
}
