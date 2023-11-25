using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragonScale : MonoBehaviour
{
    // unity can be a bit fiddly with enums so string here is fine
    public string scaleType; // classic, fire, water, golden

    [Range(0f,1f)]
    public float damage; // probably unused

    [Range(0f,1f)]
    public float cleanliness; // placeholder for now

    public int coins;

    // collection of sprites may be unneccessary at scale level
    public Sprite cleanSprite;
    public Sprite dirtySprite;
    public SpriteMask dirtySpriteMask;
    
}
