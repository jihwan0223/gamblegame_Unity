using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 엔딩 씬 오브젝트에 붙입니다.
/// 씬 구조:
///   Canvas
///   ├── FadePanel (검은 Image, 전체 채움) ← CanvasGroup 추가
///   └── VideoPlayer 오브젝트 ← VideoPlayer 컴포넌트
/// </summary>
public class EndingScene : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private CanvasGroup fadePanel;   // 검은 페이드 패널
    [SerializeField] private VideoPlayer videoPlayer; // 영상 플레이어

    [Header("페이드")]
    [SerializeField] private float fadeDuration = 3f;

    private void Start()
    {
        fadePanel.alpha = 1f;
        videoPlayer.playOnAwake = false;
        videoPlayer.Stop();

        StartCoroutine(FadeInThenPlay());
    }

    private IEnumerator FadeInThenPlay()
    {
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);

        // 영상이랑 페이드 동시 시작
        videoPlayer.Play();

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, Mathf.SmoothStep(0f, 1f, elapsed / fadeDuration));
            yield return null;
        }
        fadePanel.alpha = 0f;
    }
}