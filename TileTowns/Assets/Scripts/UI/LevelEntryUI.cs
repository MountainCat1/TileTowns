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

    private LevelConfig _levelConfig;

    public void Initialize(LevelConfig levelConfig)
    {
        _levelConfig = levelConfig;
        
        levelNameText.text = levelConfig.LevelName;
        levelDescriptionText.text = levelConfig.LevelDescription;
        levelImage.sprite = levelConfig.LevelImage;
    }

    public void Select()
    {
        OnSelected?.Invoke(_levelConfig);
    }
}    