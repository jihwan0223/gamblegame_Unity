using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    [Header("Graphic Setting")]
    public GameObject settingPopup;
    public GameObject mainMenu;
    public TabManager tabManager;

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        Debug.Log("그래픽 변경: " + index);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
        Debug.Log("전체화면 설정: " + isFull);
    }

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