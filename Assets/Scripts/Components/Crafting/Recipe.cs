using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    [SerializeField] private InventoryItem.Item recipeName;
    [SerializeField] private List<InventoryItem> requiredItems;
    [SerializeField] private List<int> requiredItemCounts;
    [SerializeField] private InventoryItem itemOutput;
    [SerializeField] private int itemCountOutput;

    public InventoryItem.Item GetRecipeName()
    {
        return recipeName;
    }

    public List<InventoryItem> GetRequiredItems()
    {
        return requiredItems;
    }

    public List<int> GetRequiredItemCounts()
    {
        return requiredItemCounts;
    }

    public InventoryItem GetItemOutput()
    {
        GameObject newObject = Instantiate(InvItemManager.instance.GetPrefabForItem(InventoryItem.Item.None), this.transform.position, this.transform.rotation);

        InventoryItem newItem = newObject.GetComponent<InventoryItem>();

        Destroy(newObject);

        newItem.SetItem(itemOutput.GetItem());
        newItem.SetItemType(itemOutput.GetItemType());
        newItem.SetItemSprite(itemOutput.GetItemSprite());
        newItem.SetMaxStackSize(itemOutput.GetMaxStackSize());
        newItem.SetItemCount(itemOutput.GetItemCount());
        newItem.SetTransformInHand(itemOutput.GetTransformInHand());
        newItem.SetRotationInHand(itemOutput.GetRotationInHand());

        return newItem.GetComponent<InventoryItem>();
    }

    public void SetItemsRequiredCount()
    {
        for (int i = 0; i < requiredItems.Count; i++)
        {
            requiredItems[i].SetItemCount(requiredItemCounts[i]);
        }
    }

    public void SetItemOutputRequiredCount()
    {
        itemOutput.SetItemCount(itemCountOutput);
    }
}