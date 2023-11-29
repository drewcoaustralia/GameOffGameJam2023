using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource tempSFXObject;
    public AudioClip bgm;
    private AudioSource _musicSource;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        _musicSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic()
    {
        _musicSource.clip = bgm;
        _musicSource.Play();
    }

    public void StopSFX(SFXObject obj)
    {
        obj.Stop();
    }

    public SFXObject PlaySFXAtPoint(AudioClip clip, Vector3 point, float delay=0f, bool loop=false, float volume=1f)
    {
        AudioSource source = Instantiate(tempSFXObject, point, Quaternion.identity);
        SFXObject obj = source.GetComponent<SFXObject>();
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        obj.Play(delay);
        if (!loop) obj.Stop(clip.length);
        return obj;
    }

    public SFXObject PlayRandomSFXAtPoint(List<AudioClip> clips, Vector3 point, float delay=0f, bool loop=false, float volume=1f)
    {
        if (clips == null || clips.Count <= 0) return null;
        return PlaySFXAtPoint(clips[Random.Range(0,clips.Count)], point, delay, loop, volume);
    }
}
