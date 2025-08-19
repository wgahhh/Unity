using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDBManager : MonoBehaviour
{
    public static ItemDBManager Instance { get; private set; }
    public ItemDBSO itemDB;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemSO GetRandomItem()
    {
        int randomIndex = Random.Range(0,itemDB.itemList.Count);
        return itemDB.itemList[randomIndex];
    }
}
