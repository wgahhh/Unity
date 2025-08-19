using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance { get; private set; }
    private TextMeshProUGUI message;

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
        message = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Hide();
    }

    private void Update()
    {
        if (message.enabled)
        {
            Color color = message.color;
            float alpha = Mathf.Lerp(color.a, 0, Time.deltaTime);
            message.color = new Color(color.r,color.g,color.b,alpha);

            if (alpha == 0)
            {
                Hide();
            }
        }
    }

    public void Show(string message)
    {
        this.message.text = message;
        this.message.color = Color.white;
        this.message.enabled = true;
    }

    public void Hide()
    {
        this.message.enabled = false;
    }
}
