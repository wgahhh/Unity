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
                type = "武器"; break;
            case ItemType.ConsumableItem:
                type = "可消耗物品"; break;
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
