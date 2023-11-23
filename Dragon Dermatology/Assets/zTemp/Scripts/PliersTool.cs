using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersTool : MonoBehaviour
{
    public GameObject top;
    public GameObject bot;

    private bool animating = false;
    public float offset = 15f;
    [SerializeField]private float progress = 0f;
    public float speed = 60f;

    [SerializeField]private GameObject nearestObject = null;
    [SerializeField]private float nearestObjectDistance = 0f;

    private GameObject heldObj = null;

    public AudioClip snipClip;

    private Color tempColor;

    void Update()
    {
        if (!animating && Input.GetKeyDown("mouse 0"))
        {
            animating = true;
            if (heldObj != null) heldObj = null;
            else if (nearestObject != null) heldObj = nearestObject;
            AudioManager.Instance.PlaySFXAtPoint(snipClip, transform.position);
        }
        if (animating)
        {
            if (progress <= offset) //closing
            {
                top.transform.localEulerAngles += new Vector3(0,0,speed * Time.deltaTime);
                bot.transform.localEulerAngles -= new Vector3(0,0,speed * Time.deltaTime);
                progress += speed * Time.deltaTime;
            }
            else //opening
            {
                if (progress >= 2*offset)
                {
                    animating = false;
                    progress = 0;
                }
                else
                {
                    top.transform.localEulerAngles -= new Vector3(0,0,speed * Time.deltaTime);
                    bot.transform.localEulerAngles += new Vector3(0,0,speed * Time.deltaTime);
                    progress += speed * Time.deltaTime;
                }
            }
        }

        if (heldObj != null) heldObj.transform.position = transform.position;
    }

    void SetNearestObject(GameObject obj=null, float dist=0f)
    {
        if (nearestObject != null) nearestObject.GetComponent<SpriteRenderer>().color = tempColor;
        if (obj != null)
        {
            tempColor = obj.GetComponent<SpriteRenderer>().color;
            obj.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        nearestObjectDistance = dist;
        nearestObject = obj;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
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

    void OnDrawGizmos()
    {
        if (nearestObject == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, nearestObject.transform.position);
    }
}