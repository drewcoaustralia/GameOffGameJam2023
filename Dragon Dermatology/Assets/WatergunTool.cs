using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatergunTool : MonoBehaviour
{

    public GameObject spray;
    private bool flipped = false;
    private bool spraying = false;

    private SFXObject loopingSound;

    public AudioClip waterStart;
    public AudioClip waterLoop;

    [Range(0f,1f)]
    public float minHeightPercentage = 0.1f;

    [Range(0f,1f)]
    public float maxHeightPercentage = 0.2f;

    [Range(0f,1f)]
    public float minWidthPercentage = 0.1f;

    [Range(0f,1f)]
    public float maxWidthPercentage = 0.2f;

    void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            spraying = true;
            AudioManager.Instance.PlaySFXAtPoint(waterStart, transform.position);
            loopingSound = AudioManager.Instance.PlaySFXAtPoint(waterLoop, transform.position, /*waterStart.length*/0f, true);
            spray.SetActive(true);
        }

        if (Input.GetKeyUp("mouse 0"))
        {
            spraying = false;
            loopingSound.Stop();
            spray.SetActive(false);
        }

    }

    void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosFixed = new Vector3(Mathf.Clamp(mousePos.x, minWidthPercentage * Screen.width, maxWidthPercentage * Screen.width), Mathf.Clamp(mousePos.y, minHeightPercentage * Screen.height, maxHeightPercentage * Screen.height), Camera.main.nearClipPlane);
        transform.parent.position = Camera.main.ScreenToWorldPoint(mousePosFixed);
        transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, Camera.main.nearClipPlane);

        // Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)));
        // transform.parent.transform.right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10)) - transform.position;

        // LookAt 2D
        // Vector3 target = new Vector3(0,0,0);
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
        target.z = Camera.main.nearClipPlane;
        // get the angle
        Vector3 norTar = (target-transform.position).normalized;
        float angle = Mathf.Atan2(norTar.y,norTar.x)*Mathf.Rad2Deg;
        // rotate to angle
        Quaternion rotation = new Quaternion ();
        rotation.eulerAngles = new Vector3(0,0,angle);
        transform.parent.rotation = rotation;
        float scale = transform.parent.localScale.x;
        if (mousePos.x < Screen.width / 2)
        {
            flipped = true;
            transform.parent.localScale = new Vector3(scale, Mathf.Abs(scale) * -1, scale);
        }
        else
        {
            transform.parent.localScale = new Vector3(scale, scale, scale);
            flipped = false;
        }

        StretchSpray(target);
    }

    public void StretchSpray(Vector3 target)
    {
        Vector3 centerPos = (transform.position + target) / 2f;
        spray.transform.position = centerPos;
        Vector3 direction = target - transform.position;
        direction = Vector3.Normalize(direction);
        spray.transform.up = direction;
        if (flipped) spray.transform.up *= -1f;
        Vector3 scale = new Vector3(1,1,1);
        scale.x = Vector3.Distance(transform.position, target);
        scale.y = Vector3.Distance(transform.position, target);
        scale.z = Vector3.Distance(transform.position, target);
        spray.transform.localScale = scale;
	}
}


// public class Player : MonoBehaviour {
// 	public Vector3 startPosition = new Vector3(0f,0f,0f);
// 	public Vector3 endPosition = new Vector3(5f, 1f, 0f);
// 	public bool mirrorZ = true;
// 	void Start() {
// 		Strech(gameObject, startPosition, endPosition, mirrorZ);
// 	}


// }