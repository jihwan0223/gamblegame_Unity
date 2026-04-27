using UnityEngine;

public class GraphicSetting : MonoBehaviour
{
    // 그래픽 품질 (0: Low, 1: Medium, 2: High)
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // 전체화면 모드 (On: 전체화면, Off: 창모드)
    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    // 프레임 제한 (0: 30fps, 1: 60fps, 2: 무제한)
    public void SetFrameRate(int index)
    {
        int[] fpsLimits = { 30, 60, -1 };
        Application.targetFrameRate = fpsLimits[index];
    }
}