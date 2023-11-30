using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionTool : Tool
{
    public int scrubRadius = 25;

    public float minSoapTime = 0.5f;
    public float maxSoapTime = 1.5f;
    private float currentSoapTime = 0f;

    public override void Start()
    {
        base.Start();
        ResetSoap();
    }

    void ResetSoap()
    {
        currentSoapTime = Random.Range(minSoapTime, maxSoapTime);
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
                        hit.collider.GetComponent<ScrubbableSpriteMask>().Scrub(mousePosition, scrubRadius);
                    }
                }
            }
        }

        currentSoapTime -= Time.deltaTime;
        if (currentSoapTime <= 0) SpawnSuds();
    }

    public override void Update()
    {
        base.Update();
    }

    void SpawnSuds()
    {
        // instantiate at position then reset
        ResetSoap();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // check if collided with dirt particles
        if (!inUse) return;
        Destroy(col.gameObject);
    }
}
