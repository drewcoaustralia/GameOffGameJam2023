using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public GameObject visualsObject;
    protected BoxCollider2D visualsCollider;
    protected Collider2D activeCollider;
    protected SpriteRenderer visualsRenderer;
    public bool onTable = true;
    public bool inUse = false;
    public List<AudioClip> sfx;
    protected SFXObject loopingSound;
    public Sprite tableSprite;
    public Sprite idleSprite;
    public Sprite inUseSprite;

    public virtual void Start()
    {
        visualsRenderer = visualsObject.GetComponent<SpriteRenderer>();
        visualsCollider = visualsObject.GetComponent<BoxCollider2D>();
        activeCollider = GetComponent<Collider2D>();
        activeCollider.enabled = false;
    }

    protected virtual void StartAction()
    {
        inUse = true;
        loopingSound = AudioManager.Instance.PlayRandomSFXAtPoint(sfx, transform.position, 0, true);
    }

    protected virtual void StopAction()
    {
        inUse = false;
        if (loopingSound != null) AudioManager.Instance.StopSFX(loopingSound);
    }

    protected virtual void OngoingAction() {}

    public virtual void Update()
    {
        if (onTable) return;

        if (Input.GetKeyDown("mouse 1"))
        {
            Debug.Log("RClick");
            ToolManager.Instance.DropHeldTool();
            return;
        }

        if (Input.GetKeyDown("mouse 0"))
        {
            StartAction();
        }

        if (Input.GetKeyUp("mouse 0"))
        {
            StopAction();
        }

        visualsRenderer.sprite = inUse ? inUseSprite : idleSprite;

        if (inUse) OngoingAction();
    }

    public void Pickup()
    {
        onTable = false;
        inUse = false;
        visualsRenderer.sprite = idleSprite;
        if (activeCollider != null) activeCollider.enabled = true;
        if (visualsCollider != null) visualsCollider.enabled = false;
    }

    public void Drop()
    {
        onTable = true;
        inUse = false;
        visualsRenderer.sprite = tableSprite;
        if (loopingSound != null) AudioManager.Instance.StopSFX(loopingSound);
        if (activeCollider != null) activeCollider.enabled = false;
        if (visualsCollider != null) visualsCollider.enabled = true;
    }

    public virtual void OnDisable()
    {
        Drop();
    }
}
