using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBroker : MonoBehaviour
{
    public static NoiseBroker Instance { get; private set; }

    private Dictionary<Vector3, NoiseInvestigators> noiseInvestigationDict = new Dictionary<Vector3, NoiseInvestigators>();

    private int lookAroundAnimationsNumber = 4;

    [Header("Debug Settings")]
    [SerializeField] private bool enableDebugLogs = true;
    [SerializeField] private int vectorRoundingDecimals = 2;

    private struct NoiseInvestigators
    {
        public Dictionary<Stalker, StalkerData> StalkersThatInspectNoiseOrigin;
    }

    private struct StalkerData
    {
        public int LookAroundAnimationIndex;
        public Vector3 LandingPosition;
        public StalkerData(int index, Vector3 position)
        {
            LookAroundAnimationIndex = index;
            LandingPosition = position;
        }
    }

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

    void Update()
    {

    }

    // Helper metoda za zaokruživanje Vector3
    private Vector3 RoundVector3(Vector3 pos)
    {
        float multiplier = Mathf.Pow(10f, vectorRoundingDecimals);
        return new Vector3(
            Mathf.Round(pos.x * multiplier) / multiplier,
            Mathf.Round(pos.y * multiplier) / multiplier,
            Mathf.Round(pos.z * multiplier) / multiplier
        );
    }

    public void AddStalkerToInspectNoiseOrigin(Vector3 noisePosition, Stalker stalker)
    {
        // Zaokruži poziciju
        noisePosition = RoundVector3(noisePosition);

        if (enableDebugLogs)
            Debug.Log($"[NoiseBroker] Adding stalker {stalker.name} to investigate position {noisePosition}");

        if (!noiseInvestigationDict.ContainsKey(noisePosition))
        {
            if (enableDebugLogs)
                Debug.Log($"[NoiseBroker] Creating new investigation for position {noisePosition}");

            NoiseInvestigators nInv = new NoiseInvestigators();
            nInv.StalkersThatInspectNoiseOrigin = new Dictionary<Stalker, StalkerData>();
            noiseInvestigationDict.Add(noisePosition, nInv);
        }

        NoiseInvestigators investigators = noiseInvestigationDict[noisePosition];

        // Proveri da li stalker već istražuje ovu poziciju
        if (investigators.StalkersThatInspectNoiseOrigin.ContainsKey(stalker))
        {
            Debug.LogWarning($"[NoiseBroker] Stalker {stalker.name} already investigating position {noisePosition}");
            return;
        }

        // 1. Skup zauzetih indeksa
        HashSet<int> usedIndices = new HashSet<int>();
        foreach (var data in investigators.StalkersThatInspectNoiseOrigin.Values)
        {
            usedIndices.Add(data.LookAroundAnimationIndex);
        }

        // 2. Lista slobodnih indeksa
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < lookAroundAnimationsNumber; i++)
        {
            if (!usedIndices.Contains(i))
                availableIndices.Add(i);
        }

        // 3. Izbor indeksa
        int chosenIndex;
        if (availableIndices.Count > 0)
        {
            chosenIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        }
        else
        {
            chosenIndex = Random.Range(0, lookAroundAnimationsNumber);
        }

        // 4. Dodavanje stalkera
        investigators.StalkersThatInspectNoiseOrigin.Add(
            stalker,
            new StalkerData(chosenIndex, Vector3.zero) // TODO: zameni sa realnom pozicijom
        );

        noiseInvestigationDict[noisePosition] = investigators;

        if (enableDebugLogs)
        {
            Debug.Log($"[NoiseBroker] Successfully added stalker {stalker.name} with animation index {chosenIndex}");
            Debug.Log($"[NoiseBroker] Total stalkers at position {noisePosition}: {investigators.StalkersThatInspectNoiseOrigin.Count}");
        }
    }

    public void RemoveStalkerFromInspectingNoiseOrigin(Vector3 noisePosition, Stalker stalker)
    {
        // Zaokruži poziciju
        noisePosition = RoundVector3(noisePosition);

        if (enableDebugLogs)
            Debug.Log($"[NoiseBroker] Removing stalker {stalker.name} from position {noisePosition}");

        if (!noiseInvestigationDict.ContainsKey(noisePosition))
        {
            if (enableDebugLogs)
                Debug.LogWarning($"[NoiseBroker] Position {noisePosition} not found in dictionary");
            return;
        }

        NoiseInvestigators investigators = noiseInvestigationDict[noisePosition];

        // Jednostavno ukloni stalkera
        bool removed = investigators.StalkersThatInspectNoiseOrigin.Remove(stalker);

        if (enableDebugLogs && removed)
            Debug.Log($"[NoiseBroker] Successfully removed stalker {stalker.name}");
        else if (enableDebugLogs && !removed)
            Debug.LogWarning($"[NoiseBroker] Stalker {stalker.name} was not found at position {noisePosition}");

        // Ako nema više stalkera, ukloni celu poziciju
        if (investigators.StalkersThatInspectNoiseOrigin.Count == 0)
        {
            noiseInvestigationDict.Remove(noisePosition);
            if (enableDebugLogs)
                Debug.Log($"[NoiseBroker] Removed position {noisePosition} from dictionary (no more stalkers)");
        }
        else
        {
            noiseInvestigationDict[noisePosition] = investigators;
        }
    }

    public int GetLookAroundAnimationIndex(Vector3 noisePosition, Stalker stalker)
    {
        // Zaokruži poziciju
        noisePosition = RoundVector3(noisePosition);

        if (enableDebugLogs)
            Debug.Log($"[NoiseBroker] Getting animation index for stalker {stalker.name} at position {noisePosition}");

        if (!noiseInvestigationDict.ContainsKey(noisePosition))
        {
            if (enableDebugLogs)
            {
                Debug.LogWarning($"[NoiseBroker] Position {noisePosition} not found in dictionary!");
                Debug.Log($"[NoiseBroker] Available positions: {noiseInvestigationDict.Count}");
                foreach (var pos in noiseInvestigationDict.Keys)
                {
                    Debug.Log($"  - {pos}");
                }
            }
            return -1;
        }

        NoiseInvestigators investigators = noiseInvestigationDict[noisePosition];

        if (investigators.StalkersThatInspectNoiseOrigin.TryGetValue(stalker, out StalkerData data))
        {
            if (enableDebugLogs)
                Debug.Log($"[NoiseBroker] Found stalker! Returning animation index: {data.LookAroundAnimationIndex}");
            return data.LookAroundAnimationIndex;
        }

        if (enableDebugLogs)
        {
            Debug.LogWarning($"[NoiseBroker] Stalker {stalker.name} not found at position {noisePosition}");
            Debug.Log($"[NoiseBroker] Stalkers at this position: {investigators.StalkersThatInspectNoiseOrigin.Count}");
            foreach (var s in investigators.StalkersThatInspectNoiseOrigin.Keys)
            {
                Debug.Log($"  - {s.name}");
            }
        }

        return -1;
    }

    // Dodatne helper metode za lakše korišćenje
    public bool IsStalkerInvestigating(Vector3 noisePosition, Stalker stalker)
    {
        noisePosition = RoundVector3(noisePosition);

        if (!noiseInvestigationDict.ContainsKey(noisePosition))
            return false;

        return noiseInvestigationDict[noisePosition].StalkersThatInspectNoiseOrigin.ContainsKey(stalker);
    }

    public int GetStalkersCountAtPosition(Vector3 noisePosition)
    {
        noisePosition = RoundVector3(noisePosition);

        if (!noiseInvestigationDict.ContainsKey(noisePosition))
            return 0;

        return noiseInvestigationDict[noisePosition].StalkersThatInspectNoiseOrigin.Count;
    }

    public void ClearAllInvestigationsAtPosition(Vector3 noisePosition)
    {
        noisePosition = RoundVector3(noisePosition);

        if (noiseInvestigationDict.ContainsKey(noisePosition))
        {
            noiseInvestigationDict.Remove(noisePosition);
            if (enableDebugLogs)
                Debug.Log($"[NoiseBroker] Cleared all investigations at position {noisePosition}");
        }
    }

    // Debug metode
    [ContextMenu("Debug Print All Investigations")]
    public void DebugPrintAllInvestigations()
    {
        Debug.Log($"===== NOISE BROKER STATE =====");
        Debug.Log($"Total noise positions: {noiseInvestigationDict.Count}");

        if (noiseInvestigationDict.Count == 0)
        {
            Debug.Log("No active investigations");
            return;
        }

        foreach (var kvp in noiseInvestigationDict)
        {
            Debug.Log($"Position: {kvp.Key}");
            Debug.Log($"  Stalkers investigating: {kvp.Value.StalkersThatInspectNoiseOrigin.Count}");
            foreach (var stalkerKvp in kvp.Value.StalkersThatInspectNoiseOrigin)
            {
                Debug.Log($"    - {stalkerKvp.Key.name}: Animation Index = {stalkerKvp.Value.LookAroundAnimationIndex}, Landing Position = {stalkerKvp.Value.LandingPosition}");
            }
        }
        Debug.Log($"=============================");
    }

    [ContextMenu("Clear All Investigations")]
    public void ClearAllInvestigations()
    {
        noiseInvestigationDict.Clear();
        if (enableDebugLogs)
            Debug.Log("[NoiseBroker] Cleared all investigations");
    }

    // Metoda za testiranje
    [ContextMenu("Test Add Random Stalker")]
    private void TestAddRandomStalker()
    {
        // Ova metoda je samo za testiranje u editoru
        GameObject testStalker = new GameObject($"TestStalker_{Random.Range(0, 100)}");
        Stalker stalkerComponent = testStalker.AddComponent<Stalker>();
        Vector3 testPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));

        AddStalkerToInspectNoiseOrigin(testPosition, stalkerComponent);
        Debug.Log($"[TEST] Added test stalker at position {RoundVector3(testPosition)}");
    }
}