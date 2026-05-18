using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeCamera : MonoBehaviour, IDragHandler, IScrollHandler, IPointerClickHandler
{
    [Header("움직일 대상")]
    [SerializeField] private RectTransform[] targets;

    [Header("줌")]
    [SerializeField] private float zoomSpeed = 0.01f;
    [SerializeField] private float minZoom   = 0.4f;
    [SerializeField] private float maxZoom   = 2.5f;

    [Header("이동 범위 제한")]
    [SerializeField] private float limitLeft  = 600f;
    [SerializeField] private float limitRight = 600f;
    [SerializeField] private float limitUp    = 200f;
    [SerializeField] private float limitDown  = 300f;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData e)
    {
        if (targets == null) return;
        float sf = _canvas != null ? _canvas.scaleFactor : 1f;

        foreach (var t in targets)
        {
            if (t == null) continue;
            Vector2 newPos = t.anchoredPosition + e.delta / sf;
            newPos.x = Mathf.Clamp(newPos.x, -limitLeft, limitRight);
            newPos.y = Mathf.Clamp(newPos.y, -limitDown, limitUp);
            t.anchoredPosition = newPos;
        }
    }

    public void OnScroll(PointerEventData e)
    {
        if (targets == null || targets.Length == 0) return;

        RectTransform main = targets[0];
        if (main == null) return;

        float cur  = main.localScale.x;
        float next = Mathf.Clamp(cur + e.scrollDelta.y * zoomSpeed, minZoom, maxZoom);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            main, e.position, e.pressEventCamera, out localPos);

        foreach (var t in targets)
        {
            if (t == null) continue;
            t.localScale       = new Vector3(next, next, 1f);
            t.anchoredPosition -= localPos * (next - cur);

            Vector2 pos = t.anchoredPosition;
            pos.x = Mathf.Clamp(pos.x, -limitLeft, limitRight);
            pos.y = Mathf.Clamp(pos.y, -limitDown, limitUp);
            t.anchoredPosition = pos;
        }
    }

    public void OnPointerClick(PointerEventData e) { }
}