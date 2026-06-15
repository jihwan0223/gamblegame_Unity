using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TutorialPanel에 붙입니다.
/// GameManager에서 ShowTutorial() 호출.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("UI")]
    public TMP_Text tutorialText;
    public TMP_Text skipText;
    public CanvasGroup canvasGroup;

    [Header("설정")]
    public float typeSpeed = 0.04f;  // 타이핑 속도
    public float fadeDuration = 0.3f;

    private string[] _messages;
    private int _currentIndex = 0;
    private bool _isTyping = false;
    private bool _isActive = false;
    private Coroutine _typeCoroutine;

    private void Awake()
    {
        instance = this;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isActive) return;

        if (Input.GetMouseButtonDown(0) || 
        Input.GetKeyDown(KeyCode.Space) || 
        Input.GetKeyDown(KeyCode.Escape))
        {
            NextMessage();
        }
    }
    private void NextMessage()
    {
        if (_isTyping)
        {
            // 타이핑 중이면 즉시 완성
            if (_typeCoroutine != null) StopCoroutine(_typeCoroutine);
            tutorialText.text = _messages[_currentIndex];
            _isTyping = false;
        }
        else
        {
            // 다음 메시지로
            _currentIndex++;
            if (_currentIndex >= _messages.Length)
            {
                HideTutorial();
                return;
            }
            _typeCoroutine = StartCoroutine(TypeMessage(_messages[_currentIndex]));
        }
    }

    /// <summary>튜토리얼 시작</summary>
    public void ShowTutorial(string[] messages)
    {
        _messages     = messages;
        _currentIndex = 0;
        _isActive     = true;

        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        skipText.text = LanguageToggle.Instance._isKorean ? "건너뛰기" : "Skip";

        StartCoroutine(FadeIn());
    }

    /// <summary>건너뛰기 버튼 OnClick() 연결</summary>
    public void Skip()
    {
        HideTutorial();
    }

    private void HideTutorial()
    {
        _isActive = false;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed          += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        if (_typeCoroutine != null) StopCoroutine(_typeCoroutine);
        _typeCoroutine = StartCoroutine(TypeMessage(_messages[_currentIndex]));
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed           += Time.deltaTime;
            canvasGroup.alpha  = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private IEnumerator TypeMessage(string message)
    {
        _isTyping         = true;
        tutorialText.text = "";

        foreach (char c in message)
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        _isTyping = false;
    }
}