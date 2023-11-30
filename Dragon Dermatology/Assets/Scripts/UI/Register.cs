using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    private SpriteRenderer _rend;
    public Sprite idleSprite;
    public Sprite hoverSprite;
    public Sprite activatedSprite;

    private bool activating = false;
    public float activationTime = 2f;

    public List<AudioClip> sfx;

    void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
    }

    public void SetHover(bool hover)
    {
        if (activating) return;
        _rend.sprite = hover ? hoverSprite : idleSprite;
    }

    public void Activate()
    {
        if (activating) return;
        // do code here
        AudioManager.Instance.PlayRandomSFXAtPoint(sfx, transform.position);
        StartCoroutine(SetActivationSprite());
    }

    private IEnumerator SetActivationSprite()
    {
        activating = true;
        _rend.sprite = activatedSprite;
        yield return new WaitForSeconds(activationTime);
        _rend.sprite = idleSprite;
        activating = false;
    }
}
