using System.IO;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameSettingsAccessor
    {
        public GameSettings Settings => GetSettings();

        private GameSettings _gameSettings;

        [Inject]
        void Construct()
        {
        }

        private GameSettings GetSettings()
        {
            // If game settings have already been loaded, return them
            if (_gameSettings is not null)
                return _gameSettings;

            // Otherwise, try to load them from the JSON file
            _gameSettings = LoadObjectFromJson<GameSettings>("GameSettings.json");
            
            // If the settings are still null, create a new default instance
            if (_gameSettings is null)
            {
                _gameSettings = new GameSettings();
                SaveObjectToJson(_gameSettings, "GameSettings.json");
            }

            return _gameSettings;
        }


        public void SaveObjectToJson<T>(T myObject, string fileName)
        {
            // Get the path to the AppData folder
            string appDataPath = Application.persistentDataPath;

            // Combine the AppData path with the file name to get the full file path
            string filePath = Path.Combine(appDataPath, fileName);

            // Convert the object to JSON
            string json = JsonUtility.ToJson(myObject);

            // Write the JSON data to the file in the AppData folder
            File.WriteAllText(filePath, json);
        }

        // Load the object from JSON
        public T LoadObjectFromJson<T>(string fileName) where T : class
        {
            // Get the path to the AppData folder
            string appDataPath = Application.persistentDataPath;

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
}