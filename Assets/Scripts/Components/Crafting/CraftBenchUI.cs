using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBenchUI : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private Recipe emptyRecipe;

    private PlayerInventory player;

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
        // Bail right away if the player doesn't have enough space
        if (player.IsInventoryFull())
        {
            return;
        }

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

        targetRecipe.SetItemsRequiredCount();

        // Check all the requirements of the recipe
        for (int i = 0; i < targetRecipe.GetRequiredItems().Count; i++)
        {
            InventoryItem itemRequired = targetRecipe.GetRequiredItems()[i];

            bool isSuccessful = player.HasItemForRecipe(itemRequired);

            if (!isSuccessful)
            {
                return;
            }
        }

        // The player has enough resources
        foreach (InventoryItem itemRequired in targetRecipe.GetRequiredItems())
        {
            player.RemoveItemFromInventory(itemRequired);
        }

        // Give the player the item
        targetRecipe.SetItemOutputRequiredCount();
        player.AddToInventory(targetRecipe.GetItemOutput());
        player.RefreshInventoryVisuals();
    }
}