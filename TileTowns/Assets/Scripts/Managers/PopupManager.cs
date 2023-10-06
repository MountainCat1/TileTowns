using UnityEngine;

public interface IPopupManager
{
    void SpawnPopup(string text, Vector3 position, Color color);
}

public class PopupManager : MonoBehaviour, IPopupManager
{
    [SerializeField] private Transform popupContainer;
    [SerializeField] private PopupDescriptor popupPrefab;
    
    public void SpawnPopup(string text, Vector3 position, Color color)
    {
        var popup = Instantiate(popupPrefab, popupContainer);
        
        popup.transform.position = position;
        popup.Text.text = text;
        popup.Text.color = color;
    }
}