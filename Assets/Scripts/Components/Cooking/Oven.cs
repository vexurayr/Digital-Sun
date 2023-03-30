using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oven : InventoryItem
{
    #region Variables
    [SerializeField] private float secondsToConvertItem;
    [SerializeField] private int sticksPerConvert;
    [SerializeField] private int woodPerConvert;
    [SerializeField] private InventoryItem emptyInvItem;
    [SerializeField] private InventoryItem cookedMeat;

    private InventoryItem fuelInput;
    private InventoryItem convertInput;
    private InventoryItem output;
    private GameObject player;
    private float timeRemaining;
    private bool hasItems = false;

    #endregion Variables

    #region MonoBehaviours
    public void Awake()
    {
        timeRemaining = secondsToConvertItem;

        fuelInput = emptyInvItem;
        convertInput = emptyInvItem;
        output = emptyInvItem;
    }

    public void Update()
    {
        // Nothing to convert or no fuel
        if (convertInput.GetItem() == Item.None || fuelInput.GetItem() == Item.None)
        {
            timeRemaining = secondsToConvertItem;
            return;
        }
        
        CountdownTimers();
    }

    #endregion MonoBehaviours

    #region GetSet
    public void SetOvenInventoryFromPlayer(GameObject player)
    {
        fuelInput = player.GetComponent<PlayerInventory>().GetOvenFuelInput();
        convertInput = player.GetComponent<PlayerInventory>().GetOvenConvertInput();
        output = player.GetComponent<PlayerInventory>().GetOvenOutput();

        CheckIfHasItems();
    }

    public void SetPlayerInventoryFromOven()
    {
        player.GetComponent<PlayerInventory>().SetOvenFuelInput(fuelInput);
        player.GetComponent<PlayerInventory>().SetOvenConvertInput(convertInput);
        player.GetComponent<PlayerInventory>().SetOvenOutput(output);

        CheckIfHasItems();
    }

    public InventoryItem GetFuelInput()
    {
        return fuelInput;
    }

    public void SetFuelInput(InventoryItem transferItem)
    {
        fuelInput = transferItem;
    }

    public InventoryItem GetConvertInput()
    {
        return convertInput;
    }

    public void SetConvertInput(InventoryItem transferItem)
    {
        convertInput = transferItem;
    }

    public InventoryItem GetOutput()
    {
        return output;
    }

    public void SetOutput(InventoryItem transferItem)
    {
        output = transferItem;
    }

    #endregion GetSet

    #region CookFunctions
    public void CountdownTimers()
    {
        // Don't run timer if there isn't enough of the specific fuel
        if (fuelInput.GetItem() == Item.Stick && fuelInput.GetItemCount() < sticksPerConvert)
        {
            Debug.Log("DONT GET HERE");
            timeRemaining = secondsToConvertItem;
            return;
        }
        else if (fuelInput.GetItem() == Item.Wood && fuelInput.GetItemCount() < woodPerConvert)
        {
            Debug.Log("DONT GET HERE");
            timeRemaining = secondsToConvertItem;
            return;
        }
        // Or if the output is at capacity
        else if (IsItemStackAtCapacity(output))
        {
            Debug.Log("DONT GET HERE");
            timeRemaining = secondsToConvertItem;
            return;
        }
        // Or if the item to be made is different than the item currently in the output
        else if (convertInput.GetItem() == Item.Uncooked_Meat && IsDifferentItemInOutput(Item.Cooked_Meat))
        {
            Debug.Log("DONT GET HERE");
            timeRemaining = secondsToConvertItem;
            return;
        }

        timeRemaining -= Time.deltaTime;

        timeRemaining = Mathf.Clamp(timeRemaining, 0f, secondsToConvertItem);

        if (timeRemaining <= 0)
        {
            ConvertItem();
            timeRemaining = secondsToConvertItem;
        }
    }

    public void ConvertItem()
    {
        // If the item is uncooked meat the output should be cooked meat
        if (convertInput.GetItem() == Item.Uncooked_Meat)
        {
            CookMeat();
        }
        else
        {
            return;
        }

        // Take away fuel
        if (fuelInput.GetItem() == Item.Stick)
        {
            fuelInput.SetItemCount(fuelInput.GetItemCount() - sticksPerConvert);
        }
        else if (fuelInput.GetItem() == Item.Wood)
        {
            fuelInput.SetItemCount(fuelInput.GetItemCount() - woodPerConvert);
        }

        // Remove the fuel if that was the last of the stack
        if (fuelInput.GetItemCount() <= 0)
        {
            fuelInput = emptyInvItem;
        }

        // Convert 1 item
        convertInput.SetItemCount(convertInput.GetItemCount() - 1);

        // Remove the item if that was the last of the stack
        if (convertInput.GetItemCount() <= 0)
        {
            convertInput = emptyInvItem;
        }

        SetPlayerInventoryFromOven();

        // Update the oven UI
        player.GetComponent<PlayerInventory>().RefreshOvenVisuals();
    }

    private void CookMeat()
    {
        // If output is currently empty
        if (output.GetItem() == Item.None)
        {
            output = cookedMeat;
            output.SetItemCount(1);
        }
        else
        {
            output.SetItemCount(output.GetItemCount() + 1);
        }
    }

    #endregion CookFunctions

    #region InventoryItemFunctions
    public override bool PrimaryAction(GameObject player)
    {
        this.player = player;
        // Update the player's Oven UI with whatever this oven is holding
        SetPlayerInventoryFromOven();
        return true;
    }

    public override void PickItemUp(Inventory targetInv)
    {
        if (player == null)
        {
            player = targetInv.gameObject;
        }

        // Don't let the player pick up the oven if there is anything in its inventory
        // The data will be lost because it isn't being given to the prefab when the oven is placed again
        if (!hasItems)
        {
            // Close the oven's inventory so the player
            if (player.GetComponent<PlayerController>().GetOvenUI().gameObject.activeInHierarchy)
            {
                player.GetComponent<PlayerController>().ToggleOvenUI();
            }

            base.PickItemUp(targetInv);
        }
    }

    #endregion InventoryItemFunctions

    #region HelperFunctions
    public void CheckIfHasItems()
    {
        if (fuelInput.GetItem() == Item.None && convertInput.GetItem() == Item.None && output.GetItem() == Item.None)
        {
            hasItems = false;
        }
        else
        {
            hasItems = true;
        }
    }

    public bool IsItemStackAtCapacity(InventoryItem item)
    {
        // Ignore this rule if the item in question is none, the count and max size are both 0
        if (item.GetItem() == Item.None)
        {
            return false;
        }

        if (item.GetItemCount() >= item.GetMaxStackSize())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDifferentItemInOutput(Item item)
    {
        if (output.GetItem() == Item.None)
        {
            return false;
        }
        else if (item == output.GetItem())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsDifferentItemInOutput(InventoryItem item)
    {
        if (output.GetItem() == Item.None)
        {
            return false;
        }
        else if (item.GetItem() == output.GetItem())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion HelperFunctions
}