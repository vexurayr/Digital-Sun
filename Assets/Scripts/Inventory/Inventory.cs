using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected int invSlots;
    [SerializeField] protected int invHandSlots;
    [SerializeField] protected InventoryItem emptyInvItem;

    protected List<InventoryItem> invItemList;
    protected List<InventoryItem> invHandItemList;

    public virtual void Awake()
    {
        invItemList = new List<InventoryItem>();
        invHandItemList = new List<InventoryItem>();

        InitializeInventory();
    }

    public virtual void InitializeInventory()
    {
        invItemList.Clear();
        invHandItemList.Clear();

        for (int i = 0; i < invSlots; i++)
        {
            invItemList.Add(emptyInvItem);
        }

        for (int i = 0; i < invHandSlots; i++)
        {
            invHandItemList.Add(emptyInvItem);
        }
    }

    public virtual void AddToInventory(InventoryItem newItem)
    {
        if (newItem == null)
        {
            return;
        }

        if (newItem.GetItemType() == InventoryItem.ItemType.Resource)
        {
            // Check if incoming item can be added to an existing stack
            for (int i = 0; i < invItemList.Count; i++)
            {
                // Determine which resource is in this inventory slot
                if (invItemList[i].GetItem() == InventoryItem.Item.Wood)
                {
                    int invMaxSize = invItemList[i].GetMaxStackSize();
                    int invCurrenSize = invItemList[i].GetItemCount();
                    int amountToAdd = newItem.GetItemCount();
                    int newAmount = invCurrenSize + amountToAdd;

                    // Skip slots that are already at capacity
                    if (invCurrenSize == invMaxSize)
                    {}
                    // Adding this will exceed max stack size, will add remainder to different stack or slot
                    else if (newAmount > invMaxSize)
                    {
                        int remainder = newAmount - invMaxSize;
                        invItemList[i].SetItemCount(invMaxSize);
                        newItem.SetItemCount(remainder);
                    }
                    // Whole stack can fit in this slot
                    else
                    {
                        invItemList[i].SetItemCount(newAmount);
                        return;
                    }
                }
            }
        }
        else if (newItem.GetItemType() == InventoryItem.ItemType.Consumable)
        {
            for (int i = 0; i < invItemList.Count; i++)
            {

            }
        }

        // Place object in first empty inventory slot
        for (int i = 0; i < invItemList.Count; i++)
        {
            if (invItemList[i].GetItemType() == InventoryItem.ItemType.Empty)
            {
                invItemList[i] = newItem;
                // Break out of the loop
                i = invItemList.Count;
            }
        }
    }

    public virtual void AddToInventory(GameObject newObj)
    {
        InventoryItem newItem = newObj.GetComponent<InventoryItem>();
        AddToInventory(newItem);
    }

    public virtual void RemoveFromInventory(int itemAtIndexToRemove, bool isInvHandItem)
    {
        if (itemAtIndexToRemove < 0 || itemAtIndexToRemove > invItemList.Count)
        {
            return;
        }

        if (!isInvHandItem)
        {
            invItemList[itemAtIndexToRemove] = emptyInvItem;
        }
        else
        {
            invHandItemList[itemAtIndexToRemove] = emptyInvItem;
        }
    }

    // Unavoidable problem with this function
    // If player has two max stacks of wood, attempting to remove the second will always remove the first
    // Use RemoveFromInventory(int itemAtIndexToRemove, bool isInvHandItem) when possible
    public virtual void RemoveFromInventory(InventoryItem itemToRemove, bool isInvHandItem)
    {
        if (itemToRemove.GetItem() == InventoryItem.Item.None)
        {
            return;
        }

        for (int i = 0; i < invItemList.Count; i++)
        {
            // Replace object from the inventory slot it is found in with an empty object
            if (invItemList[i].GetItem() == itemToRemove.GetItem() && invItemList[i].GetItemCount() == itemToRemove.GetItemCount())
            {
                if (!isInvHandItem)
                {
                    invItemList[i] = emptyInvItem;
                }
                else
                {
                    invHandItemList[i] = emptyInvItem;
                }
                
                i = invItemList.Count;
            }
        }
    }

    public virtual void RemoveFromInventory(GameObject objToRemove)
    {
        InventoryItem itemToRemove = objToRemove.GetComponent<InventoryItem>();
        bool isInvHandItem;

        if (objToRemove.GetComponent<IsInvHandItem>())
        {
            isInvHandItem = true;
        }
        else
        {
            isInvHandItem = false;
        }

        RemoveFromInventory(itemToRemove, isInvHandItem);
    }

    public virtual void DisplayInventoryDebug()
    {
        string invItems = "";
        string invHandItems = "";

        foreach (InventoryItem item in invItemList)
        {
            invItems = invItems + ", " + item.GetItem();
        }

        foreach (InventoryItem item in invHandItemList)
        {
            invHandItems = invHandItems + ", " + item.GetItem();
        }

        Debug.Log("Inventory: " + invItems + "\nHotbar: " + invHandItems);
    }

    public virtual int GetInvSlotCount()
    {
        return invSlots;
    }

    public virtual int GetInvHandSlotCount()
    {
        return invHandSlots;
    }

    public virtual List<InventoryItem> GetInvItemList()
    {
        return invItemList;
    }

    public virtual List<InventoryItem> GetInvHandItemList()
    {
        return invHandItemList;
    }
}