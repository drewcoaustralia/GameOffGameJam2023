using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    public EmptyHandTool emptyHand;

    [SerializeField]private Tool heldTool = null;

    public List<AudioClip> pickupSounds;

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

    public void SetHeldTool(Tool tool)
    {
        if (tool == null) return;
        // if (heldTool != null) DropHeldTool();
        if (heldTool != null) return;
        heldTool = tool;
        tool.Pickup();
        AudioManager.Instance.PlayRandomSFXAtPoint(pickupSounds, Vector3.zero);
        emptyHand.gameObject.SetActive(false);
    }

    public void DropHeldTool()
    {
        if (heldTool == null) return;
        heldTool.Drop();
        AudioManager.Instance.PlayRandomSFXAtPoint(pickupSounds, Vector3.zero); // TODO CHANGE SOUNDS
        heldTool = null;
        emptyHand.gameObject.SetActive(true);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        emptyHand.transform.position = Camera.main.ScreenToWorldPoint(mousePos); // plus offset
        if (heldTool != null) heldTool.transform.position = Camera.main.ScreenToWorldPoint(mousePos); // plus offset
    }

    // public void SetHover(bool hover)
    // {
    //     emptyHand.SetHover(hover);
    //     if (hover)
    //     {
    //         if (heldTool != null)
    //         {
    //             heldTool.gameObject.SetActive(false);
    //             emptyHand.gameObject.SetActive(true);
    //         }
    //     }
    //     else
    //     {
    //         if (heldTool != null)
    //         {
    //             heldTool.gameObject.SetActive(true);
    //             emptyHand.gameObject.SetActive(false);
    //         }
    //     }
    // }
}