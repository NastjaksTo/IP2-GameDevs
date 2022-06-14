using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public TextMeshProUGUI descText;                        // Reference to the description text, which pops up while hovering.
    public RectTransform descWindow;                        // Reference to the description background, which pops up while hovering.

    public static Action<string, Vector2> onMouseHover;     // Action to dynamically call multiple functions when mouse is hovering. Param: String saves the message to display, Vector2 saves the mouse position.
    public static Action onMouseLoseFocus;                  // Action to dynamically call multiple functions when mouse stops hovering.

    /// <summary>
    /// When mouse is hovering subscribe to both actions, which allows to call Actions.
    /// </summary>
    private void OnEnable()
    {
        onMouseHover += ShowDesc;
        onMouseLoseFocus += HideDesc;
    }

    /// <summary>
    /// When mouse stops hovering unsubscribe to both actions.
    /// </summary>
    private void OnDisable()
    {
        onMouseHover -= ShowDesc;
        onMouseLoseFocus -= HideDesc;
    }

    /// <summary>
    /// Hides the description at the start of the game.
    /// </summary>
    void Start()
    {
        HideDesc();
    }

    /// <summary>
    /// Activates the description when hovering over certain UI elements.
    /// </summary>
    /// <param name="desc">Sets the description text to display.</param>
    /// <param name="mousePos">Sets the mouse position for the display to show up.</param>
    private void ShowDesc(string desc, Vector2 mousePos)
    {
        descText.text = desc;
        descWindow.sizeDelta = new Vector2(descText.preferredWidth > 350 ? 350 : descText.preferredWidth, descText.preferredHeight);

        descWindow.gameObject.SetActive(true);
        descWindow.transform.position = new Vector2(mousePos.x + descWindow.sizeDelta.x * 1.25f, mousePos.y);
    }

    /// <summary>
    /// Hides the description text and the description window.
    /// </summary>
    private void HideDesc()
    {
        descText.text = default;
        descWindow.gameObject.SetActive(false);
    }
}
