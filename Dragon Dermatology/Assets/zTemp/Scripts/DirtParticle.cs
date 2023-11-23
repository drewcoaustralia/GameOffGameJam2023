using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DirtParticle : MonoBehaviour
{
    public List<Color> colors;
    public List<Sprite> sprites;

    private SpriteRenderer _rend;

    void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        _rend.sprite = sprites[Random.Range(0,sprites.Count)];
        _rend.color = colors[Random.Range(0,colors.Count)];
        transform.localPosition = new Vector3(Random.Range(-1.5f,2f), Random.Range(-3f,0f), 0f);
        float scale = Random.Range(0.03f,0.05f);
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
