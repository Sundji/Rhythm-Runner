using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public static class DataReaderWriter
    {
        private static string _highscoreDataPath = Path.Combine(Application.persistentDataPath, "HighscoreData.json");

        public static void DeleteAllData()
        {
            try
            {
                if (File.Exists(_highscoreDataPath))
                {
                    File.Delete(_highscoreDataPath);
                }
            }
            catch (IOException exception)
            {
                Debug.Log("Error while deleting the file.\nError message:\n" + exception.Message);
            }
        }

        public static void SaveHighscoreData(HighscoreData highscoreData)
        {
            try
            {
                string json = JsonUtility.ToJson(highscoreData);
                File.WriteAllText(_highscoreDataPath, json);
            }
            catch (IOException exception)
            {
                Debug.LogError("The file did not save.\nError message:\n" + exception.Message);
            }
        }

        public static bool TryLoadHighscoreData(out HighscoreData highscoreData)
        {
            try
            {
                if (File.Exists(_highscoreDataPath))
                {
                    string json = File.ReadAllText(_highscoreDataPath);
                    highscoreData = JsonUtility.FromJson<HighscoreData>(json);
                }
                else
                {
                    highscoreData = new HighscoreData
                    {
                        HighscoreEntries = new List<HighscoreEntry>()
                    };
                }
                return true;
            }
            catch (IOException exception)
            {
                Debug.LogError("The file did not load.\nError message:\n" + exception.Message);
                highscoreData = null;
                return false;
            }
        }
    }
}