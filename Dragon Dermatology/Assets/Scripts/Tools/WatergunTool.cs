using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatergunTool : Tool
{

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void OngoingAction()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
        if (hits != null && hits.Length != 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<Soap>() != null)
                    {
                        hit.collider.GetComponent<Soap>().Spray();
                    }
                }
            }
        }
    }


    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     // check if collided with dirt particles
    //     if (!inUse) return;
    //     if (col.gameObject.tag != "soap") return;
    //     Destroy(col.gameObject);
    // }
}