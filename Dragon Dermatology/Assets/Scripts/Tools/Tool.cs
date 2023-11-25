using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public GameObject visualsObject;
    protected SpriteRenderer visualsRenderer;
    public bool inUse = false;
    public AudioClip sfx;
    protected SFXObject loopingSound; // TODO: Do we need this?
    public Sprite idleSprite;
    public Sprite inUseSprite;

    public virtual void Start()
    {
        visualsRenderer = visualsObject.GetComponent<SpriteRenderer>();
    }

    protected virtual void PerformAction() {}

    public virtual void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            inUse = true;
            loopingSound = AudioManager.Instance.PlaySFXAtPoint(sfx, transform.position, 0, true);
            // tool manager to start and stop sounds instead
        }

        if (Input.GetKeyUp("mouse 0"))
        {
            inUse = false;
            if (loopingSound != null) loopingSound.Stop();
        }

        visualsRenderer.sprite = inUse ? inUseSprite : idleSprite;

        if (inUse) PerformAction();
    }

    public virtual void OnDisable()
    {
        inUse = false;
        if (loopingSound != null) loopingSound.Stop();
    }
}
