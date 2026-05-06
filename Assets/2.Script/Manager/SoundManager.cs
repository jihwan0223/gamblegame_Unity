using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<SoundData> soundList;

    private Dictionary<string, AudioClip> soundDict;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        soundDict = new Dictionary<string, AudioClip>();

        foreach (var s in soundList)
        {
            if (!soundDict.ContainsKey(s.name))
                soundDict.Add(s.name, s.clip);
        }
    }

    public void Play(string soundName)
    {
        if (soundDict.TryGetValue(soundName, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"{soundName} 없음");
        }
    }

    // Sound
    public void DrawCard()
    {
        Play("CardDrawSound");
    }

    public void OnClickButton()
    {
        Play("ClickSound");
    }

    public void PlayerWin()
    {
        Play("MoneySound");
    }

    public void PlayerLose()
    {
        Play("LoseSound");
    }

    public void PlayerDraw()
    {
        //Play("LoseSound");
    }
}