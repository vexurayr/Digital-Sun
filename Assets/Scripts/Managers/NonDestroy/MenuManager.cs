using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Variables
    public static MenuManager instance { get; private set; }

    [SerializeField] private string mainMenuName;
    [SerializeField] private string forestLevelName;

    // For in game menu
    [SerializeField] private GameObject inGameSettingsMenu;
    [SerializeField] private Slider inGameMasterVolumeLevel;
    [SerializeField] private Slider inGameMusicVolumeLevel;
    [SerializeField] private Slider inGameSFXVolumeLevel;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Text finalBugsNumber;

    // For start screen menu
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider startScreenMasterVolumeLevel;
    [SerializeField] private Slider startScreenMusicVolumeLevel;
    [SerializeField] private Slider startScreenSFXVolumeLevel;

    #endregion Variables

    #region MonoBehaviours
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
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenu.activeInHierarchy && !deathScreen.activeInHierarchy)
        {
            ToggleInGameSettingsMenu();
        }
    }

    #endregion MonoBehaviours

    #region LoadScenes
    public void LoadForestScene()
    {
        Time.timeScale = 1.0f;

        HideCursor();

        inGameSettingsMenu.SetActive(false);
        deathScreen.SetActive(false);
        mainMenu.SetActive(false);
        
        SceneManager.LoadScene(forestLevelName);

        UpdateAllAudioSliderValues();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;

        ShowCursor();

        inGameSettingsMenu.SetActive(false);
        deathScreen.SetActive(false);
        mainMenu.SetActive(true);

        SceneManager.LoadScene(mainMenuName);

        UpdateAllAudioSliderValues();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion LoadScenes

    #region ChangeMenus
    public void ToggleInGameSettingsMenu()
    {
        ToggleGamePaused();

        UpdateAllAudioSliderValues();

        // The settings menu is currently visible
        if (inGameSettingsMenu.activeInHierarchy)
        {
            inGameSettingsMenu.SetActive(false);

            if (!GameManager.instance.GetCurrentPlayerController().GetIsInventoryActive())
            {
                GameManager.instance.GetCurrentPlayerController().UseMouseToObserve();
            }

            GameManager.instance.GetCurrentPlayerController().SetAreInputsRegistered(true);
        }
        // The settings menu is currently not visible
        else
        {
            GameManager.instance.GetCurrentPlayerController().SetAreInputsRegistered(false);

            inGameSettingsMenu.SetActive(true);

            GameManager.instance.GetCurrentPlayerController().UseMouseToNavigate();
        }
    }

    public void ShowDeathScreen()
    {
        ShowCursor();

        deathScreen.SetActive(true);
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
    }

    #endregion ChangeMenus

    #region RefreshValues
    public void UpdateAllAudioSliderValues()
    {
        startScreenMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeSliderValue();
        startScreenMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeSliderValue();
        startScreenSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeSliderValue();

        inGameMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeSliderValue();
        inGameMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeSliderValue();
        inGameSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeSliderValue();
    }

    #endregion RefreshValues

    #region HelperFunctions
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

    public void ToggleGamePaused()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    public bool IsInGameMenuActive()
    {
        return inGameSettingsMenu.activeInHierarchy;
    }

    #endregion HelperFunctions

    #region AudioFunctions
    public void PlayButtonPressedSound()
    {
        AudioManager.instance.PlaySound2D("Button Press");
    }

    #endregion AudioFunctions
}