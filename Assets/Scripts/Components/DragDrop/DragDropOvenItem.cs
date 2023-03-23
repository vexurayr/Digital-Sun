using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropOvenItem : DragAndDrop
{
    #region Variables
    [SerializeField] private GameObject ovenObject;
    [SerializeField] private GameObject ovenUI;

    private Oven oven;
    private InventoryItem fuelInput;
    private InventoryItem convertInput;
    private InventoryItem outputItem;
    private CanvasGroup canvasGroup;

    private PlayerInventory playerInventory;

    #endregion Variables

    #region MonoBehaviours
    public override void Awake()
    {
        base.Awake();

        oven = ovenObject.GetComponent<Oven>();
        fuelInput = oven.GetFuelInput();
        convertInput = oven.GetConvertInput();
        outputItem = oven.GetOutput();
        canvasGroup = GetComponent<CanvasGroup>();
        playerInventory = oven.GetInteractingObject();
    }

    #endregion MonoBehaviours

    #region OnDragEvents
    public override void OnBeginDrag(PointerEventData eventData)
    {
        GameObject otherItem = eventData.pointerDrag;
        
        // Drop item to the lowest in the heirarchy so it appears on top of everything else
        int childCount = ovenUI.transform.childCount;

        otherItem.transform.SetSiblingIndex(childCount);

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        oven.RefreshInventoryVisuals();
    }

    #endregion OnDragEvents

    #region OnDropEvent
    // This event is triggered when the player moves one of their inv items into the oven's
    public override void OnDrop(PointerEventData eventData)
    {
        // Determine which InventoryItems need to swap
        GameObject otherItem = eventData.pointerDrag;
        int firstInvIndex = otherItem.GetComponent<IndexValue>().GetIndexValue();
        int secondInvIndex = GetComponent<IndexValue>().GetIndexValue();
        bool isFirstItemInvHand;
        bool isFirstItemInvArmor;
        bool isSecondItemInvHand;
        bool isSecondItemInvArmor;
        bool isCombineSuccessful = false;
        InventoryItem otherInvItem;
        InventoryItem thisInvItem;

        // Other InvItem is in invHandUI
        if (otherItem.GetComponent<IsInvHandItem>())
        {
            isFirstItemInvArmor = false;
            isFirstItemInvHand = true;
            otherInvItem = playerInventory.GetInvHandItemList()[firstInvIndex];
        }
        // Other InvItem is in invArmorUI
        else if (otherItem.GetComponent<IsInvArmorItem>())
        {
            isFirstItemInvHand = false;
            isFirstItemInvArmor = true;
            otherInvItem = playerInventory.GetInvItemArmorList()[firstInvIndex];
        }
        // Other InvItem is in invUI
        else
        {
            isFirstItemInvHand = false;
            isFirstItemInvArmor = false;
            otherInvItem = playerInventory.GetInvItemList()[firstInvIndex];
        }

        // If player is dragging around an empty invItem, don't do anything
        if (otherInvItem.GetItemType() == InventoryItem.ItemType.Empty)
        {
            return;
        }

        // This InvItem is in invHandUI
        if (GetComponent<IsInvHandItem>())
        {
            isSecondItemInvArmor = false;
            isSecondItemInvHand = true;
            thisInvItem = playerInventory.GetInvHandItemList()[secondInvIndex];
        }
        // This InvItem is in invArmorUI
        else if (GetComponent<IsInvArmorItem>())
        {
            isSecondItemInvHand = false;
            isSecondItemInvArmor = true;
            thisInvItem = playerInventory.GetInvItemArmorList()[secondInvIndex];
        }
        // This InvItem is in invUI
        else
        {
            isSecondItemInvHand = false;
            isSecondItemInvArmor = false;
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

        // Both items are in invHandUI
        if (isFirstItemInvHand && isSecondItemInvHand)
        {
            playerInventory.SwapTwoInvHandItems(firstInvIndex, secondInvIndex);
        }
        // Other item is in invUI, this item is in invHandUI
        else if (!isFirstItemInvHand && isSecondItemInvHand)
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
        else if (isFirstItemInvHand && !isSecondItemInvHand)
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
        else if (isFirstItemInvArmor && !isSecondItemInvArmor)
        {
            playerInventory.SwapArmorItemWithInvItem(firstInvIndex, secondInvIndex);
        }
        // Other item is in invUI, this item is in invArmorUI
        else if (!isFirstItemInvArmor && isSecondItemInvArmor)
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