using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Soap : MonoBehaviour
{
    public List<Sprite> sprites;
    private SpriteRenderer _rend;
    
    public float minSize = 1f;
    public float maxSize = 1.5f;

    public float shrinkSpeed = 1f;
    public float shrinkThreshold = 0.2f;
    public float fadeTime = 0.5f;
    private float alpha = 1f;
    private bool fading = false;


    void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        _rend.sprite = sprites[Random.Range(0,sprites.Count)];
        float scale = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(scale, scale, 1f);
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }

    public void Spray()
    {
        if (transform.localScale == Vector3.zero) return;
        transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, 0);
        if (transform.localScale.x < 0) transform.localScale = Vector3.zero;
        if (transform.localScale.x <= shrinkThreshold && !fading) StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        fading = true;
        for (float t = 0f; t < fadeTime; t += Time.deltaTime)
        {
            alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            _rend.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        alpha = 0f;
        _rend.color = new Color(1f, 1f, 1f, alpha);
        Destroy(gameObject);
    }

}