using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance { get; private set; }

    [SerializeField] private PlayerController currentPlayerController;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 playerSpawn;

    [SerializeField] private PlayerInventory invToPreserve;

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

        invToPreserve = playerPrefab.GetComponent<PlayerInventory>();
        Debug.Log("Are you running every time?");
        SpawnPlayer(true);
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

    public PlayerInventory GetInvToPreserve()
    {
        return invToPreserve;
    }

    public void SetInvToPreserve(PlayerInventory newInv)
    {
        Debug.Log(newInv);
        playerPrefab.GetComponent<PlayerInventory>().SetInvItemList(newInv.GetInvItemList());
        playerPrefab.GetComponent<PlayerInventory>().SetInvHandItemList(newInv.GetInvHandItemList());
        playerPrefab.GetComponent<PlayerInventory>().SetInvItemArmorList(newInv.GetInvItemArmorList());
        
        invToPreserve = playerPrefab.GetComponent<PlayerInventory>();
    }

    #endregion GetSet

    #region SpawnerFunctions
    public void SpawnPlayer(bool isRespawningFromTerminal)
    {
        if (currentPlayerController != null)
        {
            return;
        }

        GameObject spawnedPlayer = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);

        if (isRespawningFromTerminal)
        {
            Debug.Log("Setting Inventory");
            spawnedPlayer.GetComponent<PlayerInventory>().SetInvItemList(invToPreserve.GetInvItemList());
            spawnedPlayer.GetComponent<PlayerInventory>().SetInvHandItemList(invToPreserve.GetInvHandItemList());
            spawnedPlayer.GetComponent<PlayerInventory>().SetInvItemArmorList(invToPreserve.GetInvItemArmorList());
        }
    }

    #endregion SpawnerFunctions
}