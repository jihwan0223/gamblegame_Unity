using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language
    {
        English,
        Korean
    }

    public Language currentLanguage = Language.English;

    private Dictionary<string, string> english = new();
    private Dictionary<string, string> korean = new();

    void Start()
    {
        currentLanguage = Language.Korean;
    }
    void Awake()
    {
        Instance = this;
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csv = Resources.Load<TextAsset>("Localization");

        string[] lines = csv.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');

            if (row.Length < 3) continue;

            string key = row[0].Trim();
            english[key] = row[1].Trim();
            korean[key] = row[2].Trim();
        }
    }

    public string GetText(string key)
    {
        return currentLanguage == Language.Korean
            ? korean[key]
            : english[key];
    }
    
    public void SetKorean()
    {
        currentLanguage = Language.Korean;
        RefreshAll();
    }

    public void SetEnglish()
    {
        currentLanguage = Language.English;
        RefreshAll();
    }

    void RefreshAll()
    {
        var all = FindObjectsByType<LocalizedText>(FindObjectsSortMode.None);

        foreach (var t in all)
        {
            t.UpdateText();
        }
    }
}