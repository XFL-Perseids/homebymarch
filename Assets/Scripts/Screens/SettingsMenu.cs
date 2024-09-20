using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;

    public static bool settingsPanel = false;
    
    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel)
            {
                OpenSettings();
            }
            else
            {
                ExitSettings();
            }
        }
    }

    public void OpenSettings()
    {
        settingsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        settingsPanel = true;
    }

    public void ExitSettings()
    {
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        settingsPanel = false;
    }
}
