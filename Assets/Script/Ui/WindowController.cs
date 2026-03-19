using UnityEngine;

public class WindowController : MonoBehaviour
{
    [Header("Target Window")]
    public GameObject targetWindow; // 띄울 창 오브젝트

    // 버튼에 연결할 함수
    public void OpenWindow()
    {
        if (targetWindow != null)
        {
            targetWindow.SetActive(true); // 창 켜기
            
            // 필요하다면 여기서 시간 정지나 효과음 재생 가능
            // Time.timeScale = 0; 
        }
    }

    // 닫기 버튼에 연결할 함수
    public void CloseWindow()
    {
        if (targetWindow != null)
        {
            targetWindow.SetActive(false); // 창 끄기
            // Time.timeScale = 1;
        }
    }
}