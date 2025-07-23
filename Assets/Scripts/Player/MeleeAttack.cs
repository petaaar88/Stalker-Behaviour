using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private Animator animator;
    [SerializeField]
    private GameObject weapon;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("IsAttacking");
        }
    }


    public void EnableCollider()
    {
        weapon.GetComponent<CapsuleCollider>().enabled = true;
    }


    public void DisableCollider()
    {
        weapon.GetComponent<CapsuleCollider>().enabled = false;
    }
}
