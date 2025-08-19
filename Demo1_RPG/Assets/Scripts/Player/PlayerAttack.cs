using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Weapon weapon;
    public Sprite weaponIcon;
    void Start()
    {
        
    }

    void Update()
    {
        if (weapon != null && Input.GetMouseButtonDown(0))
        {
            weapon.Attack();
        }
    }

    public void LoadWeapon(Weapon weapon)
    { 
        this.weapon = weapon;
    }

    public void LoadWeapon(ItemSO item)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
            weapon = null;
        }

        string prefabName = item.prefab.name;
        Transform weaponParent = transform.Find(prefabName);
        GameObject weaponGo = GameObject.Instantiate(item.prefab);
        weaponGo.transform.parent = weaponParent;
        weaponGo.transform.localPosition = Vector3.zero;
        weaponGo.transform.localRotation = Quaternion.identity;

        this.weapon = weaponGo.GetComponent<Weapon>();
        this.weaponIcon = item.icon;
    }

    public void UnloadWeapon()
    {
        this.weapon = null;
    }
}
