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
    InSalon
}

public class Dragon : MonoBehaviour
{
    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    // unity can be a bit fiddly with enums so string here is fine
    [Tooltip("The kind of dragon.")]
    public Species species;

    [Tooltip("Indicates where the dragon is within the salon.")]
    public Mode initialMode;

    [Tooltip("Reference to the child object which renders the queued dragon.")]
    public GameObject queueRenderObject;

    [Tooltip("Reference to the child object which renders the in-salon dragon.")]
    public GameObject salonRenderObject;

    ///////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////

    private List<DragonScale> scales;
    private float happiness;
    private float stealth; // placeholder for now
    private float cleanliness; // placeholder for now
    private int coins;

    ///////////////////////////////////////////////
    // Behaviour
    ///////////////////////////////////////////////

    void Start()
    {
        DayManager.Instance.SetCurrentClient(this);

        SetMode(initialMode);
    }

    public void SetMode(Mode mode)
    {
        switch (mode) {
            case Mode.Queued:
                queueRenderObject.active = true;
                salonRenderObject.active = false;
                break;
            case Mode.InSalon:
                queueRenderObject.active = false;
                salonRenderObject.active = true;
                break;
        }
    }
}
