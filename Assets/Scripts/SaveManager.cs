using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData<T>
{
    public T data;
}

public class SaveManager : MonoBehaviour
{
    //singleton instance
    public static SaveManager Save_instance { get; private set; }

    /*private void Awake()
    {
        if (Save_instance != null)
        {
            Destroy(gameObject);
            Save_instance = null;
        }
        Save_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // saves any game data 
    public void SaveGameData<T>(T dataToSave)
    {
        SaveData<T> saveData = new SaveData<T>();
        saveData.data = dataToSave;
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.persistentDataPath + "/SaveFile.json", json);
        Debug.Log("Data saved to: " + Application.persistentDataPath + "/SaveFile.json");
    }

    //load data 
    public T LoadGameData<T>()
    {
        string path = Application.persistentDataPath + "/SaveFile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData<T> saveData = JsonUtility.FromJson<SaveData<T>>(json);
            if (saveData != null && saveData.data != null)
            {
                return saveData.data;
            }
            else
            {
                Debug.LogError("Failed to deserialize saved data.");
                return default(T);
            }
        }
        else
        {
            Debug.LogError("No saved data found.");
            return default(T);
        }
    }*/
}