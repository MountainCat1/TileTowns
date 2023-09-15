using System;
using TMPro;
using UnityEngine;
using Zenject;

public interface IToolTipController
{
    void SetTooltipProvider(object sender, Func<TooltipData> tooltipDataProvider);
    void RemoveTooltip(object sender);
}

public class ToolTipController : MonoBehaviour, IToolTipController
{
    [Inject] private IInputManager _inputManager;
    [Inject] private IGameState _gameState;

    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    
    private object _sender;
    private Func<TooltipData> _tooltipDataProvider;

    private void Start()
    {
        _inputManager.PointerMoved += UpdateTooltipPosition;
        
        HideTooltipPanel();

        _gameState.MutationChanged += UpdateTooltipData;
        _gameState.Changed += UpdateTooltipData;
    }

    private void UpdateTooltipPosition(Vector2 position)
    {
        // ReSharper disable once PossibleLossOfFraction
        if (position.y > Screen.height / 2)
            panelTransform.pivot = new Vector2(0, 1);
        else
            panelTransform.pivot = new Vector2(0, 0);

        panelTransform.transform.position = position;
    }


    public void SetTooltipProvider(object sender, Func<TooltipData> tooltipDataProvider)
    {
        _sender = sender;
        _tooltipDataProvider = tooltipDataProvider;

        UpdateTooltipData();
        
        ShowTooltipPanel();
    }

    private void UpdateTooltipData()
    {
        var tooltipData = _tooltipDataProvider?.Invoke() ?? new TooltipData();
        
        titleText.text = tooltipData.Title;
        contentText.text = tooltipData.Content;
    }

    public void RemoveTooltip(object sender)
    {
        if(sender != _sender)
            return;
        
        HideTooltipPanel();
    }

    private void ShowTooltipPanel()
    {
        panelTransform.gameObject.SetActive(true);
    }

    private void HideTooltipPanel()
    {
        panelTransform.gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct TooltipData
{
    public string Title { get; set; }
    public string Content { get; set; }
}