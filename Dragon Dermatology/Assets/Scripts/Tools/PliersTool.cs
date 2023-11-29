using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersTool : Tool
{
    [SerializeField]private GameObject nearestObject = null;
    private Transform cachedParent = null;
    [SerializeField]private float nearestObjectDistance = 0f;

    private GameObject heldObj = null;

    public override void Start()
    {
        base.Start();
    }

    protected override void StartAction()
    {
            inUse = true;
            AudioManager.Instance.PlayRandomSFXAtPoint(sfx, transform.position);
            SetHeldObject(nearestObject);
    }

    protected override void StopAction()
    {
            inUse = false;
            DropHeldObject();
    }

    public override void Update()
    {
        base.Update();
    }

    void SetNearestObject(GameObject obj=null, float dist=0f)
    {
        nearestObjectDistance = dist;
        nearestObject = obj;
    }

    void SetHeldObject(GameObject obj)
    {
        if (obj == null) return;
        heldObj = obj;
        cachedParent = heldObj.transform.parent;
        heldObj.transform.parent = transform;
        nearestObjectDistance = Vector3.Distance(heldObj.transform.position, transform.position);;
        nearestObject = obj;
    }

    void DropHeldObject()
    {
        if (heldObj == null) return;
        heldObj.transform.parent = cachedParent;
        cachedParent = null;
        heldObj = null;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        // TODO rework, check if its pickuppable tag/layer 

        if (collider.gameObject.tag != "scale") return;

        float curDistance = Vector3.Distance(collider.transform.position, transform.position);

        if (nearestObject == null || curDistance < nearestObjectDistance)
        {
            SetNearestObject(collider.gameObject, curDistance);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (nearestObject == collider.gameObject)
        {
            SetNearestObject();
        }
    }
}