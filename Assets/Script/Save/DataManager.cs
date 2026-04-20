using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameData gameData;

    string path;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        path = Path.Combine(Application.persistentDataPath, "SaveData.json");
        LoadGameData();
    }

    // 저장
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(path, json);
        Debug.Log("저장 완료: " + path);
    }

    // 불러오기
    public void LoadGameData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("데이터 로드 완료");
        }
        else
        {
            gameData = new GameData(); // 파일 없으면 새로 생성
            SaveGameData();
        }
    }
}
