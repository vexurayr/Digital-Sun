using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class DestructableHealth : Health
{
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] private int quantityToDrop;
    [SerializeField] private InventoryItem.ItemType destroyedBy;

    public override void OnTriggerEnter(Collider collider)
    {
        // Check if the hitbox that collided with the pawn is intended to deal damage
        if (collider.gameObject.GetComponent<DamageSource>() && collider.gameObject.GetComponentInParent<Tool>())
        {
            if (collider.gameObject.GetComponentInParent<Tool>().GetItemType() != destroyedBy)
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
        // Drop its item before being destroyed
        GameObject newResource = Instantiate(itemToDrop, this.gameObject.transform.position, this.gameObject.transform.rotation);
        newResource.GetComponent<InventoryItem>().SetItemCount(quantityToDrop);

        Destroy(gameObject);
    }
}