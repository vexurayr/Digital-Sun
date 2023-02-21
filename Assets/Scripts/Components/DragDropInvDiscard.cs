using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropInvDiscard : DragAndDrop
{
    [SerializeField] private GameObject player;

    private PlayerInventory playerInventory;
    private List<InventoryItem> inventoryItems;
    private List<InventoryItem> inventoryHandItems;

    public override void Awake()
    {
        base.Awake();
        playerInventory = player.GetComponent<PlayerInventory>();
        inventoryItems = playerInventory.GetInvItemList();
        inventoryHandItems = playerInventory.GetInvHandItemList();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        // Get index of InvItem to discard aka the object being dragged by the mouse
        GameObject otherItem = eventData.pointerDrag;
        int index = otherItem.GetComponent<IndexValue>().GetIndexValue();

        // Determine if InvItem comes from the inventory or the hotbar
        bool isInvHandItem;

        if (otherItem.GetComponent<IsInvHandItem>())
        {
            isInvHandItem = true;
        }
        else
        {
            isInvHandItem = false;
        }

        // Get InvItem from playerInventory using index
        InventoryItem invItem;

        if (!isInvHandItem)
        {
            invItem = inventoryItems[index];
        }
        else
        {
            invItem = inventoryHandItems[index];
        }

        // Get InvItem Prefab from instance of InvItemManager using InvItem
        GameObject itemToDiscard = InvItemManager.instance.GetPrefabForInvItem(invItem);

        // Set item count of this prefab's InventoryItem component to match the item count from the InvItem
        // Everything else will already match
        itemToDiscard.GetComponent<InventoryItem>().SetItemCount(invItem.GetItemCount());

        // Don't want to spawn/remove an empty item
        if (itemToDiscard.GetComponent<InventoryItem>().GetItem() != InventoryItem.Item.None)
        {
            Vector3 spawnLocation = player.gameObject.transform.position +
                (player.gameObject.transform.forward * player.GetComponent<PlayerController>().GetPickupMinDropDistance());
            spawnLocation.y = 0;

            // Use Physics.OverlapShere to check if there is an inventory item in the way
            Collider[] hitColliders = Physics.OverlapSphere(spawnLocation, 0.5f);
            Collider obstacle = new Collider();

            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.GetComponent<InventoryItem>())
                {
                    obstacle = collider;
                    continue;
                }
            }

            if (obstacle != null)
            {
                // Take into account height of object and distance from the object to ground
                var maxBounds = GetMaxBounds(obstacle.gameObject);
                spawnLocation.y = (maxBounds.size.y + obstacle.transform.position.y);

                Instantiate(itemToDiscard, spawnLocation, player.gameObject.transform.rotation);
            }
            else
            {
                // Spawn itemToDiscard in front of the player at valid location
                Instantiate(itemToDiscard, spawnLocation, player.gameObject.transform.rotation);
            }
            
            // Remove InvItem from playerInventory's list of InvItems using index
            playerInventory.RemoveFromInventory(index, isInvHandItem);
        }
        
        playerInventory.RefreshInventoryVisuals();
    }

    public Bounds GetMaxBounds(GameObject parentObj)
    {
        Bounds totalColliderSize = new Bounds(parentObj.transform.position, Vector3.zero);

        foreach (var child in parentObj.GetComponentsInChildren<Collider>())
        {
            totalColliderSize.Encapsulate(child.bounds);
        }

        return totalColliderSize;
    }
}