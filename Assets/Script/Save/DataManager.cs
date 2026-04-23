using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameData gameData = new GameData();

    string path;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        path = Path.Combine(Application.persistentDataPath, "SaveData.json");
        LoadGameData();
    }

    public void SaveGameData()
    {
        if (gameData == null) return;
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(path, json);
        Debug.Log("저장 완료: " + path);
    }

    public void LoadGameData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (gameData == null) gameData = new GameData();
            JsonUtility.FromJsonOverwrite(json, gameData);
        }

        // 불러온 후에도 배열 크기가 12가 아니면 강제 조정
        if (gameData.skillLevels == null || gameData.skillLevels.Length != 12)
        {
            int[] newLevels = new int[12];
            if (gameData.skillLevels != null)
            {
                System.Array.Copy(gameData.skillLevels, newLevels, Mathf.Min(gameData.skillLevels.Length, 12));
            }
            gameData.skillLevels = newLevels;
        }
    }
}