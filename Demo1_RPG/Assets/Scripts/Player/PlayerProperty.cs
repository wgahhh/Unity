using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    public Dictionary<PropertyType, List<Property>> propertyDict;
    public int hpValue = 100;
    public int energyValue = 100;
    public int mentalValue = 100;
    public int level = 1;
    public int currentExp = 0;


    // Start is called before the first frame update
    void Awake()
    {
        propertyDict = new Dictionary<PropertyType, List<Property>>();
        propertyDict.Add(PropertyType.SpeedValue, new List<Property>());
        propertyDict.Add(PropertyType.AttackValue, new List<Property>());

        AddProperty(PropertyType.SpeedValue,10);
        AddProperty(PropertyType.AttackValue,20);

        EventCenter.OnEnemyDied += OnEnemyDied;
    }

    public void UseDrug(ItemSO item)
    {
        foreach (Property p in item.propertyList)
        {
            AddProperty(p.propertyType, p.value);
        }
    }

    public void AddProperty(PropertyType pt,int value)
    {
        switch (pt)
        {
            case PropertyType.HPValue:
                hpValue += value;
                hpValue = (hpValue > 100 ? 100 : hpValue);
                return;
            case PropertyType.EnergyValue:
                energyValue += value;
                return;
            case PropertyType.MentalValue:
                mentalValue += value;
                return;
        }

        List<Property> list;
        propertyDict.TryGetValue(pt, out list);
        list.Add(new Property(pt,value));
    }

    public void RemoveProperty(PropertyType pt, int value)
    {
        switch (pt)
        {
            case PropertyType.HPValue:
                hpValue -= value;
                return;
            case PropertyType.EnergyValue:
                energyValue -= value;
                return;
            case PropertyType.MentalValue:
                mentalValue -= value;
                return;
        }

        List<Property> list;
        propertyDict.TryGetValue(pt, out list);
        list.Remove(list.Find(x => x.value == value));
    }

    private void OnDestroy()
    {
        EventCenter.OnEnemyDied -= OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        this.currentExp += enemy.exp;

        while (currentExp >= level * 30)
        {
            currentExp -= level * 30;
            level++;
        }

        PropertyUI.Instance.UpdateProperty();
    }
}
