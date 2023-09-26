using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ILevelManager
{
    void LoadMainMenu();
    void LoadLevel(LevelConfig levelConfig);
}

public class LevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string levelScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        SceneManager.LoadSceneAsync(mainMenuScene).completed += operation =>
        {
            IGameManager gameManager = FindObjectOfType<GameManager>();

            gameManager.LoadLevel(levelConfig);
        };
    }
}