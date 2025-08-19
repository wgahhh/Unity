using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI type;
    public TextMeshProUGUI description;
    public GameObject propertyGrid;
    public GameObject propertyTemplate;

    private ItemSO item;
    private ItemUI itemUI;

    private void Start()
    {
        propertyTemplate.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void UpdateItemDetail(ItemSO item,ItemUI itemUI)
    {
        this.gameObject.SetActive (true);

        this.item = item;
        this.itemUI = itemUI;

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
        this.description.text = item.description;

        foreach (Transform child in propertyGrid.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Property property in item.propertyList)
        {
            string propertyStr = "";
            string propertyType = "";
            switch (property.propertyType)
            {
                case PropertyType.HPValue:
                    propertyType = "生命值：";
                    break;
                case PropertyType.EnergyValue:
                    propertyType = "能量值：";
                    break;
                case PropertyType.MentalValue:
                    propertyType = "精神值：";
                    break;
                case PropertyType.SpeedValue:
                    propertyType = "速度值：";
                    break;
                case PropertyType.AttackValue:
                    propertyType = "攻击力：";
                    break;
                default:
                    break;
            }
            propertyStr += propertyType;
            propertyStr += property.value;
            GameObject go = GameObject.Instantiate(propertyTemplate);
            go.SetActive(true); 
            go.transform.SetParent(propertyGrid.transform);
            go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
        }
    }

    public void OnUseButtonClick()
    {
        InventoryUI.Instance.OnItemUse(item,itemUI);
        this.gameObject.SetActive(false);
    }
}
