using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public enum Item
    {
        None,
        Wood,
        Stone,
        Water,
        Berry,
        Bandage,
        StaminaBoost
    }

    public enum ItemType
    {
        Empty,
        Resource,
        Consumable,
        Equipment,
        Weapon,
        Armor
    }

    [SerializeField] private Item item;
    [SerializeField] private ItemType itemType;
    [SerializeField] private Sprite itemSprite;

    // Used to manage same objects in inventory
    [SerializeField] private int itemCount;
    [SerializeField] private int maxStackSize;

    public void PickItemUp(Inventory targetInv)
    {
        targetInv.AddToInventory(this.gameObject);
    }

    public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public void SetItemType(ItemType newType)
    {
        itemType = newType;
    }

    public int GetItemCount()
    {
        return itemCount;
    }

    public void SetItemCount(int num)
    {
        itemCount = num;
    }

    public int GetMaxStackSize()
    {
        return maxStackSize;
    }

    public void SetMaxStackSize(int num)
    {
        maxStackSize = num;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public void SetItemSprite(Sprite newSprite)
    {
        itemSprite = newSprite;
    }
}