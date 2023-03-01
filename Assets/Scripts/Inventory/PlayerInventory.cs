using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    #region Variables
    [SerializeField] private InventoryUI inventoryUI;

    private List<GameObject> invSlotsUI;
    private List<GameObject> invItemsUI;
    private List<GameObject> invItemCountersUI;
    private List<GameObject> invHandSlotsUI;
    private List<GameObject> invHandItemsUI;
    private List<GameObject> invHandItemCountersUI;

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

        RefreshInventoryVisuals();
    }

    #endregion MonoBehaviours

    #region GetSet
    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }

    public int GetSelectedInvHandSlot()
    {
        return selectedInvHandSlot;
    }

    public void SetSelectedInvHandSlot(int slotIndex)
    {
        selectedInvHandSlot = slotIndex;
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
    }

    #endregion RefreshInventoryVisuals

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

        RefreshInventoryVisuals();
    }

    public void SwapInvItemWithHandItem(int firstFromInv, int secondFromHandInv)
    {
        InventoryItem invItem = invItemList[firstFromInv];
        InventoryItem invHandItem = invHandItemList[secondFromHandInv];

        invItemList[firstFromInv] = invHandItem;
        invHandItemList[secondFromHandInv] = invItem;

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
            RemoveFromInventory(secondInvItemIndex, isSecondInvHandItem);

            return true;
        }
    }

    #endregion CombineStacks

    #region SelectHotbarSlot
    public void MoveSelectedInvHandSlotRight()
    {
        selectedInvHandSlot++;

        if (selectedInvHandSlot > invHandSlotsUI.Count - 1)
        {
            selectedInvHandSlot = 0;
        }
    }

    public void MoveSelectedInvHandSlotLeft()
    {
        selectedInvHandSlot--;

        if (selectedInvHandSlot < 0)
        {
            selectedInvHandSlot = invHandSlotsUI.Count - 1;
        }
    }

    #endregion SelectHotbarSlot
}