using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBenchUI : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private Recipe emptyRecipe;

    private PlayerInventory player;

    public void Start()
    {
        if (GetComponentInParent<PlayerInventory>() != null)
        {
            player = GetComponentInParent<PlayerInventory>();
        }
    }

    public Recipe GetRecipe(InventoryItem.Item recipeName)
    { 
        foreach (Recipe recipe in recipes)
        {
            if (recipeName == recipe.GetRecipeName())
            {
                return recipe;
            }
        }

        return null;
    }

    public void SetInteractingObject(PlayerInventory player)
    {
        this.player = player;
    }

    public void CraftStick()
    {
        CraftItem(InventoryItem.Item.Stick);
    }

    public void CraftRope()
    {
        CraftItem(InventoryItem.Item.Rope);
    }

    public void CraftBandage()
    {
        CraftItem(InventoryItem.Item.Bandage);
    }

    public void CraftStaminaBoost()
    {
        CraftItem(InventoryItem.Item.Stamina_Boost);
    }

    public void CraftCraftBench()
    {
        CraftItem(InventoryItem.Item.Craft_Bench);
    }

    public void CraftOven()
    {
        CraftItem(InventoryItem.Item.Oven);
    }

    public void CraftCanteen()
    {
        CraftItem(InventoryItem.Item.Canteen);
    }

    public void CraftWoodSpear()
    {
        CraftItem(InventoryItem.Item.Wood_Spear);
    }

    public void CraftStoneAxe()
    {
        CraftItem(InventoryItem.Item.Stone_Axe);
    }

    public void CraftStonePickaxe()
    {
        CraftItem(InventoryItem.Item.Stone_Pickaxe);
    }

    public void CraftClothBandana()
    {
        CraftItem(InventoryItem.Item.Cloth_Bandana);
    }

    public void CraftWoodenChestplate()
    {
        CraftItem(InventoryItem.Item.Wood_Chestplate);
    }

    public void CraftWoodenLeggings()
    {
        CraftItem(InventoryItem.Item.Wood_Leggings);
    }

    public void CraftItem(InventoryItem.Item targetItem)
    {
        Recipe targetRecipe = emptyRecipe;

        // Get the desired recipe
        foreach (Recipe recipe in recipes)
        {
            if (recipe.GetRecipeName() == targetItem)
            {
                targetRecipe = recipe;
                break;
            }
        }

        if (targetRecipe.GetRecipeName() == InventoryItem.Item.None)
        {
            return;
        }

        //targetRecipe.SetItemsRequiredCount();

        // Bail right away if the player doesn't have enough space
        if (player.IsInventoryFull())
        {
            // HasSameItemOfNonMaxStackSize returns an empty inventory item if there is no room
            if (player.HasSameItemOfNonMaxStackSize(targetRecipe.GetItemOutput()).GetItem() != emptyRecipe.GetRecipeName())
            {
                // In this case the play has room for that specific item
                Debug.Log("Player has room to make more of this item");
            }
            else
            {
                Debug.Log("Player does not have room to make more of this item");
                return;
            }
        }

        // Check all the requirements of the recipe
        for (int i = 0; i < targetRecipe.GetRequiredItems().Count; i++)
        {
            InventoryItem itemRequired = targetRecipe.GetRequiredItems()[i];

            bool isSuccessful = player.HasItemForRecipe(itemRequired);

            if (!isSuccessful)
            {
                Debug.Log("Player is missing an item for this recipe");
                return;
            }
        }

        // The player has enough resources
        foreach (InventoryItem itemRequired in targetRecipe.GetRequiredItems())
        {
            player.RemoveItemFromInventory(itemRequired);
        }

        // Give the player the item
        //targetRecipe.SetItemOutputRequiredCount();
        player.AddToInventory(targetRecipe.GetItemOutput());
        player.RefreshInventoryVisuals();
    }
}