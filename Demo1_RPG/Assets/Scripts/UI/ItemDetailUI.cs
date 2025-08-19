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
                type = "����"; break;
            case ItemType.ConsumableItem:
                type = "��������Ʒ"; break;
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
                    propertyType = "����ֵ��";
                    break;
                case PropertyType.EnergyValue:
                    propertyType = "����ֵ��";
                    break;
                case PropertyType.MentalValue:
                    propertyType = "����ֵ��";
                    break;
                case PropertyType.SpeedValue:
                    propertyType = "�ٶ�ֵ��";
                    break;
                case PropertyType.AttackValue:
                    propertyType = "��������";
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
