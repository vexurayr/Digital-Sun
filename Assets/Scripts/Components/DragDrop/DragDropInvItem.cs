using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropInvItem : DragAndDrop
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject invUI;

    private PlayerInventory playerInventory;
    private CanvasGroup canvasGroup;

    public override void Awake()
    {
        base.Awake();
        playerInventory = player.GetComponent<PlayerInventory>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

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

    public override void OnDrop(PointerEventData eventData)
    {
        // Determine which InventoryItems need to swap
        GameObject otherItem = eventData.pointerDrag;
        int firstInvIndex = otherItem.GetComponent<IndexValue>().GetIndexValue();
        int secondInvIndex = GetComponent<IndexValue>().GetIndexValue();
        bool isFirstItemInvHand;
        bool isSecondItemInvHand;

        // Other InvItem is in invUI
        if (otherItem.GetComponent<IsInvHandItem>())
        {
            isFirstItemInvHand = true;
        }
        // Other InvItem is in invHandUI
        else
        {
            isFirstItemInvHand = false;
        }

        // This InvItem is in invUI
        if (GetComponent<IsInvHandItem>())
        {
            isSecondItemInvHand = true;
        }
        // This InvItem is in invHandUI
        else
        {
            isSecondItemInvHand = false;
        }

        // Both items are in invUI
        if (!isFirstItemInvHand && !isSecondItemInvHand)
        {
            playerInventory.SwapTwoInvItems(firstInvIndex, secondInvIndex);
        }
        // Both items are in invHandUI
        else if (isFirstItemInvHand && isSecondItemInvHand)
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

            playerInventory.SwapInvItemWithHandItem(secondInvIndex, firstInvIndex);
        }
    }
}