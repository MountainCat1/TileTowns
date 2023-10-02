using System;
using Data;
using UnityEngine;
using Zenject;

namespace UI
{
    public class LevelSelectorUI : MonoBehaviour
    {
        [SerializeField] private LevelEntryUI levelEntryUIPrefab;
        [SerializeField] private Transform levelEntryContainer;

        [Inject] private DiContainer _container;
        [Inject] private ILevelManager _levelManager;
        [Inject] private ILevelSet _levelSet;


        private void Start()
        {
            foreach (var level in _levelSet.LevelConfigs)
            {
                // ReSharper disable once Unity.NoNullPropagation
                Debug.Log($"Loading level ({level?.LevelName})...");
                
                var entryGo = _container.InstantiatePrefab(levelEntryUIPrefab, levelEntryContainer);

                var entry = entryGo.GetComponent<LevelEntryUI>();
                
                entry.Initialize(level);
                
                entry.OnSelected += OnLevelSelected;
            }
        }

        private void OnLevelSelected(LevelConfig level)
        {
            _levelManager.LoadLevel(level);
        }
    }
}