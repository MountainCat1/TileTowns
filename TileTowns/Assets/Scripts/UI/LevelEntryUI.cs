using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEntryUI : MonoBehaviour
{
    #region Events

    public event Action<LevelConfig> OnSelected;

    #endregion

    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private TextMeshProUGUI levelDescriptionText;
    [SerializeField] private Image levelImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Button button;
    [SerializeField] private Color lockedColor;

    private LevelConfig _levelConfig;

    public void Initialize(LevelConfig levelConfig, bool unlocked)
    {
        _levelConfig = levelConfig;
        
        button.interactable = unlocked;
        
        levelNameText.text = levelConfig.LevelName;
        levelDescriptionText.text = levelConfig.LevelDescription;
        levelImage.sprite = levelConfig.Thumbnail;
        
        lockImage.gameObject.SetActive(!unlocked);
        if(!unlocked)
            levelImage.color = lockedColor;
    }

    public void Select()
    {
        OnSelected?.Invoke(_levelConfig);
    }
}    