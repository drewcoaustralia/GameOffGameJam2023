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

    [Tooltip("The desired cleanliness as a percentage between 0-1.")]
    public float desiredCleanliness;

    [Tooltip("Reference to the child object which renders the queued dragon.")]
    public GameObject queueRenderObject;

    [Tooltip("Reference to the child object which renders the in-salon dragon.")]
    public GameObject salonRenderObject;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    public bool IsSatisfied { get; private set; }

    private float cleanlinessPercent;

    // Unused - placeholders
    //private List<DragonScale> scales;
    //private float happiness;
    //private float stealth;
    //private float cleanliness;
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

    public void SetCleanlinessPercent(float percent) {
        cleanlinessPercent = percent;
        EvalAndSetIsSatisfied();
    }

    public void EvalAndSetIsSatisfied()
    {
        bool wasSatisfied = IsSatisfied;

        // Satisfaction algorithm
        IsSatisfied = (cleanlinessPercent >= desiredCleanliness);

        // Notify on change
        if (!wasSatisfied && IsSatisfied)
        {
            DayManager.Instance.DragonBecameSatisfied(this);
        }
    }
}
