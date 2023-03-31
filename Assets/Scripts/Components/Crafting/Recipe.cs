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

    //private InventoryItem itemOutput;

    private void Start()
    {
        Debug.Log("ANything");
        //itemOutput = itemToCreate;
    }

    private void Update()
    {
        Debug.Log("Item Count for " + itemOutput + ": " + itemOutput.GetItemCount());
    }

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
        return itemOutput;
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