using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolisherTool : Tool
{
    public int scrubRadius = 25;

    public float minSoapTime = 0.5f;
    public float maxSoapTime = 1.5f;
    private float currentSoapTime = 0f;

    public override void Start()
    {
        base.Start();
    }

    protected override void OngoingAction()
    {
    }

    public override void Update()
    {
        base.Update();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!inUse) return;
        if (col.gameObject.tag != "water") return;
        col.gameObject.GetComponent<Water>().Spray();
    }
}
