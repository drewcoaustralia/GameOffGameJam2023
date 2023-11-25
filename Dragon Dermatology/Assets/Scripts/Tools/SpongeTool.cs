using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeTool : Tool
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

    protected override void PerformAction()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider == DayManager.Instance.GetCurrentClient().dirtySpriteMaskCollider)
            {
                DayManager.Instance.GetCurrentClient().Scrub(mousePosition, scrubRadius);
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
