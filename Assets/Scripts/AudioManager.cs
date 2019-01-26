using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioSourcePrefab;

    [SerializeField] private Sound[] sounds;

    private IterativeBehaviourPool<AudioSource> audioSourcePool;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        audioSourcePool = new IterativeBehaviourPool<AudioSource>(audioSourcePrefab, 5, transform);
    }

    public AudioSource Play(string _name, Vector3 _position)
    {
        AudioSource audioSource = audioSourcePool.GetPooledObject();
        audioSource.clip = FindClip(_name);
        if (audioSource.clip == null)
        {
            return null;
        }

        audioSource.transform.position = _position;

        audioSource.Play();

        return audioSource;
    }

    public AudioClip FindClip(string _name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _name);
        if (s == null)
        {
            Debug.LogError("Could not find sound using: " + _name);
            return null;
        }

        return s.clip;
    }
}
