using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MainMenu_UI;

public class MainMenu_UI : MonoBehaviour
{
    [Header("Vars")]
    public string playerName;
    public int currentScore;
    public int[] highScores = new int[3];

    [Header("Highscores")]
    public TMP_Text high1;
    public TMP_Text high2;
    public TMP_Text high3;

    [Header("UI Elements")]
    public GameObject HighScoreMenu;
    public Button playGameButton;
    public Button quitGameButton;
    public Button loadHighscoresButton;
    public TMP_InputField nameInput;

    // Singleton instance of SaveManager
    private static MainMenu_UI instance;
    public static MainMenu_UI Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScoreMenu.SetActive(false);
    }

    public void playGame()
    {
        saveName();
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        saveHighscore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void viewHighscores()
    {
        gameObject.SetActive(false);
        HighScoreMenu.SetActive(true);
        high1.text = highScores[0].ToString();
        high2.text = highScores[1].ToString();
        high3.text = highScores[2].ToString();
    }

    public void highscoreBack()
    {
        HighScoreMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void saveName()
    {
        playerName = nameInput.text;
        SaveGameData(playerName);
    }

    public void loadName()
    {
        playerName = LoadGameData<string>();
        nameInput.text = playerName;
    }

    public void saveScore()
    {
        currentScore = MainManager.Instance.getPoints();
        // Add the current score to the highScores array
        for (int i = 0; i < highScores.Length; i++)
        {
            if (currentScore > highScores[i])
            {
                // Shift the higher scores down
                for (int j = highScores.Length - 1; j > i; j--)
                {
                    highScores[j] = highScores[j - 1];
                }

                // adds the current score at the right place
                highScores[i] = currentScore;
                break;
            }
        }

        // saves the new array 
        SaveGameData(highScores);
    }

    public void loadScore()
    {
        // Load the high scores from the highScores array
        highScores = LoadGameData<int[]>();
    }

    public void saveHighscore()
    {
        // Create an instance of SaveData<int[]> and set its data field
        SaveData<int[]> saveData = new SaveData<int[]>();
        saveData.data = highScores;

        // Save the data
        SaveGameData(saveData);
    }

    public void loadHighscore()
    {
        // Load the data and cast it to SaveData<int[]>
        SaveData<int[]> saveData = LoadGameData<SaveData<int[]>>();

        // Check if saveData is not null and has data
        if (saveData != null && saveData.data != null)
        {
            // Assign loaded data to highScores
            highScores = saveData.data;
        }
        else
        {
            Debug.LogError("Failed to load highscore data.");
        }
    }


    // Save game data
    public void SaveGameData<T>(T dataToSave)
    {
        SaveData<T> saveData = new SaveData<T>();
        saveData.data = dataToSave;
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.persistentDataPath + "/SaveFile.json", json);
        Debug.Log("Data saved to: " + Application.persistentDataPath + "/SaveFile.json");
    }

    // Load game data
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
    }

    [Serializable]
    public class SaveData<T>
    {
        public T data;
    }
}
