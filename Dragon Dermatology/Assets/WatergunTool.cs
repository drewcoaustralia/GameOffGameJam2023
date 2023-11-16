using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatergunTool : MonoBehaviour
{

    public GameObject spray;

    // [Range(0f,1f)]
    // public float maxHeightPercentage = 0.25f;

    // void LateUpdate()
    // {
    //     Vector3 mousePosFixed = Input.mousePosition;
    //     if (mousePosFixed.y >= maxHeightPercentage * Screen.height)
    //     {
    //         mousePosFixed.y = maxHeightPercentage * Screen.height;
    //         Debug.Log("Fixing y from " + transform.position + " to " + Camera.main.ScreenToWorldPoint(mousePosFixed));
    //         transform.position = Camera.main.ScreenToWorldPoint(mousePosFixed);
    //     }
    // }

    void Update()
    {
        // Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)));
        // transform.parent.transform.right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)) - transform.position;

        // LookAt 2D
        Vector3 target = new Vector3(0,0,0);
        // get the angle
        Vector3 norTar = (target-transform.position).normalized;
        float angle = Mathf.Atan2(norTar.y,norTar.x)*Mathf.Rad2Deg;
        // rotate to angle
        Quaternion rotation = new Quaternion ();
        rotation.eulerAngles = new Vector3(0,0,angle);
        transform.parent.rotation = rotation;
        float scale = transform.parent.localScale.x;
        if (transform.position.x > 0)
        {
            transform.parent.localScale = new Vector3(scale, Mathf.Abs(scale) * -1, scale);
        }
        else transform.parent.localScale = new Vector3(scale, scale, scale);

        spray.SetActive(Input.GetKey("mouse 0"));
    }
}
