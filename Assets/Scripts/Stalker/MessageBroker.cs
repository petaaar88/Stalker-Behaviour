using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBroker : MonoBehaviour
{
    public static MessageBroker Instance { get; private set; }
    private List<Stalker> engagedStalkers;

    public float engagementTime = 5.0f;
    private float engagementTimer = 0f;
    private bool isTimerStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddStalkerToEngagement(Stalker stalker)
    {
        engagedStalkers.Add(stalker);
    }
}
