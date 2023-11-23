using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeTool : MonoBehaviour
{
    private bool scrubbing = false;
    public AudioClip scrubbingLoop;
    private SFXObject loopingSound;
    public Sprite idle;
    public Sprite soapy;

    public int scrubRadius = 25;
    public Dragon client; //TODO: store in tools manager

    public Vector2 offset;

    private SpriteRenderer _rend;

    public float minSoapTime = 0.5f;
    public float maxSoapTime = 1.5f;
    private float currentSoapTime = 0f;

    void Start()
    {
        _rend = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        ResetSoap();
    }

    void ResetSoap()
    {
        currentSoapTime = Random.Range(minSoapTime, maxSoapTime);
    }

    void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            scrubbing = true;
            loopingSound = AudioManager.Instance.PlaySFXAtPoint(scrubbingLoop, transform.position, 0, true);
        }

        if (Input.GetKeyUp("mouse 0"))
        {
            scrubbing = false;
            loopingSound.Stop();
        }

        _rend.sprite = scrubbing ? soapy : idle;
        if (scrubbing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                client.Scrub(mousePosition, scrubRadius);
            }

            currentSoapTime -= Time.deltaTime;
            if (currentSoapTime <= 0) SpawnSuds();
        }
    }

    void SpawnSuds()
    {
        // instantiate at position then reset
        ResetSoap();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!scrubbing) return;
        Destroy(col.gameObject);
    }

    void LateUpdate()
    {
        transform.parent.position += new Vector3(offset.x, offset.y, 0);
    }
}
