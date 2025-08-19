using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    private GameObject uiGameObject;
    private GameObject content;
    public GameObject itemPrefab;
    private bool isShow = false;
    public ItemDetailUI itemDetailUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiGameObject = transform.Find("UI").gameObject;
        content = transform.Find("UI/ListBg/Scroll View/Viewport/Content").gameObject;
        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isShow)
            {
                Hide();
                isShow = false;
            }
            else
            {
                Show();
                isShow=true;
            }
        }
    }

    public void Show()
    {
        uiGameObject.SetActive(true);
    }

    public void Hide()
    {
        uiGameObject.SetActive(false);
    }

    public void AddItem(ItemSO item)
    {
        GameObject itemGo = GameObject.Instantiate(itemPrefab);
        itemGo.transform.SetParent(content.transform);
        ItemUI itemUI = itemGo.GetComponent<ItemUI>();
        
        itemUI.InitItem(item);
    }

    public void OnItemClick(ItemSO item,ItemUI itemUI )
    {
        itemDetailUI.UpdateItemDetail(item,itemUI);
    }

    public void OnItemUse(ItemSO item,ItemUI itemUI)
    {
        Destroy(itemUI.gameObject);
        InventoryManager.Instance.RemoveItem(item);

        GameObject.FindGameObjectWithTag(Tag.PLAYER).GetComponent<Player>().UseItem(item);
    }
}
