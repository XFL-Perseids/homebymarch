using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SFXManager : MonoBehaviour
{
    [SerializeField] public SoundList[] soundList;
    public static SFXManager Instance;
    public AudioSource sfxSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
    }

    public static void PlaySFX(SoundTypes sfx)
    {
        AudioClip[] clips = Instance.soundList[(int)sfx].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.sfxSource.PlayOneShot(randomClip);
    }

//for uicontrollers
    public void MuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    private void OnValidate()
    {
        string[] names = Enum.GetNames(typeof(SoundTypes));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }


[Serializable]
    public struct SoundList
    {
        public AudioClip[] Sounds { get => sfx; }
        [HideInInspector]public string name;
        [SerializeField]public AudioClip[] sfx;
    }

}
