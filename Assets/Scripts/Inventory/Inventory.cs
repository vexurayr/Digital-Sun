using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Variables
    [SerializeField] protected int invSlots;
    [SerializeField] protected int invHandSlots;
    [SerializeField] protected InventoryItem emptyInvItem;

    protected List<InventoryItem> invItemList;
    protected List<InventoryItem> invHandItemList;
    protected List<InventoryItem> invItemArmorList;

    protected InventoryItem ovenFuelInput;
    protected InventoryItem ovenConvertInput;
    protected InventoryItem ovenOutput;

    #endregion Variables

    #region MonoBehaviours
    public virtual void Awake()
    {
        invItemList = new List<InventoryItem>();
        invHandItemList = new List<InventoryItem>();
        invItemArmorList = new List<InventoryItem>();

        InitializeInventory();
    }

    #endregion MonoBehaviours

    #region GetSet
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

    public virtual List<InventoryItem> GetInvItemArmorList()
    {
        return invItemArmorList;
    }

    public virtual GameObject GetEmptyInventoryItem()
    {
        GameObject obj = InvItemManager.instance.GetPrefabForInvItem(emptyInvItem);

        return obj;
    }

    #endregion GetSet

    #region AddItemToInventory
    public virtual void AddToInventory(InventoryItem newItem)
    {
        if (newItem == null)
        {
            return;
        }

        if (newItem.GetItemType() == InventoryItem.ItemType.Resource || newItem.GetItemType() == InventoryItem.ItemType.Consumable)
        {
            // Check if incoming item can be added to an existing stack
            for (int i = 0; i < invItemList.Count; i++)
            {
                // Check if resource/consumable in this inventory slot matches the incoming item's
                if (invItemList[i].GetItem() == newItem.GetItem())
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
                        Debug.Log("Hash code of invItemList[i]: " + invItemList[i].GetHashCode());
                        invItemList[i].SetItemCount(newAmount);
                        return;
                    }
                }
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
        if (newObj.GetComponent<InventoryItem>() != null)
        {
            InventoryItem newItem = newObj.GetComponent<InventoryItem>();
            AddToInventory(newItem);
        }
    }

    #endregion AddItemToInventory

    #region RemoveItemFromInventory
    public virtual void RemoveItemFromInventory(int itemAtIndexToRemove, bool isInvHandItem, bool isInvArmorItem)
    {
        if (itemAtIndexToRemove < 0 || itemAtIndexToRemove > invItemList.Count)
        {
            return;
        }

        if (isInvHandItem)
        {
            invHandItemList[itemAtIndexToRemove] = emptyInvItem;
        }
        else if (isInvArmorItem)
        {
            invItemArmorList[itemAtIndexToRemove] = emptyInvItem;
        }
        else
        {
            invItemList[itemAtIndexToRemove] = emptyInvItem;
        }
    }

    // Will be used to remove items from the core inventory for crafting recipes
    public virtual bool RemoveItemFromInventory(InventoryItem itemToRemove)
    {
        List<int> itemsWithLowCount = new List<int>();
        int totalItemCount = 0;
        int remainder = itemToRemove.GetItemCount();

        // Check each slot of the player's inventory from last to first
        for(int i = invItemList.Count - 1; i >= 0; i--)
        {
            // If they have the item
            if (invItemList[i].GetItem() == itemToRemove.GetItem())
            {
                // If they have more than enough items
                if (invItemList[i].GetItemCount() > itemToRemove.GetItemCount() && itemsWithLowCount.Count == 0)
                {
                    invItemList[i].SetItemCount(invItemList[i].GetItemCount() - itemToRemove.GetItemCount());
                    return true;
                }
                // If they have just enough items
                else if (invItemList[i].GetItemCount() == itemToRemove.GetItemCount() && itemsWithLowCount.Count == 0)
                {
                    RemoveItemFromInventory(i, false, false);
                    return true;
                }
                // Have to check for any other same items
                else
                {
                    itemsWithLowCount.Add(i);
                    totalItemCount += invItemList[i].GetItemCount();
                }
            }
        }

        // Don't proceed if the total item count still isn't enough
        if (totalItemCount < itemToRemove.GetItemCount())
        {
            return false;
        }

        // Go through the items until there are enough to remove
        foreach (int index in itemsWithLowCount)
        {
            remainder -= invItemList[index].GetItemCount();

            // More items are needed
            if (remainder > 0)
            {
                RemoveItemFromInventory(index, false, false);
            }
            // Gathered the last items needed
            else if (remainder == 0)
            {
                RemoveItemFromInventory(index, false, false);
                return true;
            }
            // More than enough items were found
            else
            {
                invItemList[index].SetItemCount(Mathf.Abs(remainder));
                return true;
            }
        }

        return false;
    }

    #endregion RemoveItemFromInventory

    #region HelperFunctions
    public virtual void InitializeInventory()
    {
        invItemList.Clear();
        invHandItemList.Clear();
        invItemArmorList.Clear();

        for (int i = 0; i < invSlots; i++)
        {
            invItemList.Add(emptyInvItem);
        }

        for (int i = 0; i < invHandSlots; i++)
        {
            invHandItemList.Add(emptyInvItem);
        }

        for (int i = 0; i < 3; i++)
        {
            invItemArmorList.Add(emptyInvItem);
        }

        ovenFuelInput = emptyInvItem;
        ovenConvertInput = emptyInvItem;
        ovenOutput = emptyInvItem;
    }

    // Only checks invItemList because items will never be placed directly into the hotbar
    public bool IsInventoryFull()
    {
        foreach (InventoryItem item in invItemList)
        {
            if (item.GetItem() == InventoryItem.Item.None)
            {
                return false;
            }
        }

        return true;
    }

    public InventoryItem HasSameItemOfNonMaxStackSize(InventoryItem itemToCheck)
    {
        foreach (InventoryItem item in invItemList)
        {
            // Items match and the inventory item has room for more
            if (item.GetItem() == itemToCheck.GetItem() && item.GetItemCount() != item.GetMaxStackSize())
            {
                return item;
            }
        }
        
        return emptyInvItem;
    }

    public bool HasItemForRecipe(InventoryItem itemForRecipe)
    {
        List<InventoryItem> itemsWithLowCount = new List<InventoryItem>();
        int totalItemCount = 0;
        int remainder = itemForRecipe.GetItemCount();

        // Check each slot of the player's inventory
        foreach (InventoryItem item in invItemList)
        {
            // If they have the item
            if (item.GetItem() == itemForRecipe.GetItem())
            {
                // If they have more than enough items
                if (item.GetItemCount() > itemForRecipe.GetItemCount() && itemsWithLowCount.Count == 0)
                {
                    return true;
                }
                // If they have just enough items
                else if (item.GetItemCount() == itemForRecipe.GetItemCount() && itemsWithLowCount.Count == 0)
                {
                    return true;
                }
                // Have to check for any other same items
                else
                {
                    itemsWithLowCount.Add(item);
                    totalItemCount += item.GetItemCount();
                }
            }
        }

        // Don't proceed if the total item count still isn't enough
        if (totalItemCount < itemForRecipe.GetItemCount())
        {
            return false;
        }

        // Go through the items until there are enough to remove
        foreach (InventoryItem item in itemsWithLowCount)
        {
            remainder -= item.GetItemCount();

            // More items are needed
            if (remainder > 0)
            { }
            // Gathered the last items needed
            else if (remainder == 0)
            {
                return true;
            }
            // More than enough items were found
            else
            {
                return true;
            }
        }

        return false;
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

    #endregion HelperFunctions
}