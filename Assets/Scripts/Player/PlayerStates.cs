using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public enum States
    {
        WALKING,
        SPRINTING,
        CROUCHING,
        IDLE
    }

    public States currentState;
    private bool isCrouching;
    public string utrenutnomstanju;
    void Start()
    {
        currentState = States.IDLE;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        if (isCrouching)
        {
            currentState = States.CROUCHING;
            
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) 
            && Input.GetKey(KeyCode.LeftShift))
        {
            currentState = States.SPRINTING;
            NoiceListener.Instance.RegisterLoudNoice(transform.position);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            currentState = States.WALKING;
            NoiceListener.Instance.RegisterSubtleNoice(transform.position);
        }
        else
        {
            currentState = States.IDLE;
        }

        
    }

}
