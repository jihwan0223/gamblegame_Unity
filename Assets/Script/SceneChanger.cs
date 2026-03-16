using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // 입력받은 씬 이동
        SceneManager.LoadScene(sceneName);
    }

    // 게임 종료 기능
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}