using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyUI : MonoBehaviour
{
    public static PropertyUI Instance { get; private set; }

    private GameObject uiGameObject;

    private Image hpProgressBar;
    private TextMeshProUGUI hp;

    private Image levelProgressBar;
    private TextMeshProUGUI level;

    private GameObject propertyGrid;
    private GameObject propertyTemplate;
    private Image weaponIcon;

    private PlayerProperty pp;
    private PlayerAttack pa;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiGameObject = transform.Find("UI").gameObject;

        hpProgressBar = transform.Find("UI/HPProgressBar/ProgressBar").GetComponent<Image>();
        hp = transform.Find("UI/HPProgressBar/HP").GetComponent<TextMeshProUGUI>();
        levelProgressBar = transform.Find("UI/LevelProgressBar/ProgressBar").GetComponent<Image>();
        level = transform.Find("UI/LevelProgressBar/Level").GetComponent<TextMeshProUGUI>();

        propertyGrid = transform.Find("UI/PropertyGrid").gameObject;
        propertyTemplate = transform.Find("UI/PropertyGrid/PropertyTemplate").gameObject;
        weaponIcon = transform.Find("UI/WeaponIcon").GetComponent<Image>();

        propertyTemplate.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag(Tag.PLAYER);
        pp = player.GetComponent<PlayerProperty>();
        pa = player.GetComponent<PlayerAttack>();
        UpdateProperty();

        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (uiGameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void UpdateProperty()
    {
        hpProgressBar.fillAmount = pp.hpValue / 100.0f;
        hp.text = pp.hpValue + "/100";

        levelProgressBar.fillAmount = (pp.currentExp * 1.0f) / (pp.level * 30);
        level.text = pp.level.ToString();

        ClearGrid();

        AddProperty("能量值：" + pp.energyValue);
        AddProperty("精神值：" + pp.mentalValue);

        foreach (var item in pp.propertyDict)
        {
            string propertyName = "";
            switch (item.Key)
            {
                case PropertyType.SpeedValue:
                    propertyName = "速度值：";
                    break;
                case PropertyType.AttackValue:
                    propertyName = "攻击力：";
                    break;
                default:
                    break;
            }

            int sum = 0;
            foreach (var item1 in item.Value)
            {
                sum += item1.value;
            }

            AddProperty(propertyName + sum);
        }

        if (pa.weaponIcon != null)
        {
            weaponIcon.sprite = pa.weaponIcon;
        }
    }

    private void ClearGrid()
    {
        foreach (Transform child in propertyGrid.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void AddProperty(string propertyStr)
    {
        GameObject go = GameObject.Instantiate(propertyTemplate);
        go.SetActive(true);
        go.transform.SetParent(propertyGrid.transform);
        go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
    }

    private void Show()
    {
        uiGameObject.SetActive(true);
    }

    private void Hide()
    {
        uiGameObject.SetActive(false);
    }
}
