using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private KeyCode interactionKey = KeyCode.E;

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactionKey))
            CollectKey();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerNearby = true;
            Debug.Log("Press " + interactionKey + " to pick up the key.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
            isPlayerNearby = false;
    }

    private void CollectKey()
    {
        Debug.Log("Key collected!");

        if (GameManager.Instance != null)
            GameManager.Instance.SetKeyCollected(true);
        else
            Debug.LogError("GameManager instance not found!");

        Destroy(gameObject);
    }
}