using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 클릭 유도할 오브젝트에 붙입니다.
/// Image 컴포넌트가 같은 오브젝트에 있어야 합니다.
/// 플레이어가 클릭하면 자동으로 꺼집니다.
/// </summary>
[RequireComponent(typeof(Image))]
public class ClickHint : MonoBehaviour
{
    [SerializeField] private float speed = 1f;   // 깜빡이는 속도

    [SerializeField] private float minAlpha = 0f;   // 최소 AP
    [SerializeField] private float maxAlpha = 1f;   //최대 AP

    private Image _image;
    private bool  _active = true;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _active = true;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (_active)
        {
            // 알파값 0 → 1 → 0 반복
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.Abs(Mathf.Sin(Time.time * speed)));
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
            yield return null;
        }
    }

    /// <summary>클릭됐을 때 호출. 버튼 OnClick()에 연결하거나 코드에서 호출.</summary>
    public void Hide()
    {
        _active = false;
        gameObject.SetActive(false);
    }
}