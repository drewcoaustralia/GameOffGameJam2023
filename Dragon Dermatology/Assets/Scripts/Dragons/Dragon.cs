using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum Species {
    Regular,
    Fire,
    Water
}

public enum Mode {
    Queued,
    InSalon,
    Left
}

public class Dragon : MonoBehaviour
{
    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    [Tooltip("The kind of dragon.")]
    public Species species;

    [Tooltip("Indicates where the dragon is within the salon building.")]
    public Mode initialMode;

    [Range(0, 1)]
    [Tooltip("The desired cleanliness as a percentage between 0-1.")]
    public float desiredCleanliness;

    [Range(0, 1)]
    [Tooltip("The desired polish as a percentage between 0-1.")]
    public float desiredPolish;

    public int minCoinsIfClean;
    public int maxCoinsIfClean;
    public int minCoinsIfProud;
    public int maxCoinsIfProud;


    [Tooltip("Reference to the child object which renders the queued dragon.")]
    public GameObject queueRenderObject;

    [Tooltip("Reference to the child object which renders the in-salon dragon.")]
    public GameObject salonRenderObject;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    public bool IsFeelingClean { get; private set; }
    public bool IsFeelingProud { get; private set; }

    private float cleanlinessPercent;
    private float polishPercent;
    private int numSuds;

    // Unused - placeholders
    //private List<DragonScale> scales;
    //private float happiness;
    //private float stealth;
    //private int coins;

    ///////////////////////////////////////////////
    // Behaviour
    ///////////////////////////////////////////////

    void Start()
    {
        SetMode(initialMode);
    }

    public void SetMode(Mode mode)
    {
        switch (mode) {
            case Mode.Queued:
                queueRenderObject.SetActive(true);
                salonRenderObject.SetActive(false);
                break;
            case Mode.InSalon:
                queueRenderObject.SetActive(false);
                salonRenderObject.SetActive(true);
                break;
            case Mode.Left:
                queueRenderObject.SetActive(false);
                salonRenderObject.SetActive(false);
                break;
        }
    }

    public int AskForPayment()
    {
        return (IsFeelingClean ? UnityEngine.Random.Range(minCoinsIfClean, maxCoinsIfClean) : 0) +
                (IsFeelingProud ? UnityEngine.Random.Range(minCoinsIfProud, maxCoinsIfProud) : 0);
    }

    public void SetCleanlinessPercent(float percent) {
        cleanlinessPercent = percent;
        EvalFeelings();
    }

    public void SetPolishPercent(float percent) {
        polishPercent = percent;
        EvalFeelings();
    }

    public void AddSud() {
        numSuds++;
        EvalFeelings();
    }

    public void RemoveSud() {
        numSuds--;
        EvalFeelings();
    }

    private void EvalFeelings()
    {
        // Satisfaction algorithm
        bool wasClean = IsFeelingClean;
        IsFeelingClean = (cleanlinessPercent >= desiredCleanliness && numSuds < 1);
        if (wasClean != IsFeelingClean)
        {
            DayManager.Instance.DragonFeelsCleanChanged(this, IsFeelingClean);
        }

        bool wasProud = IsFeelingProud;
        IsFeelingProud = (IsFeelingClean && polishPercent >= desiredPolish);
        if (wasProud != IsFeelingProud)
        {
            DayManager.Instance.DragonFeelsProudChanged(this, IsFeelingProud);
        }
    }
}
