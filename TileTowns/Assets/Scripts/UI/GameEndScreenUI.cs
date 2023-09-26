using UnityEngine;
using Zenject;

public class GameEndScreenUI : MonoBehaviour
{
    [Inject] private IGameManager _gameManager;

    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    private void Start()
    {
        _gameManager.LevelEnded += OnLevelEnded;
    }

    private void OnLevelEnded(WinConditionCheckResult result)
    {
        if (result.Won)
        {
            winMenu.SetActive(true);
        }

        if (result.Lost)
        {
            loseMenu.SetActive(true);
        }
    }

    public void Quit()
    {
        _gameManager.LoadMainMenu();
    }

    public void Retry()
    {
        _gameManager.LoadMainMenu();
    }
    
    public void Continue()
    {
        _gameManager.LoadNextLevel();
    }
}