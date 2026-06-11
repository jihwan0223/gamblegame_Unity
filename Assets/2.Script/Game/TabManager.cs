using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [Header("Buttons")]
    public Image[] tabButtons;      // 탭 버튼들의 Image 컴포넌트
    public Color activeColor = Color.white;
    public Color idleColor = Color.gray;

    [Header("Contents")]
    public GameObject[] pages;      // 버튼 클릭 시 교체될 페이지(패널)들

    public void ChangePage(int index)
    {
        if (index < 0 || index >= pages.Length) return;

        for (int i = 0; i < pages.Length; i++)
        {
            // 1. 페이지 활성화 제어
            pages[i].SetActive(i == index);

            // 2. 버튼 색상 변경 (버튼 자체는 꺼지지 않음)
            if (i < tabButtons.Length)
            {
                tabButtons[i].color = (i == index) ? activeColor : idleColor;
            }
        }
    }
}