using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteMask))]
[RequireComponent(typeof(BoxCollider2D))] // TODO Change to shape and copy from sprite
public class ScrubbableSpriteMask : MonoBehaviour
{
     // collection of sprites may need to change when we have rigged skeletons
    public SpriteRenderer target;
    private SpriteMask mask;
    private Color[] maskColors;
    private float maskWidth;
    private float maskHeight;

    void Start()
    {
        SetupMask();
    }

    void SetupMask()
    {
        mask = GetComponent<SpriteMask>();
        mask.isCustomRangeActive = true;
        mask.frontSortingLayerID = target.sortingLayerID;
        mask.frontSortingOrder = target.sortingOrder + 1;
        mask.backSortingLayerID = target.sortingLayerID;
        mask.backSortingOrder = target.sortingOrder - 1;

        Sprite dirtySprite = target.sprite;

        Texture2D newTexture = new Texture2D((int)dirtySprite.textureRect.width, (int)dirtySprite.textureRect.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // to get centre of image
        Rect newRect = new Rect(0f, 0f, newTexture.width, newTexture.height);

        Sprite newMask = Sprite.Create(newTexture, newRect, pivot);
        mask.sprite = newMask;

        //Extract color data once
        maskColors = mask.sprite.texture.GetPixels();
    
        //Store mask dimensions
        maskWidth = target.sprite.bounds.size.x * target.sprite.pixelsPerUnit;
        maskHeight = target.sprite.bounds.size.y * target.sprite.pixelsPerUnit;
    
        ClearMask();       

        GetComponent<BoxCollider2D>().size = new Vector2(maskWidth / target.sprite.pixelsPerUnit, maskHeight / target.sprite.pixelsPerUnit);
        // copy transform into local space
    }

    void ClearMask()
    {         
        // TODO: set initial texture to inverse of main texture, check percentage fill and tare it. then later count pixels not empty for percentage clean           
        //set all color data to transparent
        for (int i = 0; i < maskColors.Length; ++i)
        {
            maskColors[i] = new Color(1, 1, 1, 0);
        }
    
        mask.sprite.texture.SetPixels(maskColors);
        mask.sprite.texture.Apply(false);
    }

    public void Scrub(Vector3 worldPosition, int radius)
    {
        Vector2 texturePos = new Vector2(worldPosition.x, worldPosition.y);
        texturePos = transform.InverseTransformPoint(texturePos);
        texturePos *= target.sprite.pixelsPerUnit;
        texturePos += new Vector2(maskWidth / 2, maskHeight / 2);

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

                // if outside the edge
                if (nx < 0) continue;
                if (ny < 0) continue;
                if (px >= maskWidth) continue;
                if (py >= maskHeight) continue;
    
                // each direction will draw in a new colour for debugging purposes
                if ((py * (int)maskWidth + px) >= 0 && (py * (int)maskWidth + px) < maskColors.Length)
                {
                    maskColors[py * (int)maskWidth + px] = new Color(1, 0, 0, 1);
                }

                if ((py * (int)maskWidth + nx) >= 0 && (py * (int)maskWidth + nx) < maskColors.Length)
                {
                    maskColors[py * (int)maskWidth + nx] = new Color(0, 1, 0, 1);
                }

                if ((ny * (int)maskWidth + px) >= 0 && (ny * (int)maskWidth + px) < maskColors.Length)
                {
                    maskColors[ny * (int)maskWidth + px] = new Color(0, 0, 1, 1);
                }

                if ((ny * (int)maskWidth + nx) >= 0 && (ny * (int)maskWidth + nx) < maskColors.Length)
                {
                    maskColors[ny * (int)maskWidth + nx] = new Color(1, 1, 1, 1);
                }
            }
        }
    
        mask.sprite.texture.SetPixels(maskColors);
        mask.sprite.texture.Apply(false);
    }
}
