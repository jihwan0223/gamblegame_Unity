using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ??? 스킬 버튼(id=10)에 붙이거나 Skill.cs의 Buy()에서 호출.
/// 씬 이름은 Inspector에서 설정.
/// </summary>
public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private string endingSceneName = "Ending";

    /// <summary>Skill.cs Buy() 완료 후 호출</summary>
    public void TriggerEnding()
    {
        SceneManager.LoadScene(endingSceneName);
    }
}