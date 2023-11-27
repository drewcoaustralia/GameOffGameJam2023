using System;
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
    private List<QueuedDragon> gaveUpWaiting = new List<QueuedDragon>();
    private List<Dragon> visited = new List<Dragon>();
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
        QueuedDragon queued = dragon.gameObject.GetComponentsInChildren<QueuedDragon>()[0];
        queue.Remove(queued);

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
            yield return new WaitForSeconds(UnityEngine.Random.Range(minArrivalTime, maxArrivalTime));

            var dragonObject = Instantiate(dragonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            QueuedDragon dragon = dragonObject.GetComponent<QueuedDragon>();
            dragon.GaveUp += QueuedDragon_OnGaveUp;
            queue.Add(dragon);
        }
    }

    private void QueuedDragon_OnGaveUp(object sender, EventArgs e)
    {
        var queuedDragon = (QueuedDragon)sender;
        queue.Remove(queuedDragon);
        gaveUpWaiting.Add(queuedDragon);

        Dragon dragon = queuedDragon.gameObject.GetComponentsInChildren<Dragon>()[0];
        dragon.SetMode(Mode.Left);
    }
}
