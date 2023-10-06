using System;
using System.IO;
using ModestTree.Util;
using UnityEngine;

[System.Serializable]
public class GameProgress
{
    public int levelProgress = 0;
}

public interface IGameProgressAccessor
{
    event Action<GameProgress> Changed;

    GameProgress Settings { get; }
    void Update();
    void Save();
}

public class GameProgressAccessor : IGameProgressAccessor
{
    private const string FileName = "GameProgress.json";
    
    public event Action<GameProgress> Changed;

    public GameProgress Settings => GetSettings();

    private static GameProgress _gameProgress;

    private readonly string _settingsFilePath = Application.persistentDataPath;

    private GameProgress GetSettings()
    {
        // If game settings have already been loaded, return them
        if (_gameProgress is not null)
            return _gameProgress;

        // Otherwise, try to load them from the JSON file
        Debug.Log($"Loading game settings from {_settingsFilePath}...");
        _gameProgress = LoadObjectFromJson<GameProgress>(FileName);

        // If the settings are still null, create a new default instance
        if (_gameProgress is null)
        {
            _gameProgress = new GameProgress();
            SaveObjectToJson(_gameProgress, FileName);
        }

        return _gameProgress;
    }


    public void Save()
    {
        SaveObjectToJson(_gameProgress ?? new GameProgress(), FileName);

        Changed?.Invoke(Settings);
    }

    public void Update()
    {
        Changed?.Invoke(Settings);
    }

    private void SaveObjectToJson<T>(T myObject, string fileName)
    {
        // Get the path to the AppData folder
        string appDataPath = _settingsFilePath;

        // Combine the AppData path with the file name to get the full file path
        string filePath = Path.Combine(appDataPath, fileName);

        // Convert the object to JSON
        string json = JsonUtility.ToJson(myObject);

        // Write the JSON data to the file in the AppData folder
        File.WriteAllText(filePath, json);
    }

    // Load the object from JSON
    private T LoadObjectFromJson<T>(string fileName) where T : class
    {
        // Get the path to the AppData folder
        string appDataPath = _settingsFilePath;

        // Combine the AppData path with the file name to get the full file path
        string filePath = Path.Combine(appDataPath, fileName);

        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON data back into the object
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogError("JSON file not found: " + filePath);
            return null;
        }
    }
}