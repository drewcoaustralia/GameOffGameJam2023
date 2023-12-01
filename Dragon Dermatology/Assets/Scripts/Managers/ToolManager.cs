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
    public List<AudioClip> dropSounds;

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

    public bool SetHeldTool(Tool tool)
    {
        if (tool == null) return false;
        if (heldTool != null) return false;
        heldTool = tool;
        tool.Pickup();
        AudioManager.Instance.PlayRandomSFXAtPoint(pickupSounds, Vector3.zero);
        emptyHand.gameObject.SetActive(false);
        return true;
    }

    public void DropHeldTool()
    {
        if (heldTool == null) return;
        heldTool.Drop();
        AudioManager.Instance.PlayRandomSFXAtPoint(dropSounds, Vector3.zero);
        heldTool = null;
        emptyHand.gameObject.SetActive(true);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        emptyHand.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        if (heldTool != null) heldTool.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}