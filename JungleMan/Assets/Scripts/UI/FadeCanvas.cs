using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class FadeCanvas : MonoBehaviour
{
    [Header("�¼�����")]
    public FadeEvnetSO fadeEvnet;


    public Image fadeImage;

    private void OnEnable()
    {
        fadeEvnet.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvnet.OnEventRaised -= OnFadeEvent;

    }

    private void OnFadeEvent(Color target, float duration,bool fadeIn)
    {
        fadeImage.DOBlendableColor(target, duration);

    }
}
