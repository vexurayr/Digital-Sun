using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    #region Variables
    [SerializeField] private Text worldToolTipUI;
    [SerializeField] private Text inventoryToolTipUI;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private OvenUI ovenUI;

    private List<GameObject> invSlotsUI;
    private List<GameObject> invItemsUI;
    private List<GameObject> invItemCountersUI;
    private List<GameObject> invHandSlotsUI;
    private List<GameObject> invHandItemsUI;
    private List<GameObject> invHandItemCountersUI;
    private List<GameObject> invArmorSlotsUI;
    private List<GameObject> invArmorItemsUI;
    private List<GameObject> invArmorItemCountersUI;

    private GameObject fuelInputSlotUI;
    private GameObject convertInputSlotUI;
    private GameObject outputSlotUI;
    private GameObject fuelInputItemUI;
    private GameObject convertInputItemUI;
    private GameObject outputItemUI;
    private GameObject fuelInputCounterUI;
    private GameObject convertInputCounterUI;
    private GameObject outputCounterUI;

    private int selectedInvHandSlot = 0;

    #endregion Variables

    #region MonoBehaviours
    public override void Awake()
    {
        base.Awake();

        invSlotsUI = inventoryUI.GetInvSlotsUI();
        invItemsUI = inventoryUI.GetInvItemsUI();
        invItemCountersUI = inventoryUI.GetInvItemCountersUI();
        invHandSlotsUI = inventoryUI.GetInvHandSlotsUI();
        invHandItemsUI = inventoryUI.GetInvHandItemsUI();
        invHandItemCountersUI = inventoryUI.GetInvHandItemCountersUI();
        invArmorSlotsUI = inventoryUI.GetInvArmorSlotsUI();
        invArmorItemsUI = inventoryUI.GetInvArmorItemsUI();
        invArmorItemCountersUI = inventoryUI.GetInvArmorItemCountersUI();

        fuelInputSlotUI = ovenUI.GetFuelInputSlot();
        convertInputSlotUI = ovenUI.GetConvertInputSlot();
        outputSlotUI = ovenUI.GetOutputSlot();
        fuelInputItemUI = ovenUI.GetFuelInputItem();
        convertInputItemUI = ovenUI.GetConvertInputItem();
        outputItemUI = ovenUI.GetOutputItem();
        fuelInputCounterUI = ovenUI.GetFuelInputCounter();
        convertInputCounterUI = ovenUI.GetConvertInputCounter();
        outputCounterUI = ovenUI.GetOutputCounter();

        RefreshInventoryVisuals();
    }

    #endregion MonoBehaviours

    #region GetSet
    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }

    public void SetInventoryUI(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
    }

    public int GetSelectedInvHandSlot()
    {
        return selectedInvHandSlot;
    }

    public void SetSelectedInvHandSlot(int slotIndex)
    {
        selectedInvHandSlot = slotIndex;
    }

    public OvenUI GetOvenUI()
    {
        return ovenUI;
    }

    public void SetOvenUI(OvenUI ovenUI)
    {
        this.ovenUI = ovenUI;
    }

    public GameObject GetActivePlayerItem()
    {
        return GameObject.Find("ActivePlayerItem");
    }

    public Text GetWorldToolTipUI()
    {
        return worldToolTipUI;
    }

    public Text GetInventoryToolTipUI()
    {
        return inventoryToolTipUI;
    }

    #endregion GetSet

    #region RefreshInventoryVisuals
    public void RefreshInventoryVisuals()
    {
        // Move UI inventory items to the correct location
        for (int i = 0; i < invItemList.Count; i++)
        {
            invItemsUI[i].transform.position = invSlotsUI[i].transform.position;
        }

        for (int i = 0; i < invHandItemList.Count; i++)
        {
            invHandItemsUI[i].transform.position = invHandSlotsUI[i].transform.position;
        }

        for (int i = 0; i < invItemArmorList.Count; i++)
        {
            invArmorItemsUI[i].transform.position = invArmorSlotsUI[i].transform.position;
        }

        // Set correct sprites and text for stack size and adjust IndexValue
        for (int i = 0; i < invItemList.Count; i++)
        {
            if (invItemList[i].GetItemSprite() == null)
            {
                invItemsUI[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
                invItemsUI[i].GetComponent<RawImage>().texture = null;
            }
            else
            {
                invItemsUI[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                invItemsUI[i].GetComponent<RawImage>().texture = invItemList[i].GetItemSprite().texture;
            }

            if (invItemList[i].GetItemCount() > 1)
            {
                invItemCountersUI[i].GetComponent<Text>().text = invItemList[i].GetItemCount().ToString();
            }
            else
            {
                invItemCountersUI[i].GetComponent<Text>().text = "";
            }

            invItemsUI[i].GetComponent<IndexValue>().SetIndexValue(i);
        }

        for (int i = 0; i < invHandItemList.Count; i++)
        {
            if (invHandItemList[i].GetItemSprite() == null)
            {
                invHandItemsUI[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
                invHandItemsUI[i].GetComponent<RawImage>().texture = null;
            }
            else
            {
                invHandItemsUI[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                invHandItemsUI[i].GetComponent<RawImage>().texture = invHandItemList[i].GetItemSprite().texture;
            }

            if (invHandItemList[i].GetItemCount() > 1)
            {
                invHandItemCountersUI[i].GetComponent<Text>().text = invHandItemList[i].GetItemCount().ToString();
            }
            else
            {
                invHandItemCountersUI[i].GetComponent<Text>().text = "";
            }

            invHandItemsUI[i].GetComponent<IndexValue>().SetIndexValue(i);
        }

        for (int i = 0; i < invItemArmorList.Count; i++)
        {
            if (invItemArmorList[i].GetItemSprite() == null)
            {
                invArmorItemsUI[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
                invArmorItemsUI[i].GetComponent<RawImage>().texture = null;
            }
            else
            {
                invArmorItemsUI[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                invArmorItemsUI[i].GetComponent<RawImage>().texture = invItemArmorList[i].GetItemSprite().texture;
            }

            if (invItemArmorList[i].GetItemCount() > 1)
            {
                invArmorItemCountersUI[i].GetComponent<Text>().text = invItemArmorList[i].GetItemCount().ToString();
            }
            else
            {
                invArmorItemCountersUI[i].GetComponent<Text>().text = "";
            }

            invArmorItemsUI[i].GetComponent<IndexValue>().SetIndexValue(i);
        }

        RefreshOvenVisuals();
    }

    #endregion RefreshInventoryVisuals

    #region RefreshOvenVisuals
    public void RefreshOvenVisuals()
    {
        // Move UI inventory items to the correct location
        fuelInputItemUI.transform.position = fuelInputSlotUI.transform.position;
        convertInputItemUI.transform.position = convertInputSlotUI.transform.position;
        outputItemUI.transform.position = outputSlotUI.transform.position;

        // Set correct sprites and text for Fuel Input
        if (ovenFuelInput.GetItemSprite() == null)
        {
            fuelInputItemUI.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            fuelInputItemUI.GetComponent<RawImage>().texture = null;
        }
        else
        {
            fuelInputItemUI.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            fuelInputItemUI.GetComponent<RawImage>().texture = ovenFuelInput.GetItemSprite().texture;
        }

        if (ovenFuelInput.GetItemCount() > 1)
        {
            fuelInputCounterUI.GetComponent<Text>().text = ovenFuelInput.GetItemCount().ToString();
        }
        else
        {
            fuelInputCounterUI.GetComponent<Text>().text = "";
        }

        // For Convert Input
        if (ovenConvertInput.GetItemSprite() == null)
        {
            convertInputItemUI.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            convertInputItemUI.GetComponent<RawImage>().texture = null;
        }
        else
        {
            convertInputItemUI.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            convertInputItemUI.GetComponent<RawImage>().texture = ovenConvertInput.GetItemSprite().texture;
        }

        if (ovenConvertInput.GetItemCount() > 1)
        {
            convertInputCounterUI.GetComponent<Text>().text = ovenConvertInput.GetItemCount().ToString();
        }
        else
        {
            convertInputCounterUI.GetComponent<Text>().text = "";
        }

        // For Output
        if (ovenOutput.GetItemSprite() == null)
        {
            outputItemUI.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            outputItemUI.GetComponent<RawImage>().texture = null;
        }
        else
        {
            outputItemUI.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            outputItemUI.GetComponent<RawImage>().texture = ovenOutput.GetItemSprite().texture;
        }

        if (ovenOutput.GetItemCount() > 1)
        {
            outputCounterUI.GetComponent<Text>().text = ovenOutput.GetItemCount().ToString();
        }
        else
        {
            outputCounterUI.GetComponent<Text>().text = "";
        }
    }

    #endregion RefreshOvenVisuals

    #region SwapInvItems
    // Called whenever the player adjusts Inventory through the UI
    public void SwapTwoInvItems(int first, int second)
    {
        // Swap Inventory Items in Inventory
        InventoryItem firstInvItem = invItemList[first];
        InventoryItem secondInvItem = invItemList[second];
        
        invItemList[first] = secondInvItem;
        invItemList[second] = firstInvItem;
        
        RefreshInventoryVisuals();
    }

    public void SwapTwoInvHandItems(int first, int second)
    {
        InventoryItem firstInvHandItem = invHandItemList[first];
        InventoryItem secondInvHandItem = invHandItemList[second];

        invHandItemList[first] = secondInvHandItem;
        invHandItemList[second] = firstInvHandItem;

        // Update the scene with the player's currently held object
        CreateItemInHand(invHandItemList[selectedInvHandSlot]);

        RefreshInventoryVisuals();
    }

    public void SwapInvItemWithHandItem(int firstFromInv, int secondFromHandInv)
    {
        InventoryItem invItem = invItemList[firstFromInv];
        InventoryItem invHandItem = invHandItemList[secondFromHandInv];

        invItemList[firstFromInv] = invHandItem;
        invHandItemList[secondFromHandInv] = invItem;

        // Update the scene with the player's currently held object
        CreateItemInHand(invHandItemList[selectedInvHandSlot]);

        RefreshInventoryVisuals();
    }

    // Index of 0 = helmet, Index of 1 = chestplate, Index of 2 = leggings
    public void SwapArmorItemWithInvItem(int firstFromArmorInv, int secondFromInv)
    {
        // The invItem in this case is the armor being equipped
        InventoryItem invArmorItem = invItemArmorList[firstFromArmorInv];
        InventoryItem invItem = invItemList[secondFromInv];

        bool isInvArmorItemEmpty = false;
        bool isInvItemEmpty = false;

        if (invArmorItem.GetItemType() == InventoryItem.ItemType.Empty)
        {
            isInvArmorItemEmpty = true;
        }
        if (invItem.GetItemType() == InventoryItem.ItemType.Empty)
        {
            isInvItemEmpty = true;
        }

        if (isInvArmorItemEmpty && isInvItemEmpty)
        {
            return;
        }
        // Attempting to equip armor in empty armor slot
        else if (isInvArmorItemEmpty && !isInvItemEmpty)
        {
            // Only equip armor if the type matches the kind of armor the slot is meant to store
            if (invItem.GetItemType() == InventoryItem.ItemType.Helmet && firstFromArmorInv == 0)
            {
                invItemArmorList[firstFromArmorInv] = invItem;
                invItemList[secondFromInv] = invArmorItem;

                invItem.PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
            else if (invItem.GetItemType() == InventoryItem.ItemType.Chestplate && firstFromArmorInv == 1)
            {
                invItemArmorList[firstFromArmorInv] = invItem;
                invItemList[secondFromInv] = invArmorItem;

                invItem.PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
            else if (invItem.GetItemType() == InventoryItem.ItemType.Leggings && firstFromArmorInv == 2)
            {
                invItemArmorList[firstFromArmorInv] = invItem;
                invItemList[secondFromInv] = invArmorItem;

                invItem.PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
        }
        // Attempting to remove armor and place in an empty slot
        else if (!isInvArmorItemEmpty && isInvItemEmpty)
        {
            invItemArmorList[firstFromArmorInv] = invItem;
            invItemList[secondFromInv] = invArmorItem;

            invArmorItem.SecondaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
        }
        // Attempting to remove armor and add something else to the armor slot
        else
        {
            // Replace the helmet with a different helmet
            if (invItem.GetItemType() == InventoryItem.ItemType.Helmet && firstFromArmorInv == 0)
            {
                invItemArmorList[firstFromArmorInv] = invItem;
                invItemList[secondFromInv] = invArmorItem;

                invArmorItem.SecondaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
                invItem.PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
            // Replace chestplate with different chestplate
            else if (invItem.GetItemType() == InventoryItem.ItemType.Chestplate && firstFromArmorInv == 1)
            {
                invItemArmorList[firstFromArmorInv] = invItem;
                invItemList[secondFromInv] = invArmorItem;

                invArmorItem.SecondaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
                invItem.PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
            // Replace leggings with different leggings
            else if (invItem.GetItemType() == InventoryItem.ItemType.Leggings && firstFromArmorInv == 2)
            {
                invItemArmorList[firstFromArmorInv] = invItem;
                invItemList[secondFromInv] = invArmorItem;

                invArmorItem.SecondaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
                invItem.PrimaryAction(GameManager.instance.GetCurrentPlayerController().gameObject);
            }
        }

        RefreshInventoryVisuals();
    }

    #endregion SwapInvItems

    #region CombineStacks
    // These will always be of the same item
    public bool CombineStacks(InventoryItem firstInvItem, InventoryItem secondInvItem, int secondInvItemIndex, bool isSecondInvHandItem)
    {
        int firstInvItemCount = firstInvItem.GetItemCount();
        int secondInvItemCount = secondInvItem.GetItemCount();
        int maxItemCount = firstInvItem.GetMaxStackSize();
        int newAmount = firstInvItemCount + secondInvItemCount;

        // Can't combine stacks if 1 or both are already at capacity
        if (firstInvItemCount == maxItemCount || secondInvItemCount == maxItemCount)
        {
            return false;
        }
        // Adding this will exceed max stack size, will give any remainder back to second inv item
        else if (newAmount > maxItemCount)
        {
            secondInvItemCount = newAmount - maxItemCount;

            firstInvItem.SetItemCount(maxItemCount);
            secondInvItem.SetItemCount(secondInvItemCount);
            return true;
        }
        // Stacks can combine fully into 1 stack
        else
        {
            firstInvItem.SetItemCount(newAmount);

            // Remove second item from the player's inventory
            RemoveItemFromInventory(secondInvItemIndex, isSecondInvHandItem, false);

            return true;
        }
    }

    #endregion CombineStacks

    #region SelectHotbarSlot
    public void MoveSelectedInvHandSlotRight()
    {
        AudioManager.instance.PlaySound2D("Swap Hand Item");

        selectedInvHandSlot++;

        if (selectedInvHandSlot > invHandSlotsUI.Count - 1)
        {
            selectedInvHandSlot = 0;
        }
    }

    public void MoveSelectedInvHandSlotLeft()
    {
        AudioManager.instance.PlaySound2D("Swap Hand Item");

        selectedInvHandSlot--;

        if (selectedInvHandSlot < 0)
        {
            selectedInvHandSlot = invHandSlotsUI.Count - 1;
        }
    }

    #endregion SelectHotbarSlot

    #region ChangeInvHandItemInScene
    public void CreateHandItemOnPlayerSpawn()
    {
        // Update the scene with the player's currently held object
        CreateItemInHand(invHandItemList[selectedInvHandSlot]);
    }

    public void CreateItemInHand(InventoryItem newItem)
    {
        // There will only be one object in the scene with this name
        Destroy(GameObject.Find("ActivePlayerItem"));

        // Get the prefab of the new item that will be in the scene
        GameObject itemPrefab = InvItemManager.instance.GetPrefabForInvItem(newItem);

        // Instantiate and make this new item a child of the player's camera
        GameObject itemInScene = Instantiate(itemPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        itemInScene.transform.parent = GameManager.instance.GetCurrentPlayerController().GetPlayerCamera().transform;

        // Adjust the item so it appears to be held
        itemInScene.gameObject.transform.localPosition = newItem.GetTransformInHand();
        itemInScene.gameObject.transform.localEulerAngles = newItem.GetRotationInHand();
        itemInScene.GetComponent<InventoryItem>().SetItemCount(newItem.GetItemCount());
        // Make sure this item can't be grabbed to duplicate it
        itemInScene.GetComponent<InventoryItem>().SetIsGrabbable(false);

        // Give the item in the scene the unique name
        itemInScene.gameObject.name = "ActivePlayerItem";

        // Special cases for data transfer
        if (itemInScene.GetComponent<Canteen>())
        {
            itemInScene.GetComponent<Canteen>().SetChargesStored(newItem.GetChargesStored());
        }
    }

    #endregion ChangeInvHandItemInScene

    #region OvenFunctions
    public InventoryItem GetOvenFuelInput()
    {
        return ovenFuelInput;
    }

    public void SetOvenFuelInput(InventoryItem newFuelInput)
    {
        ovenFuelInput = newFuelInput;
    }

    public InventoryItem GetOvenConvertInput()
    {
        return ovenConvertInput;
    }

    public void SetOvenConvertInput(InventoryItem newConvertInput)
    {
        ovenConvertInput = newConvertInput;
    }

    public InventoryItem GetOvenOutput()
    {
        return ovenOutput;
    }

    public void SetOvenOutput(InventoryItem newOutput)
    {
        ovenOutput = newOutput;
    }

    public void SwapInvItemWithOvenItem(int invItemIndex, InventoryItem ovenItem,
        bool isOvenFuelInput, bool isOvenConvertInput, bool isOvenOutput)
    {
        // Swap Inventory Items
        InventoryItem firstInvItem = invItemList[invItemIndex];

        invItemList[invItemIndex] = ovenItem;

        if (isOvenFuelInput)
        {
            ovenFuelInput = firstInvItem;
        }
        else if (isOvenConvertInput)
        {
            ovenConvertInput = firstInvItem;
        }
        else if (isOvenOutput)
        {
            ovenOutput = firstInvItem;
        }

        Oven lastOpenedOven = GameManager.instance.GetCurrentPlayerController().GetLastOpenedOven();

        lastOpenedOven.SetOvenInventoryFromPlayer(gameObject);
        
        RefreshInventoryVisuals();
    }

    public void SwapInvHandItemWithOvenItem(int invItemIndex, InventoryItem ovenItem,
        bool isOvenFuelInput, bool isOvenConvertInput, bool isOvenOutput)
    {
        // Swap Inventory Items
        InventoryItem firstInvItem = invHandItemList[invItemIndex];
        Debug.Log(firstInvItem.GetItem() + ", " + ovenItem.GetItem());

        invHandItemList[invItemIndex] = ovenItem;
        
        if (isOvenFuelInput)
        {
            ovenFuelInput = firstInvItem;
        }
        else if (isOvenConvertInput)
        {
            ovenConvertInput = firstInvItem;
        }
        else if (isOvenOutput)
        {
            ovenOutput = firstInvItem;
        }

        Oven lastOpenedOven = gameObject.GetComponent<PlayerController>().GetLastOpenedOven();

        lastOpenedOven.SetOvenInventoryFromPlayer(gameObject);

        RefreshInventoryVisuals();
    }

    #endregion OvenFunctions
}