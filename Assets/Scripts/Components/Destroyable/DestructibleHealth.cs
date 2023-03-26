using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class DestructibleHealth : Health
{
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] private int quantityToDrop;
    [SerializeField] private InventoryItem.ItemType destroyedBy;

    public override void OnTriggerEnter(Collider collider)
    {
        // Check if the hitbox that collided with the pawn is intended to deal damage
        if (!collider.gameObject.GetComponent<DamageSource>())
        {
            Debug.Log("No Damage Source");
            return;
        }

        // If destroyedBy is empty, the source of damage is not restricted
        if (destroyedBy == InventoryItem.ItemType.Empty)
        {
            InventoryItem item = collider.gameObject.GetComponentInParent<InventoryItem>();
            Defense defense = this.gameObject.GetComponent<Defense>();
            float damageTaken = item.GetDamageToEnemy();

            float totalDamage = damageTaken - (damageTaken * defense.GetCurrentValue());

            DecCurrentValue(totalDamage);
            Debug.Log("Damage Recieved: " + totalDamage);
        }
        // If it's a tool, use its damage to specialty instead
        else if (collider.gameObject.GetComponentInParent<Tool>())
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
            Debug.Log("Damage Recieved: " + totalDamage);
        }
        else if (collider.gameObject.GetComponentInParent<Weapon>())
        {
            if (collider.gameObject.GetComponentInParent<Weapon>().GetItemType() != destroyedBy)
            {
                return;
            }

            Weapon weapon = collider.gameObject.GetComponentInParent<Weapon>();
            Defense defense = this.gameObject.GetComponent<Defense>();
            float damageTaken = weapon.GetDamageToEnemy();

            float totalDamage = damageTaken - (damageTaken * defense.GetCurrentValue());

            DecCurrentValue(totalDamage);
            Debug.Log("Damage Recieved: " + totalDamage);
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