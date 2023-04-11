using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance { get; private set; }

    [SerializeField] private PlayerController currentPlayerController;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 playerSpawn;

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

    private void Start()
    {
        SpawnerManager.instance.SpawnOnNewZone();
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

    #region SpawnerFunctions
    public void SpawnPlayer(PlayerInventory inventory, bool isRespawningFromTerminal)
    {
        if (currentPlayerController != null)
        {
            Debug.Log("Player already exists.");
            return;
        }
        
        GameObject spawnedPlayer = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
        
        if (isRespawningFromTerminal)
        {
            Debug.Log("Setting Inventory");
            spawnedPlayer.GetComponent<PlayerInventory>().SetInvItemList(inventory.GetInvItemList());
            spawnedPlayer.GetComponent<PlayerInventory>().SetInvHandItemList(inventory.GetInvHandItemList());
            spawnedPlayer.GetComponent<PlayerInventory>().SetInvItemArmorList(inventory.GetInvItemArmorList());
        }
    }

    #endregion SpawnerFunctions
}