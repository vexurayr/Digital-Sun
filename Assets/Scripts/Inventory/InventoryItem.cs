using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    #region Variables
    public enum Item
    {
        None,
        Wood,
        Stone,
        Water,
        Berry,
        Bandage,
        Stamina_Boost,
        Cloth_Bandana,
        Wood_Chestplate,
        Wood_Leggings,
        Wood_Spear,
        Stone_Axe
    }

    public enum ItemType
    {
        Empty,
        Resource,
        Consumable,
        Equipment,
        Axe,
        Pickaxe,
        Weapon,
        Helmet,
        Chestplate,
        Leggings
    }

    [SerializeField] protected Item item;
    [SerializeField] protected ItemType itemType;
    [SerializeField] protected Sprite itemSprite;

    // Used to manage same objects in inventory
    [SerializeField] protected int itemCount;
    [SerializeField] protected int maxStackSize;

    // Used to place objects in a way that the player seems to be holding it
    [SerializeField] protected float transformInHandX, transformInHandY, transformInHandZ;
    [SerializeField] protected float rotationInHandX, rotationInHandY, rotationInHandZ;

    #endregion Variables

    #region Actions
    public virtual bool PrimaryAction(GameObject player)
    { 
        return false;
    }

    public virtual bool PrimaryAction()
    {
        return false;
    }

    public virtual bool SecondaryAction(GameObject player)
    {
        return false;
    }

    public virtual bool SecondaryAction()
    {
        return false;
    }

    public void PickItemUp(Inventory targetInv)
    {
        targetInv.AddToInventory(this.gameObject);
    }

    #endregion Actions

    #region GetSet
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

    public Vector3 GetTransformInHand()
    {
        return new Vector3(transformInHandX, transformInHandY, transformInHandZ);
    }

    public Vector3 GetRotationInHand()
    {
        return new Vector3(rotationInHandX, rotationInHandY, rotationInHandZ);
    }

    #endregion GetSet
}