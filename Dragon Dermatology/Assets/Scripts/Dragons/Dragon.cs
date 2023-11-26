using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragon : MonoBehaviour
{
    ///////////////////////////////////////////////
    // Component references
    ///////////////////////////////////////////////

    // collection of sprites may need to change when we have rigged skeletons
    // TODO: Add references to the new rig and animations
    // public SpriteRenderer cleanSpriteRend;
    // public SpriteRenderer dirtySpriteRend;
    // public SpriteMask dirtySpriteMask;

    ///////////////////////////////////////////////
    // Settings
    ///////////////////////////////////////////////

    // unity can be a bit fiddly with enums so string here is fine
    public string species; // classic, fire, water

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
    }
}
