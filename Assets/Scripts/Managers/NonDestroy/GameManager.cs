using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance { get; private set; }

    [SerializeField] private PlayerController currentPlayerController;

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

    private void Update()
    {
        
    }

    #endregion MonoBehaviours

    #region GetSet
    public PlayerController GetCurrentPlayerController()
    {
        return currentPlayerController;
    }

    public void SetCurrentPlayerController(PlayerController newController)
    {
        currentPlayerController = newController;
    }

    #endregion GetSet
}