using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// 볼륨 슬라이더 오브젝트에 붙입니다.
/// Inspector에서 AudioMixer와 Slider 연결.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider bgmSlider;

    private const string BGM_KEY = "BGMVolume";

    private void Start()
    {
        float saved = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        bgmSlider.value = saved;
        SetBGMVolume(saved);

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void SetBGMVolume(float value)
    {
        // Slider 0~1 값을 dB로 변환 (-80dB ~ 0dB)
        float dB = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        mixer.SetFloat(BGM_KEY, dB);
        PlayerPrefs.SetFloat(BGM_KEY, value);
    }
}