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
            Debug.Log("위치 : " + Application.persistentDataPath);
        }
        else
        {
            Debug.Log("저장 파일이 없어 새로 생성합니다.");
            Debug.Log("위치 : " + Application.persistentDataPath);
            gameData = new GameData();
            SaveGameData();
        }
    }
    // [데이터 초기화] 테스트용
    [ContextMenu("Reset Save Data")]
    public void ResetData()
    {
        gameData = new GameData();
        SaveGameData();
    }
}
