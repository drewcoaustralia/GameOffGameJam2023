using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionTool : Tool
{
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
        if (col.gameObject.tag != "wound") return;
        col.gameObject.GetComponent<Wound>().Spray();
    }
}
