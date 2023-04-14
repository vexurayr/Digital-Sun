using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropInvItem : DragAndDrop
{
    #region Variables
    [SerializeField] private GameObject invUI;

    private PlayerInventory playerInventory;
    private CanvasGroup canvasGroup;

    #endregion Variables

    #region MonoBehaviours
    public override void Awake()
    {
        base.Awake();
        playerInventory = GetComponentInParent<PlayerInventory>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    #endregion MonoBehaviours

    #region OnDragEvents
    public override void OnBeginDrag(PointerEventData eventData)
    {
        GameObject otherItem = eventData.pointerDrag;
        
        // Drop item to the lowest in the heirarchy so it appears on top of everything else
        int childCount = invUI.transform.childCount;

        otherItem.transform.SetSiblingIndex(childCount);

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        playerInventory.RefreshInventoryVisuals();
    }

    #endregion OnDragEvents

    #region OnDropEvent
    public override void OnDrop(PointerEventData eventData)
    {
        GameObject otherItem = eventData.pointerDrag;

        // Determine which InventoryItems need to swap
        int firstInvIndex = 0;
        int secondInvIndex = 0;
        bool isFirstItemInv = false;
        bool isFirstItemInvHand = false;
        bool isFirstItemInvArmor = false;
        bool isSecondItemInv = false;
        bool isSecondItemInvHand = false;
        bool isSecondItemInvArmor = false;
        bool isFirstItemFuelInput = false;
        bool isSecondItemFuelInput = false;
        bool isFirstItemConvertInput = false;
        bool isSecondItemConvertInput = false;
        bool isFirstItemOutput = false;
        bool isSecondItemOutput = false;
        bool isCombineSuccessful = false;
        InventoryItem otherInvItem = playerInventory.GetEmptyInventoryItem().GetComponent<InventoryItem>();
        InventoryItem thisInvItem = playerInventory.GetEmptyInventoryItem().GetComponent<InventoryItem>();
        GameObject prefabFromInvItemManager;

        if (otherItem.GetComponent<IndexValue>())
        {
            firstInvIndex = otherItem.GetComponent<IndexValue>().GetIndexValue();
        }
        if (GetComponent<IndexValue>())
        {
            secondInvIndex = GetComponent<IndexValue>().GetIndexValue();
        }
        if (otherItem.GetComponent<Classify>())
        {
            if (otherItem.GetComponent<Classify>().GetClassification() == Classify.Classification.IsOvenFuelInput)
            {
                isFirstItemFuelInput = true;
                otherInvItem = playerInventory.GetOvenFuelInput();
            }
            else if (otherItem.GetComponent<Classify>().GetClassification() == Classify.Classification.IsOvenConvertInput)
            {
                isFirstItemConvertInput = true;
                otherInvItem = playerInventory.GetOvenConvertInput();
            }
            else if (otherItem.GetComponent<Classify>().GetClassification() == Classify.Classification.IsOvenOutput)
            {
                isFirstItemOutput = true;
                otherInvItem = playerInventory.GetOvenOutput();
            }
        }
        if (GetComponent<Classify>())
        {
            if (GetComponent<Classify>().GetClassification() == Classify.Classification.IsOvenFuelInput)
            {
                isSecondItemFuelInput = true;
                thisInvItem = playerInventory.GetOvenFuelInput();
            }
            else if (GetComponent<Classify>().GetClassification() == Classify.Classification.IsOvenConvertInput)
            {
                isSecondItemConvertInput = true;
                thisInvItem = playerInventory.GetOvenConvertInput();
            }
            else if (GetComponent<Classify>().GetClassification() == Classify.Classification.IsOvenOutput)
            {
                isSecondItemOutput = true;
                thisInvItem = playerInventory.GetOvenOutput();
            }
        }

        if (otherItem.GetComponent<Classify>())
        {
            // Skip this block to avoid overwriting thisInvItem
        }
        // Other InvItem is in invHandUI
        else if (otherItem.GetComponent<IsInvHandItem>())
        {
            isFirstItemInvHand = true;
            otherInvItem = playerInventory.GetInvHandItemList()[firstInvIndex];
        }
        // Other InvItem is in invArmorUI
        else if (otherItem.GetComponent<IsInvArmorItem>())
        {
            isFirstItemInvArmor = true;
            otherInvItem = playerInventory.GetInvItemArmorList()[firstInvIndex];
        }
        // Other InvItem is in invUI
        else
        {
            isFirstItemInv = true;
            otherInvItem = playerInventory.GetInvItemList()[firstInvIndex];
        }

        // If player is dragging around an empty invItem, don't do anything
        if (otherInvItem.GetItemType() == InventoryItem.ItemType.Empty)
        {
            return;
        }

        if (GetComponent<Classify>())
        {
            // Skip this block to avoid overwriting thisInvItem
        }
        // This InvItem is in invHandUI
        else if (GetComponent<IsInvHandItem>())
        {
            isSecondItemInvHand = true;
            thisInvItem = playerInventory.GetInvHandItemList()[secondInvIndex];
        }
        // This InvItem is in invArmorUI
        else if (GetComponent<IsInvArmorItem>())
        {
            isSecondItemInvArmor = true;
            thisInvItem = playerInventory.GetInvItemArmorList()[secondInvIndex];
        }
        // This InvItem is in invUI
        else
        {
            isSecondItemInv = true;
            thisInvItem = playerInventory.GetInvItemList()[secondInvIndex];
        }

        // Check if items are the same
        if (thisInvItem.GetItem() == otherInvItem.GetItem())
        {
            // Attempt to combine this item into the other item's stack instead of swapping
            isCombineSuccessful = playerInventory.CombineStacks(thisInvItem, otherInvItem, firstInvIndex, isFirstItemInvHand);
        }

        // Don't swap items if they successfully combined stacks
        if (isCombineSuccessful)
        {
            return;
        }

        // If either is -1, the swap happened above
        if (firstInvIndex < 0 || secondInvIndex < 0)
        {
            return;
        }

        // Other item is inv, this item is fuelInput
        if (isFirstItemInv && isSecondItemFuelInput)
        {
            prefabFromInvItemManager = InvItemManager.instance.GetPrefabForInvItem(otherInvItem);

            if (!prefabFromInvItemManager.GetComponent<Classify>())
            {
                return;
            }
            // Only allow if the inventory item is a fuel source
            if (prefabFromInvItemManager.GetComponent<Classify>().GetClassification() == Classify.Classification.IsFuelSource)
            {
                playerInventory.SwapInvItemWithOvenItem(firstInvIndex, thisInvItem,
                    isSecondItemFuelInput, isSecondItemConvertInput, isSecondItemOutput);
            }
        }
        // Other item is fuelInput, this item is inv
        else if (isFirstItemFuelInput && isSecondItemInv)
        {
            // Dragging fuel back into the inventory should be completely fine
            playerInventory.SwapInvItemWithOvenItem(secondInvIndex, otherInvItem,
                isFirstItemFuelInput, isFirstItemConvertInput, isFirstItemOutput);
        }
        // Other item is inv, this item is convertInput
        else if (isFirstItemInv && isSecondItemConvertInput)
        {
            prefabFromInvItemManager = InvItemManager.instance.GetPrefabForInvItem(otherInvItem);

            if (!prefabFromInvItemManager.gameObject.GetComponent<Classify>())
            {
                return;
            }
            // Only allow if the inventory item is a convertable item
            if (prefabFromInvItemManager.gameObject.GetComponent<Classify>().GetClassification() == Classify.Classification.IsConvertable)
            {
                playerInventory.SwapInvItemWithOvenItem(firstInvIndex, thisInvItem,
                    isSecondItemFuelInput, isSecondItemConvertInput, isSecondItemOutput);
            }
        }
        // Other item is convertInput, this item is inv
        else if (isFirstItemConvertInput && isSecondItemInv)
        {
            // Dragging non-converted item back into the inventory should be fine
            playerInventory.SwapInvItemWithOvenItem(secondInvIndex, otherInvItem,
                isFirstItemFuelInput, isFirstItemConvertInput, isFirstItemOutput);
        }
        // Other item is invHand, this item is convertInput
        else if (isFirstItemInvHand && isSecondItemConvertInput)
        {
            prefabFromInvItemManager = InvItemManager.instance.GetPrefabForInvItem(otherInvItem);

            if (!prefabFromInvItemManager.gameObject.GetComponent<Classify>())
            {
                return;
            }
            // Player could be holding an item like uncooked meat in their hand
            if (prefabFromInvItemManager.gameObject.GetComponent<Classify>().GetClassification() == Classify.Classification.IsConvertable)
            {
                playerInventory.SwapInvHandItemWithOvenItem(firstInvIndex, thisInvItem,
                    isSecondItemFuelInput, isSecondItemConvertInput, isSecondItemOutput);
            }
        }
        // Other item is convertInput, this item is invHand
        else if (isFirstItemConvertInput && isSecondItemInvHand)
        {
            // Don't allow resources to be dragged into the hand
            if (otherInvItem.GetItemType() == InventoryItem.ItemType.Resource)
            {
                return;
            }

            playerInventory.SwapInvHandItemWithOvenItem(secondInvIndex, otherInvItem,
                isFirstItemFuelInput, isFirstItemConvertInput, isFirstItemOutput);
        }
        // Other item is inv, this item is output
        else if (isFirstItemInv && isSecondItemOutput)
        {
            // For simplicity sake, don't allow items to be dragged into the output
        }
        // Other item is output, this item is inv
        else if (isFirstItemOutput && isSecondItemInv)
        {
            // Dragging an output item into the player's inventory should be fine
            playerInventory.SwapInvItemWithOvenItem(secondInvIndex, otherInvItem,
                isFirstItemFuelInput, isFirstItemConvertInput, isFirstItemOutput);
        }
        // Both items are in invHandUI
        else if (isFirstItemInvHand && isSecondItemInvHand)
        {
            playerInventory.SwapTwoInvHandItems(firstInvIndex, secondInvIndex);
        }
        // Other item is in invUI, this item is in invHandUI
        else if (isFirstItemInv && isSecondItemInvHand)
        {
            // Prevent resources from entering the invHand
            if (playerInventory.GetInvItemList()[firstInvIndex].GetItemType() == InventoryItem.ItemType.Resource)
            {
                return;
            }

            // Prevent armor from entering the invHand
            if (playerInventory.GetInvItemList()[firstInvIndex].GetItemType() == InventoryItem.ItemType.Helmet ||
                playerInventory.GetInvItemList()[firstInvIndex].GetItemType() == InventoryItem.ItemType.Chestplate ||
                playerInventory.GetInvItemList()[firstInvIndex].GetItemType() == InventoryItem.ItemType.Leggings)
            {
                return;
            }

            playerInventory.SwapInvItemWithHandItem(firstInvIndex, secondInvIndex);
        }
        // Other item is in invHandUI, this item is in invUI
        else if (isFirstItemInvHand && isSecondItemInv)
        {
            // Prevent resources from entering the invHand
            if (playerInventory.GetInvItemList()[secondInvIndex].GetItemType() == InventoryItem.ItemType.Resource)
            {
                return;
            }

            // Prevent armor from entering the invHand
            if (playerInventory.GetInvItemList()[secondInvIndex].GetItemType() == InventoryItem.ItemType.Helmet ||
                playerInventory.GetInvItemList()[secondInvIndex].GetItemType() == InventoryItem.ItemType.Chestplate ||
                playerInventory.GetInvItemList()[secondInvIndex].GetItemType() == InventoryItem.ItemType.Leggings)
            {
                return;
            }

            playerInventory.SwapInvItemWithHandItem(secondInvIndex, firstInvIndex);
        }
        // Other item is in invArmorUI, this item is in invHandUI
        else if (isFirstItemInvArmor && isSecondItemInvHand)
        {
            // Armor shouldn't be in the hotbar
        }
        // Other item is in invHandUI, this item is in inveArmorUI
        else if (isFirstItemInvHand && isSecondItemInvArmor)
        {
            // Armor shouldn't be in the hotbar
        }
        // Other item is in invArmorUI, this item is in invUI
        else if (isFirstItemInvArmor && isSecondItemInv)
        {
            playerInventory.SwapArmorItemWithInvItem(firstInvIndex, secondInvIndex);
        }
        // Other item is in invUI, this item is in invArmorUI
        else if (isFirstItemInv && isSecondItemInvArmor)
        {
            playerInventory.SwapArmorItemWithInvItem(secondInvIndex, firstInvIndex);
        }
        // Both items are in invArmorUI
        else if (isFirstItemInvArmor && isSecondItemInvArmor)
        {
            // Do nothing because this would mean swapping helmet in helmet slot with chestplate in chestplate slot
        }
        // Both items are in invUI
        else
        {
            playerInventory.SwapTwoInvItems(firstInvIndex, secondInvIndex);
        }
    }

    #endregion OnDropEvent
}