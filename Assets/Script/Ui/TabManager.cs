using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [Header("Tab Content")]
    public GameObject[] contentPages;     
    public TextMeshProUGUI titleText;      
    public string[] pageTitleNames;        

    [Header("Tab Buttons")]
    public Image[] tabButtonImages;       
    public Color activeColor = Color.white;
    public Color idleColor = Color.gray;

    // 탭 버튼(사운드, 그래픽 등)에 연결할 함수
    public void ChangePage(int index) // 이름을 GoToPage에서 ChangePage로 변경
    {
        if (index < 0 || index >= contentPages.Length) return;

        for (int i = 0; i < contentPages.Length; i++)
        {
            contentPages[i].SetActive(i == index);
            if (tabButtonImages != null && i < tabButtonImages.Length)
            {
                tabButtonImages[i].color = (i == index) ? activeColor : idleColor;
            }
        }

        if (titleText != null && index < pageTitleNames.Length)
            titleText.text = pageTitleNames[index];
    }
}