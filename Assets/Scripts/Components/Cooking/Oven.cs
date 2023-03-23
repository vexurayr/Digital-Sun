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
    private PlayerInventory player;
    private bool isHoldingItems = false;
    private float timeRemaining;

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
        // Nothing to convert
        if (convertInput.GetItem() == Item.None)
        {
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

    #endregion MonoBehaviours

    #region GetSet
    public void SetOvenInventoryFromPlayer(GameObject player)
    {
        fuelInput = player.GetComponent<PlayerInventory>().GetOvenFuelInput();
        convertInput = player.GetComponent<PlayerInventory>().GetOvenConvertInput();
        output = player.GetComponent<PlayerInventory>().GetOvenOutput();
    }

    public void SetPlayerInventoryFromOven(GameObject player)
    {
        player.GetComponent<PlayerInventory>().SetOvenFuelInput(fuelInput);
        player.GetComponent<PlayerInventory>().SetOvenConvertInput(convertInput);
        player.GetComponent<PlayerInventory>().SetOvenOutput(output);
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

    #region RefreshInventoryVisuals
    public void RefreshInventoryVisuals()
    {
        isHoldingItems = false;

        // Move UI inventory items to the correct location
        fuelInputItem.transform.position = fuelInputSlot.transform.position;
        convertInputItem.transform.position = convertInputSlot.transform.position;
        outputItem.transform.position = outputSlot.transform.position;

        // Set correct sprites and text for Fuel Input stack size and adjust IndexValue
        if (fuelInput.GetItemSprite() == null)
        {
            fuelInputItem.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            fuelInputItem.GetComponent<RawImage>().texture = null;
        }
        else
        {
            fuelInputItem.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            fuelInputItem.GetComponent<RawImage>().texture = fuelInput.GetItemSprite().texture;
            isHoldingItems = true;
        }

        if (fuelInput.GetItemCount() > 1)
        {
            fuelInputCounter.GetComponent<Text>().text = fuelInput.GetItemCount().ToString();
        }
        else
        {
            fuelInputCounter.GetComponent<Text>().text = "";
        }

        // For Convert Input
        if (convertInput.GetItemSprite() == null)
        {
            convertInputItem.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            convertInputItem.GetComponent<RawImage>().texture = null;
        }
        else
        {
            convertInputItem.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            convertInputItem.GetComponent<RawImage>().texture = convertInput.GetItemSprite().texture;
            isHoldingItems = true;
        }

        if (convertInput.GetItemCount() > 1)
        {
            convertInputCounter.GetComponent<Text>().text = convertInput.GetItemCount().ToString();
        }
        else
        {
            convertInputCounter.GetComponent<Text>().text = "";
        }

        // For Output
        if (output.GetItemSprite() == null)
        {
            outputItem.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            outputItem.GetComponent<RawImage>().texture = null;
        }
        else
        {
            outputItem.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            outputItem.GetComponent<RawImage>().texture = output.GetItemSprite().texture;
            isHoldingItems = true;
        }

        if (output.GetItemCount() > 1)
        {
            outputCounter.GetComponent<Text>().text = output.GetItemCount().ToString();
        }
        else
        {
            outputCounter.GetComponent<Text>().text = "";
        }
    }

    #endregion RefreshInventoryVisuals

    #region SwapInvItems
    // Called whenever the player adjusts Inventory through the UI
    // index = 0 -> fuelInput, index = 1 -> convertInput, index = 2 -> output
    public void SwapItems(InventoryItem newOvenItem, int index)
    {
        if (index == 0)
        {
            SwapPlayerItemWithFuelInputItem(newOvenItem);
        }
        else if (index == 1)
        {
            SwapPlayerItemWithConvertInputItem(newOvenItem);
        }
        else if (index == 2)
        {
            SwapPlayerItemWithOutputItem(newOvenItem);
        }
    }

    public InventoryItem SwapPlayerItemWithFuelInputItem(InventoryItem newFuelInput)
    {
        // Only let the player add fuel to this input
        if (newFuelInput.gameObject.GetComponent<Classify>().GetClassification() != Classify.Classification.IsFuelSource)
        {
            return emptyInvItem;
        }

        InventoryItem oldFuelInput = fuelInput;
        fuelInput = newFuelInput;

        RefreshInventoryVisuals();

        return oldFuelInput;
    }

    public InventoryItem SwapPlayerItemWithConvertInputItem(InventoryItem newConvertInput)
    {
        // Only let the player add items that are meant to go into the oven
        if (newConvertInput.gameObject.GetComponent<Classify>().GetClassification() != Classify.Classification.IsConvertable)
        {
            return emptyInvItem;
        }

        InventoryItem oldConvertInput = convertInput;
        convertInput = newConvertInput;

        RefreshInventoryVisuals();

        return oldConvertInput;
    }

    public InventoryItem SwapPlayerItemWithOutputItem(InventoryItem newOutput)
    {
        // Only let the player remove items from output
        if (newOutput.GetItem() != Item.None)
        {
            return emptyInvItem;
        }

        InventoryItem oldOutput = output;
        output = newOutput;

        RefreshInventoryVisuals();

        return oldOutput;
    }

    #endregion SwapInvItems

    #region CookFunctions
    public void ConvertItem()
    {
        if (convertInput.GetItem() == Item.Uncooked_Meat)
        {
            CookMeat();
        }
        else
        {
            return;
        }
    }

    private void CookMeat()
    {
        // Bail if the oven's output contains a different non-empty item
        // or the output is at capacity
        if (output.GetItem() != Item.Cooked_Meat && output.GetItem() != Item.None && output.GetItemCount() < output.GetMaxStackSize())
        {
            return;
        }

        // Output doesn't contain any cooked meat
        if (output.GetItem() == Item.None)
        {
            output = cookedMeat;
            output.SetItemCount(1);

            convertInput.SetItemCount(convertInput.GetItemCount() - 1);
        }
        else
        {
            output.SetItemCount(output.GetItemCount() + 1);

            convertInput.SetItemCount(convertInput.GetItemCount() - 1);
        }

        if (convertInput.GetItemCount() <= 0)
        {
            convertInput = emptyInvItem;
        }
    }

    #endregion CookFunctions

    #region InventoryItemFunctions
    public override bool PrimaryAction(GameObject player)
    {
        // Update the player's Oven UI with whatever this oven is holding
        SetPlayerInventoryFromOven(player);
        return true;
    }

    public override void PickItemUp(Inventory targetInv)
    {
        if (!isHoldingItems)
        {
            base.PickItemUp(targetInv);
        }
    }

    #endregion InventoryItemFunctions
}