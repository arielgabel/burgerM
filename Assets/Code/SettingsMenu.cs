using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SettingsMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject settingsMenuUI;

    public void HandleClick()
    {
        if (!GameIsPaused && !settingsMenuUI.activeInHierarchy)
            PauseNShowMenu();
        else if (settingsMenuUI.activeInHierarchy)
            ReturnToGame();
        else
            return;

        GameIsPaused = !GameIsPaused;
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
    }

    public void ReturnToGame()
    {
        settingsMenuUI.SetActive(false); // im turning of father object so cant see animatino
        Time.timeScale = 1f;
    }

    public void PauseNShowMenu()
    {
        Time.timeScale = 0f;
        settingsMenuUI.SetActive(true);
    }

    public void RestartMyGame()
    {
        GameIsPaused = !GameIsPaused;
        Time.timeScale = 1f;
        settingsMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelUpClick()
    {
        Image tempImage = settingsMenuUI.GetComponent<Image>();
        tempImage.enabled = false;
        Animator knife = settingsMenuUI.transform.GetChild(1).GetComponent<Animator>();
        knife.SetBool("ScreenPressed", true);
        Time.timeScale = 1.0f;
    }

    public bool gameState()
    {
        return GameIsPaused;
    }
}
