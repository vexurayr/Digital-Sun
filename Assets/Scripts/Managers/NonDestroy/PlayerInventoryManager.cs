using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    #region Variables
    public static PlayerInventoryManager instance { get; private set; }

    [SerializeField] private GameObject playerUI;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private SurvivalUI survivalUI;
    [SerializeField] private GameObject basicCraftingUI;

    private PlayerInventory playerInventory;
    private OvenUI ovenUI;
    private Text worldToolTipUI;
    private Text inventoryToolTipUI;
    private CraftBench lastOpenedCraftBench;
    private Oven lastOpenedOven;

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

        playerInventory = GetComponent<PlayerInventory>();

        ovenUI = playerInventory.GetOvenUI();
        worldToolTipUI = playerInventory.GetWorldToolTipUI();
        inventoryToolTipUI = playerInventory.GetInventoryToolTipUI();
    }

    #endregion MonoBehaviours

    #region GetSet
    public PlayerInventory GetPlayerInventory()
    {
        return playerInventory;
    }

    public void SetInventoryUI(InventoryUI inventoryUI)
    {
        playerInventory.SetInventoryUI(inventoryUI);
    }

    public GameObject GetPlayerUI()
    {
        return playerUI;
    }

    public void SetOvenUI(OvenUI ovenUI)
    {
        playerInventory.SetOvenUI(ovenUI);
    }

    #endregion GetSet

    #region InventoryFunctions
    public void ResetInventory()
    {
        playerInventory.InitializeInventory();
    }

    public void CheckArmorOnSceneLoad()
    {
        StartCoroutine(CheckArmor());
    }

    private IEnumerator CheckArmor()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < playerInventory.GetInvItemArmorList().Count; i++)
        {
            if (playerInventory.GetInvItemArmorList()[i].GetItemType() == InventoryItem.ItemType.Helmet)
            {
                playerInventory.GetInvItemArmorList()[i].PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
            else if (playerInventory.GetInvItemArmorList()[i].GetItemType() == InventoryItem.ItemType.Chestplate)
            {
                playerInventory.GetInvItemArmorList()[i].PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
            else if (playerInventory.GetInvItemArmorList()[i].GetItemType() == InventoryItem.ItemType.Leggings)
            {
                playerInventory.GetInvItemArmorList()[i].PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
        }
    }

    public void ToggleInventoryUI(bool newState)
    {
        if (newState)
        {
            foreach (GameObject obj in inventoryUI.GetInvSlotsUI())
            {
                obj.gameObject.SetActive(true);
            }
            foreach (GameObject obj in inventoryUI.GetInvItemsUI())
            {
                obj.gameObject.SetActive(true);
            }
            foreach (GameObject obj in inventoryUI.GetInvArmorSlotsUI())
            {
                obj.gameObject.SetActive(true);
            }
            foreach (GameObject obj in inventoryUI.GetInvArmorItemsUI())
            {
                obj.gameObject.SetActive(true);
            }

            inventoryUI.GetInvItemDiscardUI().SetActive(true);
            survivalUI.gameObject.SetActive(true);
            basicCraftingUI.SetActive(true);
        }
        else
        {
            inventoryToolTipUI.text = "";

            foreach (GameObject obj in inventoryUI.GetInvSlotsUI())
            {
                obj.gameObject.SetActive(false);
            }
            foreach (GameObject obj in inventoryUI.GetInvItemsUI())
            {
                obj.gameObject.SetActive(false);
            }
            foreach (GameObject obj in inventoryUI.GetInvArmorSlotsUI())
            {
                obj.gameObject.SetActive(false);
            }
            foreach (GameObject obj in inventoryUI.GetInvArmorItemsUI())
            {
                obj.gameObject.SetActive(false);
            }

            inventoryUI.GetInvItemDiscardUI().SetActive(false);
            survivalUI.gameObject.SetActive(false);
            basicCraftingUI.SetActive(false);

            if (GameManager.instance.GetCurrentPlayerController())
            {
                lastOpenedCraftBench = GameManager.instance.GetCurrentPlayerController().GetLastOpenedCraftBench();
            }

            if (lastOpenedCraftBench != null)
            {
                if (lastOpenedCraftBench.GetCraftBenchUI().activeInHierarchy)
                {
                    lastOpenedCraftBench.GetCraftBenchUI().SetActive(false);
                }
            }
            if (ovenUI.gameObject.activeInHierarchy)
            {
                ToggleOvenUI();
            }
        }
    }

    public void ToggleOvenUI()
    {
        if (ovenUI.isActiveAndEnabled)
        {
            ovenUI.gameObject.SetActive(false);
            ovenUI.GetFuelInputSlot().SetActive(false);
            ovenUI.GetFuelInputItem().SetActive(false);
            ovenUI.GetConvertInputSlot().SetActive(false);
            ovenUI.GetConvertInputItem().SetActive(false);
            ovenUI.GetOutputSlot().SetActive(false);
            ovenUI.GetOutputItem().SetActive(false);
        }
        else
        {
            ovenUI.gameObject.SetActive(true);
            ovenUI.GetFuelInputSlot().SetActive(true);
            ovenUI.GetFuelInputItem().SetActive(true);
            ovenUI.GetConvertInputSlot().SetActive(true);
            ovenUI.GetConvertInputItem().SetActive(true);
            ovenUI.GetOutputSlot().SetActive(true);
            ovenUI.GetOutputItem().SetActive(true);
        }
    }

    #endregion InventoryFunctions
}