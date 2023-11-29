using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyHandTool : Tool
{
    private Tool hoveringTool = null;
    private List<GameObject> hoveringToolList;
    public override void Start()
    {
        base.Start();
        onTable = false;
        SetHover(false);
        activeCollider.enabled = true;
        hoveringToolList = new List<GameObject>();
    }

    protected override void StartAction()
    {
        ToolManager.Instance.SetHeldTool(hoveringTool);
    }

    public override void Update()
    {
        // change to pickup tools
        // base.Update();
        if (hoveringToolList.Count > 0)
        {
            SetHover(true);
            float dist = Mathf.Infinity;
            foreach (GameObject tool in hoveringToolList)
            {
                if (Vector3.Distance(tool.transform.position, transform.position) < dist)
                {
                    dist = Vector3.Distance(tool.transform.position, transform.position);
                    hoveringTool = tool.GetComponent<Tool>();
                }
            }
        }
        else
        {
            hoveringTool = null;
            SetHover(false);
        }

        if (Input.GetKeyDown("mouse 0"))
        {
            StartAction();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "tool") return;
        hoveringToolList.Add(collider.transform.parent.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "tool") return;
        hoveringToolList.Remove(collider.transform.parent.gameObject);
    }

    public void SetHover(bool hover)
    {
        inUse = hover;
        visualsRenderer.sprite = hover ? inUseSprite : idleSprite;
    }

    public override void OnDisable()
    {
    }
}
