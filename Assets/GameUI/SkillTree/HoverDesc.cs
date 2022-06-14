using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDesc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string descToShow;               // Reference to the description text.
    private float timeToWait = 0.33f;       // Float to save the time in which the description will appear.
    
    /// <summary>
    /// Calls methods when the mouse starts hovering over certain UI elements.
    /// </summary>
    /// <param name="eventData">Gets mouse events</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    /// <summary>
    /// Calls methods when the mouse stops hovering over certain UI elements.
    /// </summary>
    /// <param name="eventData">Gets mouse events</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverManager.onMouseLoseFocus();
    }

    /// <summary>
    /// Shows the description text on the mouse position when called.
    /// </summary>
    private void ShowMessage()
    {
        HoverManager.onMouseHover(descToShow, Input.mousePosition);
    }

    /// <summary>
    /// Calls the method to show the description text after set amount of time.
    /// </summary>
    /// <returns>Returns after set amount of time</returns>
    private IEnumerator StartTimer()
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        ShowMessage();
    }
}
