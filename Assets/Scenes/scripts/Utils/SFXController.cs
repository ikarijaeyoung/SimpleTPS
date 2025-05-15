using DesignPattern;
using UnityEngine;

public class SFXController : PooledObject
{
    private AudioSource _audioSource;
    private float _currentCount;


    private void Awake() => Init();
    private void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        // _audioSource.playOnAwake
    }
    private void Update()
    {
        _currentCount -= Time.deltaTime;
        if (_currentCount <= 0) ReturnPool();
    }

    public void Play(AudioClip clip)
    {
        // _audioSource.loop = loop;
        // _audioSource.playOnAwake = onAwake;

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        _currentCount = clip.length;
    }
}
