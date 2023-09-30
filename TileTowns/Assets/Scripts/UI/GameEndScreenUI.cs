using UnityEngine;
using Zenject;

public class GameEndScreenUI : MonoBehaviour
{
    [Inject] private IGameManager _gameManager;
    [Inject] private ILevelManager _levelManager;

    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    private void Start()
    {
        _gameManager.LevelEnded += OnLevelEnded;
    }

    private void OnLevelEnded(IGameResult result)
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
        _levelManager.LoadMainMenu();
    }

    public void Retry()
    {
        _gameManager.Restart();
    }
    
    public void Continue()
    {
        _gameManager.Restart();
    }
}