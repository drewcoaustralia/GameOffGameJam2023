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

    [Tooltip("The lowest amount of time it might take for a dragon to show up.")]
    public float minArrivalTime;

    [Tooltip("The highest amount of time it might take for a dragon to show up.")]
    public float maxArrivalTime;

    [Tooltip("The least regular scale goal for a day.")]
    public int minRegularScaleGoal;
    [Tooltip("The most regular scales goal for a day.")]
    public int maxRegularScaleGoal;
    [Tooltip("The least fire scales goal for a day.")]
    public int minFireScaleGoal;
    [Tooltip("The most fire scales goal for a day.")]
    public int maxFireScaleGoal;
    [Tooltip("The least water scales goal for a day.")]
    public int minWaterScaleGoal;
    [Tooltip("The most water scales goal for a day.")]
    public int maxWaterScaleGoal;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    // Visitors
    private Dragon currentDragon = null;
    private List<QueuedDragon> queue = new List<QueuedDragon>();
    private List<QueuedDragon> unseen = new List<QueuedDragon>();
    private List<Dragon> seenHappy = new List<Dragon>();
    private List<Dragon> seenUnhappy = new List<Dragon>();

    // Daily goal
    public int RegularScaleGoal { get; private set; } = 0;
    public int FireScaleGoal { get; private set; } = 0;
    public int WaterScaleGoal { get; private set; } = 0;

    // Earnings
    public int CoinsCollected { get; private set; } = 0;
    public int RegularScalesCollected { get; private set; } = 0;
    public int FireScalesCollected { get; private set; } = 0;
    public int WaterScalesCollected { get; private set; } = 0;
    public int GoldenScalesCollected { get; private set; } = 0;
    public int UnseenDragons { get { return unseen.Count; } }
    public int SeenHappyDragons { get { return seenHappy.Count; } }
    public int SeenUnhappyDragons { get { return seenUnhappy.Count; } }

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
        RegularScaleGoal = UnityEngine.Random.Range(minRegularScaleGoal, maxRegularScaleGoal);
        FireScaleGoal = UnityEngine.Random.Range(minFireScaleGoal, maxFireScaleGoal);
        WaterScaleGoal = UnityEngine.Random.Range(minWaterScaleGoal, maxWaterScaleGoal);
        StartCoroutine(QueueRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown("z")) // temporary debugging key
        {
            FinishCurrentClient();
        }
        else if (Input.GetKeyDown("x")) // temporary debugging key
        {
            FinishDay();
        }
    }

    public void SetCurrentClient(Dragon dragon)
    {
        // It's best to avoid defensive programming like this.. but just in case :)
        if (currentDragon)
        {
            FinishCurrentClient();
        }

        // Remove from queue
        QueuedDragon queued = dragon.gameObject.GetComponentsInChildren<QueuedDragon>()[0];
        queued.NoLongerWaiting = true;
        queue.Remove(queued);

        currentDragon = dragon;
    }

    public Dragon GetCurrentClient()
    {
        return currentDragon;
    }

    public void FinishCurrentClient()
    {
        if (currentDragon == null) {
            Debug.Log("ERROR: FinishCurrentClient: Current dragon is null!");
            return;
        }

        // Place in completion list
        if (currentDragon.IsSatisfied) {
            seenHappy.Add(currentDragon);
        } else {
            seenUnhappy.Add(currentDragon);
        }

        currentDragon = null;
    }

    public void FinishDay()
    {
        // Any dragons in the queue will leave unhappily
        while (queue.Count > 0)
        {
            MakeDragonLeaveQueue(queue[0]);
        }
    }

    public void DragonGaveUp(QueuedDragon dragon)
    {
        MakeDragonLeaveQueue(dragon);
    }

    public void DragonBecameSatisfied(Dragon dragon)
    {
        // TODO: Present this to the player somehow
    }

    private IEnumerator QueueRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minArrivalTime, maxArrivalTime));

            // Instantiate and queue a dragon
            var dragonObject = Instantiate(dragonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            QueuedDragon dragon = dragonObject.GetComponent<QueuedDragon>();
            queue.Add(dragon);
        }
    }

    private void MakeDragonLeaveQueue(QueuedDragon d) {
        queue.Remove(d);
        d.NoLongerWaiting = true;

        unseen.Add(d);

        Dragon dragon = d.gameObject.GetComponentsInChildren<Dragon>()[0];
        dragon.SetMode(Mode.Left);
    }
}