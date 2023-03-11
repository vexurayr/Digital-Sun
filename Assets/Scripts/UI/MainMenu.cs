using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;

    public void PlayButton()
    {
        transitionAnimator.SetTrigger("Begin Fade");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}