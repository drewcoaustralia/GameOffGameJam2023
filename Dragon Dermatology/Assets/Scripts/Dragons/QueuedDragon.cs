using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedDragon : MonoBehaviour
{
    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    [Tooltip("The least possible amount of time to wait before giving up.")]
    public float waitTimeLower;

    [Tooltip("The most possible amount of time to wait before giving up.")]
    public float waitTimeUpper;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////
    
    public bool NoLongerWaiting { get; set; }

    ///////////////////////////////////////////////
    // Behaviour
    ///////////////////////////////////////////////

    void Start()
    {
        var giveUpAfter = UnityEngine.Random.Range(waitTimeLower, waitTimeUpper);
        StartCoroutine(GiveUpTimer(giveUpAfter));
    }

    private IEnumerator GiveUpTimer(float giveUpAfter)
    {
        yield return new WaitForSeconds(giveUpAfter);

        if (!NoLongerWaiting) {
            DayManager.Instance.DragonGaveUp(this);
        }
    }
}