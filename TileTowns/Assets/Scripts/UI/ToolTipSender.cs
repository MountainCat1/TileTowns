using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ToolTipSender : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Events

    public event Action PointerEntered;
    public event Action PointerExited;

    #endregion

    [Inject] private IToolTipController _toolTipController;

    public Func<TooltipData> TooltipDataProvider { get; set; }
    
    // This method is called when the mouse pointer enters the GameObject
    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTipController.SetTooltipProvider(this, TooltipDataProvider);
        PointerEntered?.Invoke();
    }

    // This method is called when the mouse pointer exits the GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTipController.RemoveTooltip(this);
        PointerExited?.Invoke();
    }
}