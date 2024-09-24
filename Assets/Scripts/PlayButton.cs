using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public GameObject playButtonUI;
    public void PlayGame()
    {
        SFXManager.PlaySFX(SoundTypes.Button);
    }

}
