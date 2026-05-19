using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ScreenClickArea에 붙입니다.
/// 클릭 시 컴퓨터 화면이 서서히 전체화면으로 확대됩니다.
/// </summary>
public class ComputerZoom : MonoBehaviour, IPointerClickHandler
{
    [Header("연결")]
    [SerializeField] private CanvasGroup bgCanvasGroup;      // 배경 이미지 CanvasGroup
    [SerializeField] private GameObject  fullscreenPanel;    // 전체화면 패널

    [Header("애니메이션")]
    [SerializeField] private float zoomDuration = 1.5f;

    private RectTransform _rect;
    private Vector2       _originalPos;
    private Vector2       _originalSize;
    private bool          _isZoomed    = false;
    private bool          _isAnimating = false;

    private void Awake()
    {
        _rect         = GetComponent<RectTransform>();
        _originalPos  = _rect.anchoredPosition;
        _originalSize = _rect.sizeDelta;

        if (fullscreenPanel != null)
            fullscreenPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isZoomed && !_isAnimating)
            StartCoroutine(ZoomIn());
    }

    public void ZoomOut()
    {
        if (!_isAnimating)
            StartCoroutine(ZoomOutRoutine());
    }

    private IEnumerator ZoomIn()
    {
        _isAnimating = true;

        // 캔버스 크기 가져오기
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 targetSize = canvasRect.sizeDelta;
        Vector2 targetPos  = Vector2.zero;

        Vector2 startSize = _rect.sizeDelta;
        Vector2 startPos  = _rect.anchoredPosition;
        float startBgAlpha = bgCanvasGroup != null ? bgCanvasGroup.alpha : 1f;

        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t  = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);

            _rect.sizeDelta        = Vector2.Lerp(startSize, targetSize, t);
            _rect.anchoredPosition = Vector2.Lerp(startPos,  targetPos,  t);

            if (bgCanvasGroup != null)
                bgCanvasGroup.alpha = Mathf.Lerp(startBgAlpha, 0f, t);

            yield return null;
        }

        _rect.sizeDelta        = targetSize;
        _rect.anchoredPosition = targetPos;
        if (bgCanvasGroup != null) bgCanvasGroup.alpha = 0f;

        if (fullscreenPanel != null)
            fullscreenPanel.SetActive(true);

        _isZoomed    = true;
        _isAnimating = false;
    }

    private IEnumerator ZoomOutRoutine()
    {
        _isAnimating = true;

        if (fullscreenPanel != null)
            fullscreenPanel.SetActive(false);

        Vector2 startSize = _rect.sizeDelta;
        Vector2 startPos  = _rect.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t  = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);

            _rect.sizeDelta        = Vector2.Lerp(startSize,    _originalSize, t);
            _rect.anchoredPosition = Vector2.Lerp(startPos,     _originalPos,  t);

            if (bgCanvasGroup != null)
                bgCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        _rect.sizeDelta        = _originalSize;
        _rect.anchoredPosition = _originalPos;
        if (bgCanvasGroup != null) bgCanvasGroup.alpha = 1f;

        _isZoomed    = false;
        _isAnimating = false;
    }
}