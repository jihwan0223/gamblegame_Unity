using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSound;
    public AudioSource sfxSound;

    [Header("Audio Clips")]
    public AudioClip cardDrow;
    public AudioClip getMoney;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Debug.Log("SoundManaget instance maked");
        }
        else
        {
            Debug.LogWarning("SoundManager Error");
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null && sfxSound != null)
        {
            sfxSound.PlayOneShot(clip); 
        }
        else
        {
            Debug.LogWarning("재생할 클립이 없거나 AudioSource가 연결되지 않았습니다.");
        }
    }
}
