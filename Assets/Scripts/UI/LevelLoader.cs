using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    private void Awake()
    {
        // Only allows for one game manager, one singleton
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadForestLevel()
    {
        SceneManager.LoadScene("Forest");
    }

    public void LoadMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }

    public void Lose()
    {
        StartCoroutine(WaitFor(2f));
    }

    public IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        LoadMainMenu();
    }
}