using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public TextMeshProUGUI descText;
    public RectTransform descWindow;

    public static Action<string, Vector2> onMouseHover;
    public static Action onMouseLoseFocus;

    private void OnEnable()
    {
        onMouseHover += ShowDesc;
        onMouseLoseFocus += HideDesc;
    }

    private void OnDisable()
    {
        onMouseHover -= ShowDesc;
        onMouseLoseFocus -= HideDesc;
    }

    void Start()
    {
        HideDesc();
    }

    private void ShowDesc(string desc, Vector2 mousePos)
    {
        descText.text = desc;
        descWindow.sizeDelta = new Vector2(descText.preferredWidth > 350 ? 350 : descText.preferredWidth, descText.preferredHeight);

        descWindow.gameObject.SetActive(true);
        descWindow.transform.position = new Vector2(mousePos.x + descWindow.sizeDelta.x * 1.25f, mousePos.y);
    }

    private void HideDesc()
    {
        descText.text = default;
        descWindow.gameObject.SetActive(false);
    }
}
