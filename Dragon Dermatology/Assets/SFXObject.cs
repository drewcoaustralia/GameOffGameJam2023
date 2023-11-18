using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXObject : MonoBehaviour
{
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    IEnumerator DelayedPlay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        PlayActual();
    }

    private void PlayActual()
    {
        source.Play();
    }

    IEnumerator DelayedStop(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StopActual();
    }

    private void StopActual()
    {
        source.Stop();
        Destroy(gameObject);
    }

    public void Play(float delay=0f)
    {
        if (delay <= 0) PlayActual();
        else StartCoroutine(DelayedPlay(delay));
    }

    public void Stop(float delay=0f)
    {
        if (delay <= 0) StopActual();
        else StartCoroutine(DelayedStop(delay));
    }
}
