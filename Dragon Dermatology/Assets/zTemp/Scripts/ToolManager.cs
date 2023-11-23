using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }
    public Texture2D blank;
    public Texture2D hover;

    private GameObject heldObj;

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

    public void SetHeldObject(GameObject obj)
    {
        if (heldObj != null) heldObj.SetActive(false);
        obj.SetActive(true);
        heldObj = obj;
        AudioManager.Instance.PlaySFXAtPoint(pickupSound, Vector3.zero);
    }

    void Update()
    {
        if (heldObj == null) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        heldObj.transform.position = Camera.main.ScreenToWorldPoint(mousePos); // plus offset
    }
}