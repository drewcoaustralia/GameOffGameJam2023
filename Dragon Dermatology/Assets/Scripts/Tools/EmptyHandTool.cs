using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyHandTool : Tool
{
    public Vector3 idleOffset;
    public Vector3 inUseOffset;
    
    public bool isHovering = false;

    public override void Start()
    {
        base.Start();
        SetHover(false);
    }

    protected override void PerformAction()
    {
    }

    public override void Update()
    {
        // base.Update();
    }

    public void SetHover(bool hover)
    {
        isHovering = hover;
        visualsRenderer.sprite = hover ? inUseSprite : idleSprite;
        visualsObject.transform.localPosition = hover ? inUseOffset : idleOffset;
    }
}
