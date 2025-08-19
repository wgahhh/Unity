using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : InteractableObject
{
    public ItemSO item;
    protected override void Interact()
    {
        InventoryManager.Instance.AddItem(item);
        Destroy(this.gameObject);
    }
}
