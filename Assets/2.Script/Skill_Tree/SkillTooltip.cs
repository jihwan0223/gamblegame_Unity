using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// SkillTooltip 패널에 붙입니다.
/// </summary>
public class SkillTooltip : MonoBehaviour
{
    public Skill hoveredSkill { get; set; } = null;

    public static SkillTooltip instance;

    [SerializeField] private TMP_Text tooltipText;
    [SerializeField] private float animDuration = 0.15f;

    private RectTransform _rect;
    private Coroutine _anim;

    private void Awake()
    {
        instance = this;
        _rect    = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

     public void Show(string message)
    {
        gameObject.SetActive(true);
        tooltipText.text = message;

        if (_anim != null) StopCoroutine(_anim);
        _anim = StartCoroutine(PopIn());
    }

    public void Hide()
    {
        if (_anim != null) StopCoroutine(_anim);
        gameObject.SetActive(false);
    }

    private IEnumerator PopIn()
    {
        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t     = Mathf.SmoothStep(0f, 1f, elapsed / animDuration);
            float scale = t < 0.7f
                ? Mathf.Lerp(0f, 1.05f, t / 0.7f)
                : Mathf.Lerp(1.05f, 1f, (t - 0.7f) / 0.3f);
            _rect.localScale = Vector3.one * scale;
            yield return null;
        }
        _rect.localScale = Vector3.one;
    }
}