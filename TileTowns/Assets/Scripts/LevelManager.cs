using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public interface ILevelManager
{
    void LoadMainMenu();
    void LoadLevel(LevelConfig levelConfig);
}

public class LevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string levelScene;

    private ZenjectSceneLoader _sceneLoader;
    
    [Inject]
    public void Construct(ZenjectSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    
    public void LoadMainMenu()
    {
        _sceneLoader.LoadScene(mainMenuScene);
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        SceneManager.LoadSceneAsync(levelScene).completed += operation =>
        {
            var gamemManager = FindObjectsOfType<GameManager>();
            
            gamemManager[0].LoadLevel(levelConfig);
        };
    }
}