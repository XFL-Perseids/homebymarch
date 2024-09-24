using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    public void MuteMusic()
    {
        MusicManager.Instance.MuteMusic();   
    }

    public void MusicVolume()
    {
        MusicManager.Instance.MusicVolume(musicSlider.value);
    }


    //sfx manager
    public void MuteSFX()
    {
        SFXManager.Instance.MuteSFX();
    }
    public void SFXVolume()
    {
        SFXManager.Instance.SFXVolume(sfxSlider.value);
    }

}
