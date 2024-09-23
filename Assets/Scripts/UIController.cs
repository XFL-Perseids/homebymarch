using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    public void MuteMusic()
    {
        AudioManager.Instance.MuteMusic();   
    }

    public void MuteSFX()
    {
        AudioManager.Instance.MuteSFX();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

}
