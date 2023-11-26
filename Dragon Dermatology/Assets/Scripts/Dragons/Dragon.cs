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

    // collection of sprites may need to change when we have rigged skeletons
    public SpriteRenderer cleanSpriteRend;
    public SpriteRenderer dirtySpriteRend;
    public SpriteMask dirtySpriteMask;
    public BoxCollider2D dirtySpriteMaskCollider; // does it need this or can we calc from sprite size?
    private Color[] maskColors;
    private float maskWidth;
    private float maskHeight;

    void Start()
    {
        // DayManager.Instance.AddToQueue(this);
        DayManager.Instance.SetCurrentClient(this);
        ResetDirtyLayer();
    }

    void ResetDirtyLayer()
    {
        //Extract color data once
        maskColors = dirtySpriteMask.sprite.texture.GetPixels();
    
        //Store mask dimensions
        // maskWidth = dirtySpriteMask.sprite.texture.width;
        // maskHeight = dirtySpriteMask.sprite.texture.height;
        maskWidth = dirtySpriteRend.sprite.bounds.size.x * dirtySpriteRend.sprite.pixelsPerUnit;
        maskHeight = dirtySpriteRend.sprite.bounds.size.y * dirtySpriteRend.sprite.pixelsPerUnit;
    
        ClearMask();       
    }

    void ClearMask()
    {         
        // TODO: set initial texture to inverse of main texture, check percentage fill and tare it. then later count pixels not empty for percentage clean           
        //set all color data to transparent
        for (int i = 0; i < maskColors.Length; ++i)
        {
            maskColors[i] = new Color(1, 1, 1, 0);
        }
    
        dirtySpriteMask.sprite.texture.SetPixels(maskColors);
        dirtySpriteMask.sprite.texture.Apply(false);
    }

    public void Scrub(Vector3 worldPosition, int radius)
    {
        Vector2 texturePos = new Vector2(worldPosition.x, worldPosition.y);
        texturePos += (Vector2)dirtySpriteRend.sprite.bounds.extents * transform.localScale.x - (Vector2)transform.position;
        texturePos *= dirtySpriteRend.sprite.pixelsPerUnit / transform.localScale.x;

        //Normalize to the texture coodinates
        // int x = (int)((((worldPosition - dirtySpriteMask.transform.position).x)/dirtySpriteMaskCollider.size.x)*maskWidth/transform.localScale.x + maskWidth/2);
        // int y = (int)((((worldPosition - dirtySpriteMask.transform.position).y)/dirtySpriteMaskCollider.size.y)*maskHeight/transform.localScale.y + maskHeight/2);

        //Draw onto the mask
        DrawOnMask((int)texturePos.x, (int)texturePos.y, radius);
    }

    private void DrawOnMask(int cx, int cy, int r)
    {
        int px, nx, py, ny, d;
        
        for (int x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
    
            for (int y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;
    
                if ((py * (int)maskWidth + px) >= 0 && (py * (int)maskWidth + px) < maskColors.Length)
                    maskColors[py * (int)maskWidth + px] = new Color(1, 0, 0, 1);

                if ((py * (int)maskWidth + nx) >= 0 && (py * (int)maskWidth + nx) < maskColors.Length)
                    maskColors[py * (int)maskWidth + nx] = new Color(0, 1, 0, 1);

                if ((ny * (int)maskWidth + px) >= 0 && (ny * (int)maskWidth + px) < maskColors.Length)
                    maskColors[ny * (int)maskWidth + px] = new Color(0, 0, 1, 1);

                if ((ny * (int)maskWidth + nx) >= 0 && (ny * (int)maskWidth + nx) < maskColors.Length)
                    maskColors[ny * (int)maskWidth + nx] = new Color(1, 1, 1, 1);
            }
        }
    
        dirtySpriteMask.sprite.texture.SetPixels(maskColors);
        dirtySpriteMask.sprite.texture.Apply(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ResetDirtyLayer();
        }
    }
}
