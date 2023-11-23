using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskTest : MonoBehaviour
{
    public SpriteMask Mask;
    public int radius = 25;
    private Color[] Colors;
    private int Width;
    private int Height;
    
    void Start() 
    {
        //Extract color data once
        Colors = Mask.sprite.texture.GetPixels();
    
        //Store mask dimensionns
        Width = Mask.sprite.texture.width;
        Height = Mask.sprite.texture.height;
    
        ClearMask();       
    }
    
    void ClearMask()
    {                    
        //set all color data to transparent
        for (int i = 0; i < Colors.Length; ++i)
        {
        Colors[i] = new Color(1, 1, 1, 0);
        }
    
        Mask.sprite.texture.SetPixels(Colors);
        Mask.sprite.texture.Apply(false);
    }
    
    //This function will draw a circle onto the texture at position cx, cy with radius r
    public void DrawOnMask(int cx, int cy, int r)
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
    
                if ((py * Width + px) >= 0 && (py * Width + px) < Colors.Length)
                    Colors[py * Width + px] = new Color(1, 0, 0, 1);

                if ((py * Width + nx) >= 0 && (py * Width + nx) < Colors.Length)
                    Colors[py * Width + nx] = new Color(0, 1, 0, 1);

                if ((ny * Width + px) >= 0 && (ny * Width + px) < Colors.Length)
                    Colors[ny * Width + px] = new Color(0, 0, 1, 1);

                if ((ny * Width + nx) >= 0 && (ny * Width + nx) < Colors.Length)
                    Colors[ny * Width + nx] = new Color(1, 1, 1, 1);
            }
        }
    
        Mask.sprite.texture.SetPixels(Colors);
        Mask.sprite.texture.Apply(false);
    }
    
    void Update()
    {
    
        //Get mouse coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
    
        //Check if mouse button is held down
        if (Input.GetKey("mouse 0"))
        {
            //Check if we hit the collider
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {              
                //Normalize to the texture coodinates
                BoxCollider2D box = hit.collider.gameObject.GetComponent<BoxCollider2D>();
                // int y = (int)((0.5 - (Mask.transform.position - mousePosition).y) * Height);
                // int x = (int)((0.5 - (Mask.transform.position - mousePosition).x) * Width);
                // int x = (int)(((mousePosition*2/box.size.x) - Mask.transform.position).x + Width/2);
                // int y = (int)(((mousePosition*2/box.size.y) - Mask.transform.position).y + Height/2);
                int x = (int)((((mousePosition - Mask.transform.position).x)/box.size.x)*Width/hit.collider.gameObject.transform.parent.localScale.x + Width/2);
                int y = (int)((((mousePosition - Mask.transform.position).y)/box.size.y)*Height/hit.collider.gameObject.transform.parent.localScale.y + Height/2);
    
                //Draw onto the mask
                DrawOnMask(x, y, radius);
            }
        }
    }
}
