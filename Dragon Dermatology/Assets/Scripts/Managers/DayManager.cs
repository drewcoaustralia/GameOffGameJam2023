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

    [Tooltip("Toggles the queue on or off.")]
    public bool queueActive = true;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    // Visitors
    private Dragon currentDragon = null;
    private List<QueuedDragon> queue = new List<QueuedDragon>();
    private List<QueuedDragon> unseen = new List<QueuedDragon>();
    private List<Dragon> seenProud = new List<Dragon>();
    private List<Dragon> seenClean = new List<Dragon>();
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
    public int SeenProudDragons { get { return seenProud.Count; } }
    public int SeenCleanDragons { get { return seenClean.Count; } }
    public int SeenUnhappyDragons { get { return seenUnhappy.Count; } }

    ///////////////////////////////////////////////
    // Component Lifecycle
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
        RegularScaleGoal = UnityEngine.Random.Range(GameManager.Instance.minRegularScaleGoal, GameManager.Instance.maxRegularScaleGoal);
        FireScaleGoal = UnityEngine.Random.Range(GameManager.Instance.minFireScaleGoal, GameManager.Instance.maxFireScaleGoal);
        WaterScaleGoal = UnityEngine.Random.Range(GameManager.Instance.minWaterScaleGoal, GameManager.Instance.maxWaterScaleGoal);
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

    ///////////////////////////////////////////////
    // Salon
    ///////////////////////////////////////////////

    public void SetCurrentClient(Dragon dragon)
    {
        // Shouldn't happen but in case (not much testing)
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

    public void RegularScaleCollected()
    {
        RegularScalesCollected++;

        // TODO: Dragon looks annoyed
    }

    public void FireScaleCollected()
    {
        FireScalesCollected++;

        // TODO: Dragon looks annoyed
    }

    public void WaterScaleCollected()
    {
        WaterScalesCollected++;

        // TODO: Dragon looks annoyed
    }

    public void GoldenScaleCollected()
    {
        GoldenScalesCollected++;

        // TODO: Dragon looks annoyed
    }

    public void FinishCurrentClient()
    {
        if (currentDragon == null) {
            Debug.Log("ERROR: FinishCurrentClient: Current dragon is null!");
            return;
        }

        // Place in completion list
        if (currentDragon.IsFeelingProud) {
            seenProud.Add(currentDragon);
        } else if (currentDragon.IsFeelingClean) {
            seenClean.Add(currentDragon);
        } else {
            seenUnhappy.Add(currentDragon);
        }

        CoinsCollected += currentDragon.AskForPayment();

        currentDragon = null;
    }

    public void DragonFeelsCleanChanged(Dragon dragon, bool feelsClean)
    {
        // TODO: Present this to the player somehow. Maybe shouldn't even be here and all done in dragon?
    }

    public void DragonFeelsProudChanged(Dragon dragon, bool feelsProud)
    {
        // TODO: Present this to the player somehow. Maybe shouldn't even be here and all done in dragon?
    }

    ///////////////////////////////////////////////
    // Scoring
    ///////////////////////////////////////////////

    public void FinishDay()
    {
        // Any dragons in the queue will leave unhappily
        while (queue.Count > 0)
        {
            DragonGaveUpQueueing(queue[0]);
        }

        // TODO: Show Summary Splash. Can contain:
        //         IsGoalMet() -- if not, player can use golden scale (GoldenScalesCollected)
        //         RegularScalesCollected / RegularScaleGoal
        //         FireScalesCollected / FireScaleGoal
        //         WaterScalesCollected / WaterScaleGoal
        //         CoinsCollected
        //         SeenProudDragons
        //         SeenCleanDragons
        //         SeenUnhappyDragons
        //         UnseenDragons

        // TODO: Player can use golden scales, then click 'cash out' button when ready
        // Earn some coins when using a golden scale for fun
            // CoinsCollected += GameManager.Instance.coinsPerGoldenScale;
            // GoldenScalesCollected--;

        // Earn some coins for goals met
        CoinsCollected += GoldenScalesCollected * GameManager.Instance.coinsPerGoldenScale; // These get reduced if spent on goals
        CoinsCollected += CoinsForScales(RegularScalesCollected, RegularScaleGoal, GameManager.Instance.coinsAtRegularScaleGoal, GameManager.Instance.coinsPerBonusRegularScale);
        CoinsCollected += CoinsForScales(FireScalesCollected, FireScaleGoal, GameManager.Instance.coinsAtFireScaleGoal, GameManager.Instance.coinsPerBonusFireScale);
        CoinsCollected += CoinsForScales(WaterScalesCollected, WaterScaleGoal, GameManager.Instance.coinsAtWaterScaleGoal, GameManager.Instance.coinsPerBonusWaterScale);

        // TODO: Let the player look at their final earnings, then click 'Done' button when ready

        // Start the next day
        GameManager.Instance.StartNextDay(CoinsCollected);
    }

    private int CoinsForScales(int collected, int goal, int coinsAtGoal, int coinsPerBonus)
    {
        if (collected >= goal)
        {
            return coinsAtGoal + coinsPerBonus * (collected - goal);
        }
        else
        {
            return 0;
        }
    }

    private bool IsGoalMet()
    {
        return RegularScalesCollected >= RegularScaleGoal &&
                FireScalesCollected >= FireScaleGoal &&
                WaterScalesCollected >= WaterScaleGoal;
    }

    ///////////////////////////////////////////////
    // Queue
    ///////////////////////////////////////////////

    private IEnumerator QueueRoutine()
    {
        while (queueActive)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(GameManager.Instance.minArrivalTime, GameManager.Instance.maxArrivalTime));

            // Instantiate and queue a dragon
            GameObject dragonObject = Instantiate(dragonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            QueuedDragon dragon = dragonObject.GetComponent<QueuedDragon>();
            queue.Add(dragon);
        }
    }

    public void DragonGaveUpQueueing(QueuedDragon d)
    {
        queue.Remove(d);
        d.NoLongerWaiting = true;

        unseen.Add(d);

        Dragon dragon = d.gameObject.GetComponentsInChildren<Dragon>()[0];
        dragon.SetMode(Mode.Left);
    }
}