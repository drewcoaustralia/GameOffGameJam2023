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
    public Score Score { get {
        return new Score(
            CoinsCollected,
            RegularScalesCollected,
            FireScalesCollected,
            WaterScalesCollected,
            SeenProudDragons,
            SeenCleanDragons,
            SeenUnhappyDragons,
            UnseenDragons
        );
    }}

    // Timer progress bar step
    private float timeProgressPercent = 0;
    private float timeProgressStepPerSecond;
    private int[] spawnWeights = DailyRules.SpawnWeights(1);

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

        // You'll hit a divide by zero error so just inform the dev that things aren't set up
        if (GameManager.Instance.hoursInDay == 0) {
            Debug.Log("ERROR: GameManager is not configured (hoursInDay)");
            return;
        }
        float secondsInDay = GameManager.Instance.hoursInDay * 3600;
        float stepPerSecond = 1f / secondsInDay;
        timeProgressStepPerSecond = stepPerSecond * GameManager.Instance.timeRatio;

        // Get the dragon spawn weights for this day, or else make them equally weighted
        this.spawnWeights = DailyRules.SpawnWeights(GameManager.Instance.CurrentDay);

        StartCoroutine(QueueRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown("z")) // temporary debugging key
        {
            FinishCurrentClient();
        }
        if (Input.GetKeyDown("c")) // temporary debugging key
        {
            AddCoins(UnityEngine.Random.Range(1, 101));
        }
        if (Input.GetKeyDown("1")) // temporary debugging key
        {
            AddRegularScale();
        }
        if (Input.GetKeyDown("2")) // temporary debugging key
        {
            AddFireScale();
        }
        if (Input.GetKeyDown("3")) // temporary debugging key
        {
            AddWaterScale();
        }
        if (Input.GetKeyDown("4")) // temporary debugging key
        {
            AddGoldenScale();
        }
        if (Input.GetKeyDown("x")) // temporary debugging key
        {
            FinishDay();
        }
        ClockTick(Time.deltaTime);
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

    public void AddRegularScale()
    {
        UIManager.Instance.SetScalesRegular(++RegularScalesCollected);

        // TODO: Dragon looks annoyed
    }

    public void AddFireScale()
    {
        UIManager.Instance.SetScalesFire(++FireScalesCollected);

        // TODO: Dragon looks annoyed
    }

    public void AddWaterScale()
    {
        UIManager.Instance.SetScalesWater(++WaterScalesCollected);

        // TODO: Dragon looks annoyed
    }

    public void AddGoldenScale()
    {
        UIManager.Instance.SetScalesGolden(++GoldenScalesCollected);

        // TODO: Dragon looks annoyed
    }

    public void AddCoins(int coins)
    {
        CoinsCollected += coins;
        UIManager.Instance.SetCoins(CoinsCollected);
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

        AddCoins(currentDragon.AskForPayment());
        Debug.Log($"Finished client. {Score}");

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

        // TODO: Player can use golden scales, then click 'cash out' button when ready
        // Earn some coins when using a golden scale for fun
            // AddCoins(GameManager.Instance.coinsPerGoldenScale);
            // GoldenScalesCollected--;

        // Earn some coins for goals met
        AddCoins(GoldenScalesCollected * GameManager.Instance.coinsPerGoldenScale); // These get reduced if spent on goals
        AddCoins(CoinsForScales(RegularScalesCollected, RegularScaleGoal, GameManager.Instance.coinsPerBonusRegularScale));
        AddCoins(CoinsForScales(FireScalesCollected, FireScaleGoal, GameManager.Instance.coinsPerBonusFireScale));
        AddCoins(CoinsForScales(WaterScalesCollected, WaterScaleGoal, GameManager.Instance.coinsPerBonusWaterScale));

        // Lose coins for dragons that didn't get seen
        AddCoins(-1 * Mathf.Max(0, CoinsCollected - UnseenDragons * GameManager.Instance.coinsLostPerUnseenDragon));

        // TODO: Let the player look at their final earnings, then click 'Done' button when ready

        // Start the next day
        Debug.Log($"Finished day: {Score}");
        GameManager.Instance.StartNextDay(Score);
    }

    private int CoinsForScales(int collected, int goal, int coinsPerBonus)
    {
        if (collected > goal)
        {
            return coinsPerBonus * (collected - goal);
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
    // Clock
    ///////////////////////////////////////////////

    private void ClockTick(float timeDelta)
    {
        timeProgressPercent+= timeProgressStepPerSecond * timeDelta;
        Debug.Log($"Time progress: {timeProgressPercent}");
        UIManager.Instance.SetTimer(timeProgressPercent);
    }

    ///////////////////////////////////////////////
    // Queue
    ///////////////////////////////////////////////

    private IEnumerator QueueRoutine()
    {
        while (queueActive)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(GameManager.Instance.minArrivalTime, GameManager.Instance.maxArrivalTime));

            GameObject dragonObject = Instantiate(dragonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Dragon d = dragonObject.GetComponent<Dragon>();
            QueuedDragon qd = dragonObject.GetComponent<QueuedDragon>();

            // Pick a species
            Species species;
            int idx = WeightedRandom.Index(spawnWeights);
            switch (idx) {
                case 0:
                    species = Species.Regular;
                    break;
                case 1:
                    species = Species.Water;
                    break;
                case 2:
                    species = Species.Fire;
                    break;
                default:
                    throw new Exception($"Unexpected dragon species index: {idx}");
            };
            d.Species = species;

            // Add to queue
            queue.Add(qd);
            UIManager.Instance.SetQueue(queue);

            Debug.Log($"Dragon queued: {species}.");
        }
    }

    public void DragonGaveUpQueueing(QueuedDragon d)
    {
        queue.Remove(d);
        d.NoLongerWaiting = true;

        unseen.Add(d);

        Dragon dragon = d.gameObject.GetComponentsInChildren<Dragon>()[0];
        dragon.SetMode(Mode.Left);
        Debug.Log("Dragon gave up queuing.");
    }
}