using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    
    protected override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        var inventory = player.GetComponent<Inventory>();
        
        if (inventory.Add(item))
        {
            Debug.Log($"picking up {item.name}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("not enough room");
        }
    }
}
