using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeTool : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Triggered with " + col.gameObject.name);
        Destroy(col.gameObject);
    }
}
