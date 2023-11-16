using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance { get; private set; }
    public Texture2D blank;
    public Texture2D hover;

    private GameObject heldObj;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    // void Start()
    // {
    //     Cursor.SetCursor(blank, Vector2.zero, CursorMode.Auto);
    // }

    // public void SetHover(bool hovering)
    // {
    //     if (hovering) Cursor.SetCursor(hover, Vector2.zero, CursorMode.Auto);
    //     else Cursor.SetCursor(blank, Vector2.zero, CursorMode.Auto);
    // }

    // public void SetCursor(Texture2D image)
    // {
    //     Cursor.SetCursor(image, Vector2.zero, CursorMode.Auto);
    // }

    public void SetHeldObject(GameObject obj)
    {
        if (heldObj != null) heldObj.SetActive(false);
        obj.SetActive(true);
        heldObj = obj;
    }

    void Update()
    {
        if (heldObj == null) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        heldObj.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}