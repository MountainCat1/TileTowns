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
        [Inject] private IGameProgressAccessor _gameProgressAccessor;


        private void Start()
        {
            var progress = _gameProgressAccessor.Progress;
            
            foreach (var level in _levelSet.LevelConfigs)
            {
                // ReSharper disable once Unity.NoNullPropagation
                Debug.Log($"Loading level ({level?.LevelName})...");
                
                var entryGo = _container.InstantiatePrefab(levelEntryUIPrefab, levelEntryContainer);

                var entry = entryGo.GetComponent<LevelEntryUI>();
                
                var unlocked = progress.level + 1 > _levelSet.LevelConfigs.IndexOf(level);
                
                entry.Initialize(level, unlocked);
                
                entry.OnSelected += OnLevelSelected;
            }
        }

        private void OnLevelSelected(LevelConfig level)
        {
            _levelManager.LoadLevel(level);
        }
    }
}