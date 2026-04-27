using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    [Header("UI 설정")]
    public GameObject settingPopup;
    public GameObject mainMenu;
    public TabManager tabManager;

    // 해상도 설정
    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0: //4K
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
            
            case 1: //FHD
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;

            case 2: //HD
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
        }
        Debug.Log("해상도 변경: " + index);
    }

    // 그래픽 설정
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("그래픽 변경: " + qualityIndex);
    }

    
    // 전체화면 설정
    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
        Debug.Log("전체화면 설정: " + isFull);
    }

    // 안티앨리어싱 
    public void SetAntiAliasing(bool isOn)
    {
        QualitySettings.antiAliasing = isOn ? 4 : 0;
        Debug.Log("안티앨리어싱: " + (isOn ? "ON(4x)" : "OFF"));
    }

    // 프레임
    public void SetFrameRate(int index)
    {
        switch (index)
        {
            case 0: Application.targetFrameRate = -1; break;
            case 1: Application.targetFrameRate = 30; break;
            case 2: Application.targetFrameRate = 60; break;
            case 3: Application.targetFrameRate = 144; break;
        }
        Debug.Log("프레임 제한: " + Application.targetFrameRate);
    }


    // 설정창 
    public void Open()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }

        if (settingPopup != null)
        {
            settingPopup.SetActive(true);
        }

        if (tabManager != null)
        {
            tabManager.ChangePage(0);
        }

        Debug.Log("설정창 열림");
    }

    public void Close()
    {
        if (settingPopup != null)
        {
            settingPopup.SetActive(false);
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }

        Debug.Log("설정창이 닫힘");
    }
}