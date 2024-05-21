using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using Cinemachine;

//I STOLE THIS FROM A BRACKYS TUTORIAL
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource musicSource;
    public Sound[] sounds;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        musicSource = GetComponent<AudioSource>();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
    }

    //Only one track should play at a time
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            musicSource.Stop();
            musicSource.clip = s.clip;
            musicSource.pitch = s.pitch;
            musicSource.volume = s.volume;
            musicSource.Play();

        }
    }

    public void PlaySound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {

            s.source.Play();
        }
    }

    public void PlayUniqueSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) {

            s.source.pitch = s.pitch + UnityEngine.Random.Range(-0.2f, 0.2f);
            s.source.Play();
        }
    }
}
