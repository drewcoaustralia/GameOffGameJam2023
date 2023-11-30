using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    // Skill curve
    [Tooltip("How much should the scale requirements go up each day? Affected scale type is random.")]
    public int daysToComplete;
    [Tooltip("How much should the scale requirements go up each day? Affected scale type is random.")]
    public int goalIncreasePerDay;
    [Tooltip("Each day dragons arrive a bit faster. This gets subtracted from min/maxArrivalTime.")]
    public float arrivalTimeDecreasePerDay;

    [Tooltip("How many seconds pass in the salon for one of our seconds.")]
    public int timeRatio;
    public int hoursInDay;

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

    public int coinsPerBonusRegularScale;
    public int coinsPerBonusFireScale;
    public int coinsPerBonusWaterScale;
    public int coinsPerGoldenScale;
    public int coinsLostPerUnseenDragon;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    public int CurrentDay { get; private set; } = 1;

    public Score Score { get; private set; }

    ///////////////////////////////////////////////
    // Component Lifecycle
    ///////////////////////////////////////////////

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        } 
        else 
        { 
            Destroy(this); 
        } 
    }

    public void StartNextDay(Score previousDayScore)
    {
        Score = Score.Add(previousDayScore);
        CurrentDay++;

        if (CurrentDay >= daysToComplete) {
            // GJ!
            SceneManager.LoadScene("YouWin");
            return;
        }

        // More scales need to be collected
        for (int i = 0; i < goalIncreasePerDay; i++)
        {
            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0:
                    minRegularScaleGoal++;
                    maxRegularScaleGoal++;
                    break;
                case 1:
                    minFireScaleGoal++;
                    maxFireScaleGoal++;
                    break;
                case 2:
                    minWaterScaleGoal++;
                    maxWaterScaleGoal++;
                    break;
            }
        }

        // Dragons arrive faster
        minArrivalTime -= arrivalTimeDecreasePerDay;
        maxArrivalTime -= arrivalTimeDecreasePerDay;

        // Reload the scene
        SceneManager.LoadScene("Gameplay");
    }
}