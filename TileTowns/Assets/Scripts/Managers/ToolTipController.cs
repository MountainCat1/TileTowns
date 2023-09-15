using TMPro;
using UnityEngine;
using Zenject;

public interface IToolTipController
{
    void SetTooltip(object sender, TooltipData tooltipData);
    void RemoveTooltip(object sender);
}

public class ToolTipController : MonoBehaviour, IToolTipController
{
    [Inject] private IInputManager _inputManager;

    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    
    private object _sender;

    private void Start()
    {
        _inputManager.PointerMoved += UpdateTooltipPosition;
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


    public void SetTooltip(object sender, TooltipData tooltipData)
    {
        _sender = sender;
        
        titleText.text = tooltipData.Title;
        contentText.text = tooltipData.Content;
        
        ShowTooltipPanel();
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
        panelTransform.gameObject.SetActive(true);
    }
}

[System.Serializable]
public struct TooltipData
{
    public string Title { get; set; }
    public string Content { get; set; }
}