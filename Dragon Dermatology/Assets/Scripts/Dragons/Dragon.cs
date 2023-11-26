using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragon : MonoBehaviour
{
    public float waitingTime;
    public bool leftBeforeClient = false; // could be stored in happiness somehow?

    // unity can be a bit fiddly with enums so string here is fine
    public string species; // classic, fire, water

    public List<DragonScale> scales;

    [Range(0f,1f)]
    public float happiness;

    [Range(0f,1f)]
    public float stealth; // placeholder for now

    [Range(0f,1f)]
    public float cleanliness; // placeholder for now

    public int coins;

    void Start()
    {
        // DayManager.Instance.AddToQueue(this);
        DayManager.Instance.SetCurrentClient(this);
    }
}
