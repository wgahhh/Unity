using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI type;

    private ItemSO item;

    public void InitItem(ItemSO item)
    {
        string type = "";
        switch (item.itemType)
        {
            case ItemType.Weapon:
                type = "����"; break;
            case ItemType.ConsumableItem:
                type = "��������Ʒ"; break;
        }
        this.icon.sprite = item.icon;
        this.name.text = item.name;
        this.type.text = type;
        this.item = item;
    }

    public void OnClick()
    {
        InventoryUI.Instance.OnItemClick(item,this);
    }
}
