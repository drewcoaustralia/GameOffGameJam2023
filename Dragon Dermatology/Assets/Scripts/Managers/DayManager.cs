using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    public GameObject dragonPrefab;

    [Tooltip("The lowest amount of time it might take for a dragon to show up")]
    public float minArrivalTime;

    [Tooltip("The highest amount of time it might take for a dragon to show up")]
    public float maxArrivalTime;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    private Dragon currentDragon = null;
    private List<QueuedDragon> queue = new List<QueuedDragon>();
    private List<DragonScale> scalesCollected = new List<DragonScale>();
    private int coinsCollected = 0;

    ///////////////////////////////////////////////
    // Behaviour
    ///////////////////////////////////////////////

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        StartCoroutine(QueueRoutine());
    }

    public void SetCurrentClient(Dragon dragon)
    {
        currentDragon = dragon;
    }

    public Dragon GetCurrentClient()
    {
        return currentDragon;
    }

    private IEnumerator QueueRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minArrivalTime, maxArrivalTime));
            var dragonObject = Instantiate(dragonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            QueuedDragon dragon = dragonObject.GetComponent<QueuedDragon>();
            queue.Add(dragon);
        }
    }
}
