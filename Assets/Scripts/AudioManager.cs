using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public  AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Start()
    {
        PlayMusic("PlaceHolder Music");
    }

    public void PlayMusic(string name)
    {
        AudioClip s = Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
        }
        else{
            musicSource.clip = s;
            musicSource.Play();
        }
        
    }

    public void PlaySFX(string name)
    {
        AudioClip s = Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            sfxSource.clip = s;
            sfxSource.Play();
        }
    }

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void MuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void  MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}

//add ths script for the character movements

// AudioManger.Instance.PlaySFX("name of the sound");