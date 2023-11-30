using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmptyHandTool : Tool
{
    private Tool hoveringTool = null;
    private List<GameObject> hoveringToolList;
    private GameObject hoveringUIObject;
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
        if (hoveringUIObject != null)
        {
            switch (hoveringUIObject.tag)
            {
                case "register":
                {
                    hoveringUIObject.GetComponent<Register>().Activate();
                    return;
                }
                default:
                    // no hovering tag found, break to normal usage
                    break;
            }
        }
        else
        {
            ToolManager.Instance.SetHeldTool(hoveringTool);
        }
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
                    if (hoveringTool != null) hoveringTool.SetHover(false);
                    hoveringTool = tool.GetComponent<Tool>();
                    hoveringTool.SetHover(true);
                }
            }
        }
        else
        {
            if (hoveringTool != null) hoveringTool.SetHover(false);
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
        switch (collider.gameObject.tag)
        {
            case "register":
            {
                hoveringUIObject = collider.gameObject;
                collider.gameObject.GetComponent<Register>().SetHover(true);
                return;
            }
            case "tool":
            {
                hoveringToolList.Add(collider.transform.parent.gameObject);
                return;
            }
            default:
            {
                return;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "register":
            {
                hoveringUIObject = null;
                collider.gameObject.GetComponent<Register>().SetHover(false);
                return;
            }
            case "tool":
            {
                hoveringToolList.Remove(collider.transform.parent.gameObject);
                return;
            }
            default:
            {
                return;
            }
        }
    }

    public override void SetHover(bool hover)
    {
        inUse = hover;
        visualsRenderer.sprite = hover ? inUseSprite : idleSprite;
    }

    public override void OnDisable()
    {
    }
}
