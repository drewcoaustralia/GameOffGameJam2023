using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeTool : MonoBehaviour
{
    private bool scrubbing = false;
    public AudioClip scrubbingLoop;
    private SFXObject loopingSound;
    public Sprite idle;
    public Sprite soapy;

    public int scrubRadius = 25;
    public SpriteMask dirtyMask;
    private Color[] maskColors;
    private int maskWidth;
    private int maskHeight;
    private bool tempFixForNoDragonPrefab = false;

    public Vector2 offset;

    private SpriteRenderer _rend;

    public float minSoapTime = 0.5f;
    public float maxSoapTime = 1.5f;
    private float currentSoapTime = 0f;

    void Start()
    {
        _rend = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        if (!tempFixForNoDragonPrefab)
        {
            ResetDirtyLayer();
            tempFixForNoDragonPrefab = true;
            transform.parent.gameObject.SetActive(false);
        }
        ResetSoap();
    }

    void ResetDirtyLayer()
    {
        //Extract color data once
        maskColors = dirtyMask.sprite.texture.GetPixels();
    
        //Store mask dimensionns
        maskWidth = dirtyMask.sprite.texture.width;
        maskHeight = dirtyMask.sprite.texture.height;
    
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
    
        dirtyMask.sprite.texture.SetPixels(maskColors);
        dirtyMask.sprite.texture.Apply(false);
    }

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
    
                if ((py * maskWidth + px) >= 0 && (py * maskWidth + px) < maskColors.Length)
                    maskColors[py * maskWidth + px] = new Color(1, 0, 0, 1);

                if ((py * maskWidth + nx) >= 0 && (py * maskWidth + nx) < maskColors.Length)
                    maskColors[py * maskWidth + nx] = new Color(0, 1, 0, 1);

                if ((ny * maskWidth + px) >= 0 && (ny * maskWidth + px) < maskColors.Length)
                    maskColors[ny * maskWidth + px] = new Color(0, 0, 1, 1);

                if ((ny * maskWidth + nx) >= 0 && (ny * maskWidth + nx) < maskColors.Length)
                    maskColors[ny * maskWidth + nx] = new Color(1, 1, 1, 1);
            }
        }
    
        dirtyMask.sprite.texture.SetPixels(maskColors);
        dirtyMask.sprite.texture.Apply(false);
    }

    void ResetSoap()
    {
        currentSoapTime = Random.Range(minSoapTime, maxSoapTime);
    }

    void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            scrubbing = true;
            loopingSound = AudioManager.Instance.PlaySFXAtPoint(scrubbingLoop, transform.position, 0, true);
        }

        if (Input.GetKeyUp("mouse 0"))
        {
            scrubbing = false;
            loopingSound.Stop();
        }

        _rend.sprite = scrubbing ? soapy : idle;
        if (scrubbing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {              
                //Normalize to the texture coodinates
                BoxCollider2D box = hit.collider.gameObject.GetComponent<BoxCollider2D>();
                int x = (int)((((mousePosition - dirtyMask.transform.position).x)/box.size.x)*maskWidth/hit.collider.gameObject.transform.parent.localScale.x + maskWidth/2);
                int y = (int)((((mousePosition - dirtyMask.transform.position).y)/box.size.y)*maskHeight/hit.collider.gameObject.transform.parent.localScale.y + maskHeight/2);
    
                //Draw onto the mask
                DrawOnMask(x, y, scrubRadius);
            }

            currentSoapTime -= Time.deltaTime;
            if (currentSoapTime <= 0) SpawnSuds();
        }
    }

    void SpawnSuds()
    {
        // instantiate at position then reset
        ResetSoap();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!scrubbing) return;
        Destroy(col.gameObject);
    }

    void LateUpdate()
    {
        transform.parent.position += new Vector3(offset.x, offset.y, 0);
    }
}
