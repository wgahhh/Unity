using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI contentText;
    private Button continueButton;

    private int contentIndex = 0;
    private List<string> contentList;

    private GameObject uiGameObject;

    private Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        uiGameObject = transform.Find("UI").gameObject;
        Hide();
        nameText = transform.Find("UI/NameTextBg/NameText").GetComponent<TextMeshProUGUI>();
        contentText = transform.Find("UI/ContentText").GetComponent<TextMeshProUGUI>();
        continueButton = transform.Find("UI/ContinueButton").GetComponent<Button>();
        continueButton.onClick.AddListener(this.OnContinueButtonClick);
    }
    public void Show()
    {
        uiGameObject.SetActive(true);
    }

    public void Show(string name, string[] content,Action OnDialogueEnd = null)
    {
        nameText.text = name;
        contentList = new List<string>();
        contentList.AddRange(content);
        contentText.text = contentList[0];
        contentIndex = 0;
        Show();
        this.OnDialogueEnd = OnDialogueEnd;
    }

    public void Hide()
    {
        uiGameObject.SetActive(false); 
    }

    private void OnContinueButtonClick()
    {
        contentIndex++;
        if (contentIndex >= contentList.Count)
        {
            OnDialogueEnd?.Invoke();
            Hide();
            return;
        }
        contentText.text = contentList[contentIndex];
    }
}
