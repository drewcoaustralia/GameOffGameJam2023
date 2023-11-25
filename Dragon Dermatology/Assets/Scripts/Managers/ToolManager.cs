using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    public EmptyHandTool emptyHand;

    private Tool heldTool = null;
    private SFXObject playingSound;

    public AudioClip pickupSound;

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

    void Start()
    {
        Cursor.visible = false;
    }

    public void SetHeldTool(Tool tool)
    {
        if (heldTool != null) heldTool.gameObject.SetActive(false);
        // obj.SetActive(true);
        heldTool = tool;
        AudioManager.Instance.PlaySFXAtPoint(pickupSound, Vector3.zero);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        emptyHand.transform.position = Camera.main.ScreenToWorldPoint(mousePos); // plus offset
        if (heldTool != null) heldTool.transform.position = Camera.main.ScreenToWorldPoint(mousePos); // plus offset
    }

    public void SetHover(bool hover)
    {
        emptyHand.SetHover(hover);
        if (hover)
        {
            if (heldTool != null)
            {
                heldTool.gameObject.SetActive(false);
                emptyHand.gameObject.SetActive(true);
            }
        }
        else
        {
            if (heldTool != null)
            {
                heldTool.gameObject.SetActive(true);
                emptyHand.gameObject.SetActive(false);
            }
        }
    }
}