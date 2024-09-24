using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public static bool GameIsPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Debug.Log("resume clicked");

        SFXManager.PlaySFX(SoundTypes.Button);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        SFXManager.PlaySFX(SoundTypes.Button);
    }

    public void Help()
    {
        Debug.Log("Help");

        SFXManager.PlaySFX(SoundTypes.Button);
    }

    public void BacktoMenu()
    {
        Debug.Log("Back to Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

        SFXManager.PlaySFX(SoundTypes.Button);
    }

}
