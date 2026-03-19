using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    [Header("Main Containers")]
    public GameObject fullWindowCanvas;   
    public GameObject lobbyUIPanel;       
    
    [Header("References")]
    public TabManager tabManager; 

    public void OpenWindow()
    {
        fullWindowCanvas.SetActive(true);
        lobbyUIPanel.SetActive(false);
        
        // 창 열 때 무조건 0번(사운드) 페이지가 보이게 설정
        if (tabManager != null) 
            tabManager.ChangePage(0); 
    }

    public void CloseWindow()
    {
        fullWindowCanvas.SetActive(false);
        lobbyUIPanel.SetActive(true);
    }
}