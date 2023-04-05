using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    // For in game menu
    public GameObject inGameSettingsMenu;
    public Slider inGameMasterVolumeLevel;
    public Slider inGameMusicVolumeLevel;
    public Slider inGameSFXVolumeLevel;
    public GameObject deathScreen;
    public Text deathText;
    public Text finalBugsText;
    public Text finalBugsNumber;

    // For start screen menu
    public GameObject startScreenBackground;
    public GameObject gameNameText;
    public GameObject mainMenu;
    public Slider startScreenMasterVolumeLevel;
    public Slider startScreenMusicVolumeLevel;
    public Slider startScreenSFXVolumeLevel;

    private bool isSettingsMenuActive;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (SettingsManager.instance != null)
        {
            UpdateAllAudioSliderValues();
        }

        if (instance.startScreenBackground.activeInHierarchy == true)
        {
            PlayMainMenuMusic();
        }
        else if (instance.startScreenBackground.activeInHierarchy == false)
        {
            PlayBackgroundMusic();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInGameSettingsMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("RandomMapTesting");

        deathScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleInGameSettingsMenu()
    {
        // Pressing ESC won't bring this menu up in the main menu or if the player has just died
        if (startScreenBackground.activeInHierarchy || deathScreen.activeInHierarchy)
        {
            return;
        }

        ToggleGamePaused();

        UpdateAllAudioSliderValues();

        // The settings menu is currently visible
        if (isSettingsMenuActive)
        {
            isSettingsMenuActive = false;

            inGameSettingsMenu.SetActive(false);

            HideCursor();
        }
        // The settings menu is currently not visible
        else
        {
            isSettingsMenuActive = true;

            inGameSettingsMenu.SetActive(true);

            ShowCursor();
        }
    }

    public void UpdateAllAudioSliderValues()
    {
        startScreenMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeSliderValue();
        startScreenMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeSliderValue();
        startScreenSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeSliderValue();

        inGameMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeSliderValue();
        inGameMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeSliderValue();
        inGameSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeSliderValue();
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    public void HideCursor()
    {
        // Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Makes the cursor invisible
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        StartCoroutine(WaitToGoBackToMainMenu());
    }

    private void NowGoBackToMainMenu()
    {
        SceneManager.LoadScene("StartScreen");

        startScreenBackground.SetActive(true);
        gameNameText.SetActive(true);
        mainMenu.SetActive(true);

        ShowCursor();
    }

    public void ShowDeathScreen()
    {
        ShowCursor();

        ToggleGamePaused();

        deathScreen.SetActive(true);
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
    }

    public void PlayButtonPressedSound()
    {
        //AudioManager.instance.PlaySound("All Button Pressed", gameObject.transform);

        StartCoroutine(WaitToReleaseButton());
    }

    public void PlayButtonReleasedSound()
    {
        //AudioManager.instance.PlaySound("All Button Released", gameObject.transform);
    }

    public void PlayMainMenuMusic()
    {
        if (AudioManager.instance.IsSoundAlreadyPlaying("All Background Music"))
        {
            AudioManager.instance.StopSound("All Background Music");
        }
        if (!AudioManager.instance.IsSoundAlreadyPlaying("All Main Menu Music"))
        {
            AudioManager.instance.PlayLoopingSound("All Main Menu Music", gameObject.transform);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (AudioManager.instance.IsSoundAlreadyPlaying("All Main Menu Music"))
        {
            AudioManager.instance.StopSound("All Main Menu Music");
        }
        if (!AudioManager.instance.IsSoundAlreadyPlaying("All Background Music"))
        {
            AudioManager.instance.PlayLoopingSound("All Background Music", gameObject.transform);
        }
    }

    public void ToggleGamePaused()
    {
        //List<PlayerController> playerControllers = GameManager.instance.players;
    }

    private IEnumerator WaitToReleaseButton()
    {
        yield return new WaitForSeconds(0.2f);

        PlayButtonReleasedSound();
    }

    private IEnumerator WaitToGoBackToMainMenu()
    {
        yield return new WaitForEndOfFrame();

        NowGoBackToMainMenu();
    }
}