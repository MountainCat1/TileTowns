using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEventSender : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Events

    public event Action PointerEntered;
    public event Action PointerExited;

    #endregion
    
    // This method is called when the mouse pointer enters the GameObject
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEntered?.Invoke();
    }

    // This method is called when the mouse pointer exits the GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExited?.Invoke();
    }
}