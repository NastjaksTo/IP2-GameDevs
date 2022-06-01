using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDesc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string descToShow;
    private float timeToWait = 0.33f;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        Debug.Log(descToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverManager.onMouseLoseFocus();
    }

    private void ShowMessage()
    {
        HoverManager.onMouseHover(descToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        
        ShowMessage();

    }
}
