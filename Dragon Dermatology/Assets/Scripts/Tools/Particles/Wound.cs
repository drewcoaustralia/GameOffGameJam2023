using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Wound : MonoBehaviour
{
    public List<Sprite> sprites;
    private SpriteRenderer _rend;
    
    public float minSize = 0.5f;
    public float maxSize = 1.5f;

    // public float shrinkSpeed = 1f;
    private float alpha = 1f;
    public float fadeSpeed = 1f;


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
        alpha -= fadeSpeed * Time.deltaTime;
        _rend.color = new Color(1f, 1f, 1f, alpha);
        if (alpha <= 0) Destroy(gameObject);
    }
}