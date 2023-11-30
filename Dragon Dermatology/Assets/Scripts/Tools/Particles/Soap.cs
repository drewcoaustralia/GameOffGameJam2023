using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Soap : MonoBehaviour
{
    public List<Sprite> sprites;
    private SpriteRenderer _rend;
    
    public float minSize = 0.5f;
    public float maxSize = 1.5f;

    public float shrinkSpeed = 1f;


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
        transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, 1);
        if (transform.localScale.x <= 0) Destroy(gameObject);
    }
}