using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 빈 오브젝트에 붙입니다.
/// Inspector에서 BGM 클립 4개 연결.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] bgmClips;   // BGM 4개
    [SerializeField] private AudioMixerGroup mixerGroup;

    private AudioSource _source;
    private int         _lastIndex = -1;

    private static BGMManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _source             = GetComponent<AudioSource>();
        _source.loop        = false;
    }

    private void Start()
    {
        PlayNext();
    }

    private void Update()
    {
        // 곡 끝나면 다음 곡 재생
        if (!_source.isPlaying)
            PlayNext();
    }

    private void PlayNext()
    {
        if (bgmClips == null || bgmClips.Length == 0) return;

        // 같은 곡 연속 재생 방지
        int index;
        do { index = Random.Range(0, bgmClips.Length); }
        while (index == _lastIndex && bgmClips.Length > 1);

        _lastIndex   = index;
        _source.clip = bgmClips[index];
        _source.Play();
    }
}