using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteMask))]
[RequireComponent(typeof(BoxCollider2D))]
public class ScrubbableSpriteMask : MonoBehaviour
{
    public SpriteRenderer target;
    [Range(0f, 1f)]public float transparency = 0.7f;
    private SpriteMask mask;
    private Color[] maskColors;
    private float maskWidth;
    private float maskHeight;

    private bool[] originalPixels;
    private int originalPixelCount = 0;
    private int currentPixelCount = 0;
    public float cleanliness { get {return currentPixelCount / originalPixelCount;}}

    void Start()
    {
        SetupMask();
    }

    void Update()
    {
        Debug.Log("cleanliness percent: " + cleanliness);
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
        target.color = new Color(1f, 1f, 1f, transparency);
        target.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        Texture2D maskTexture = new Texture2D((int)dirtySprite.textureRect.width, (int)dirtySprite.textureRect.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // to get centre of image
        Rect newRect = new Rect(0f, 0f, maskTexture.width, maskTexture.height);

        Sprite newMask = Sprite.Create(maskTexture, newRect, pivot, dirtySprite.pixelsPerUnit);
        mask.sprite = newMask;

        //Extract color data once
        maskColors = mask.sprite.texture.GetPixels();
        // will need read access to og sprite
        // maskColors = target.sprite.texture.GetPixels();
        originalPixels = new bool[maskColors.Length];
    
        //Store mask dimensions
        maskWidth = target.sprite.bounds.size.x * target.sprite.pixelsPerUnit;
        maskHeight = target.sprite.bounds.size.y * target.sprite.pixelsPerUnit;
    
        ClearMask();       

        // GetComponent<BoxCollider2D>().size = new Vector2(maskWidth / target.sprite.pixelsPerUnit, maskHeight / target.sprite.pixelsPerUnit);
        transform.position = target.transform.position;
        // transform.rotation = target.transform.rotation;
        // transform.localScale = target.transform.localScale;
        GetComponent<BoxCollider2D>().size = target.gameObject.GetComponent<BoxCollider2D>().size;
        Destroy(target.gameObject.GetComponent<PolygonCollider2D>());
        // copy transform into local space
    }

    void ClearMask()
    {         
        // TODO: set initial texture to inverse of main texture, check percentage fill and tare it. then later count pixels not empty for percentage clean           
        //set all color data to transparent
        for (int i = 0; i < maskColors.Length; ++i)
        {
            maskColors[i] = new Color(1, 1, 1, 0);
            originalPixels[i] = (maskColors[i].a != 0);
            originalPixelCount++;
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

    private void Draw(int index, Color color)
    {
        if (index >= 0 && index < maskColors.Length)
        {
            maskColors[index] = new Color(1, 0, 0, 1);
            if (originalPixels[index])
            {
                currentPixelCount++;
                originalPixels[index] = false; // to avoid double counting
            }
        }
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
                int index;

                index = py * (int)maskWidth + px;
                Draw(index, Color.red);
                index = py * (int)maskWidth + nx;
                Draw(index, Color.green);
                index = ny * (int)maskWidth + px;
                Draw(index, Color.blue);
                index = ny * (int)maskWidth + nx;
                Draw(index, Color.white);
            }
        }
    
        mask.sprite.texture.SetPixels(maskColors);
        mask.sprite.texture.Apply(false);
    }
}
