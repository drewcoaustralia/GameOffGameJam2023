using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScript : MonoBehaviour
{
    // public Texture2D cursor;
    public GameObject cursorObj;
    private bool hovering = false;
    private bool pickedup = false;

    public void SetHover(bool hover)
    {
        if (pickedup) return;
        hovering = hover;
        // MouseManager.Instance.SetHover(hovering);
    }

    public void Pickup()
    {
        if (!hovering) return;
        // MouseManager.Instance.SetCursor(cursor);
        MouseManager.Instance.SetHeldObject(cursorObj);
        pickedup = true;
    }
}