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

    [Tooltip("Reference to the child object which renders the queued dragon.")]
    public GameObject queueRenderObject;

    [Tooltip("Reference to the child object which renders the in-salon dragon.")]
    public GameObject salonRenderObject;

    ///////////////////////////////////////////////
    // Init
    ///////////////////////////////////////////////

    public Species Species { get; set; }

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    public bool IsFeelingClean { get; private set; }
    public bool IsFeelingProud { get; private set; }

    private float cleanlinessPercent;
    private float polishPercent;
    private int numSuds;

    private float desiredCleanliness = 1f;
    private float desiredPolish = 1f;

    ///////////////////////////////////////////////
    // Behaviour
    ///////////////////////////////////////////////

    void Start()
    {
        SetMode(Mode.Queued);
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
        return (IsFeelingClean ? DragonTraits.CoinsIfClean[Species] : 0) +
                (IsFeelingProud ? DragonTraits.CoinsIfPolished[Species] : 0);
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
