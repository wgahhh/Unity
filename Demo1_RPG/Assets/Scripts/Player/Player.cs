using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerProperty playerProperty;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerProperty = GetComponent<PlayerProperty>();
    }

    public void UseItem(ItemSO item)
    {
        switch (item.itemType)
        {
            case ItemType.Weapon:
                playerAttack.LoadWeapon(item);
                break;
            case ItemType.ConsumableItem:
                playerProperty.UseDrug(item);
                break;
            default:
                break;
        }

        PropertyUI.Instance.UpdateProperty();
    }
}
