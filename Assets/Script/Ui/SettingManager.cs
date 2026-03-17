using UnityEngine;
using TMPro; // 텍스트 변경을 위해 필요

public class SettingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingPopup;    // 설정창 전체 (Blocker 포함)
    public GameObject mainMenuPanel;   // 메인 화면 버튼들의 부모 오브젝트

    [Header("Page Settings")]
    public GameObject[] pages;         // 설정 페이지들 (Sound, Game 등)
    public TextMeshProUGUI categoryTitle; // 상단 제목 텍스트
    public string[] pageNames = { "SOUND", "GAMEPLAY", "INFO" }; 

    private int currentPage = 0;

    // [중요] 세팅 버튼을 누를 때 실행할 함수
    public void OpenPopup()
    {
        settingPopup.SetActive(true);    // 설정창 켜기
        mainMenuPanel.SetActive(false); // 메인 버튼들 숨기기
        currentPage = 0;                // 첫 페이지부터 시작
        UpdatePage();
    }

    // X 버튼을 누를 때 실행할 함수
    public void ClosePopup()
    {
        settingPopup.SetActive(false);   // 설정창 끄기
        mainMenuPanel.SetActive(true);  // 메인 버튼들 다시 보이기
    }

    public void NextPage()
    {
        currentPage = (currentPage + 1) % pages.Length;
        UpdatePage();
    }

    public void PreviousPage()
    {
        currentPage--;
        if (currentPage < 0) currentPage = pages.Length - 1;
        UpdatePage();
    }

    void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
        
        if (categoryTitle != null)
            categoryTitle.text = pageNames[currentPage];
    }
}