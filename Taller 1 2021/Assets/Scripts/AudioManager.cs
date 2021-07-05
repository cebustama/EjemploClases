using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        instance = this;

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null) 
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void ReproducirCancion(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ReproducirEfecto(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
