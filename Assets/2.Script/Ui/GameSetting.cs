using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // 그래픽 품질 (0: Low, 1: Medium, 2: High 등)
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // 전체화면 토글
    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    // 볼륨 조절 (0.0 ~ 1.0)
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}