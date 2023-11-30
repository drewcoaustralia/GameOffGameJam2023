using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatergunTool : Tool
{
    public int sprayRadius = 10;

    public float minDropTime = 0.5f;
    public float maxDropTime = 1.5f;
    private float currentDropTime = 0f;
    public GameObject dropPrefab;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    void ResetDrops()
    {
        currentDropTime = Random.Range(minDropTime, maxDropTime);
    }

    protected override void OngoingAction()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
        if (hits != null && hits.Length != 0)
        {
            // check for highest layer? or just scrub all
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<ScrubbableSpriteMask>() != null)
                    {
                        hit.collider.GetComponent<ScrubbableSpriteMask>().Scrub(mousePosition, sprayRadius);
                        currentDropTime -= Time.deltaTime;
                        if (currentDropTime <= 0) SpawnDrops(hit.collider.transform);
                    }
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!inUse) return;
        if (col.gameObject.tag != "soap") return;
        col.gameObject.GetComponent<Soap>().Spray();
    }

    void SpawnDrops(Transform parent)
    {
        // instantiate at position then reset
        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity, parent);
        ResetDrops();
    }
}