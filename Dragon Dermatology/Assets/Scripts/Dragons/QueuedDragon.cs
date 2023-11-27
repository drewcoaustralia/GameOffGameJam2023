using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedDragon : MonoBehaviour
{
    /// <summary>
    /// The dragon gave up waiting.
    /// </summary>
    public event EventHandler GaveUp;

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

    ///////////////////////////////////////////////
    // Behaviour
    ///////////////////////////////////////////////

    void Start()
    {
        var giveUpAfter = UnityEngine.Random.Range(waitTimeLower, waitTimeUpper);
        StartCoroutine(GiveUpTimer(giveUpAfter));
    }

    void Update()
    {
        
    }

    private IEnumerator GiveUpTimer(float giveUpAfter)
    {
        yield return new WaitForSeconds(giveUpAfter);

        if (GaveUp != null)
        {
            GaveUp(this, EventArgs.Empty);
        }
    }
}