using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class TreeHealth : Health
{
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] private int quantityToDrop;

    public override void OnTriggerEnter(Collider collider)
    {
        // Check if an axe's hitbox has collided with the pawn
        if (collider.gameObject.GetComponent<DamageSource>() && collider.gameObject.GetComponentInParent<Tool>())
        {
            if (collider.gameObject.GetComponentInParent<Tool>().GetItemType() != InventoryItem.ItemType.Axe)
            {
                return;
            }

            Tool tool = collider.gameObject.GetComponentInParent<Tool>();
            Defense defense = this.gameObject.GetComponent<Defense>();
            float damageTaken = tool.GetDamageToSpecialty();

            float totalDamage = damageTaken - (damageTaken * defense.GetCurrentValue());

            DecCurrentValue(totalDamage);
        }
    }

    public override void Die()
    {
        // Drop wood before being destroyed
        GameObject newResource = Instantiate(itemToDrop, this.gameObject.transform.position, this.gameObject.transform.rotation);
        newResource.GetComponent<InventoryItem>().SetItemCount(quantityToDrop);

        Destroy(gameObject);
    }
}