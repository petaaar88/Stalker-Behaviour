using UnityEngine;

public class Pickup : MonoBehaviour
{
    private ThrowingObject throwingObject;
    private bool isInRange = false;

    void Start()
    {
        throwingObject = FindObjectOfType<ThrowingObject>();
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (throwingObject.AddProjectile())
                Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRange = false;
    }
}
