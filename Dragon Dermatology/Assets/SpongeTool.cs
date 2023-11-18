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
}
