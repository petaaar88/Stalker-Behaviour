using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private Health playerHealth;

    void Start()
    {
        playerHealth = gameObject.GetComponent<Health>();

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("StalkerAttackLeft")){
            playerHealth.TakeDamage(10);
        }

        if (other.CompareTag("StalkerAttackRight"))
        {
            playerHealth.TakeDamage(10);
        }
    }
}
