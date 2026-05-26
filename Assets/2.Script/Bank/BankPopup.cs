using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BankPopup 패널 오브젝트에 붙입니다.
/// 은행 버튼에서 Open(), 닫기 버튼에서 Close() 호출.
/// </summary>
public class BankPopup : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text feedbackText;

    [Header("입력")]
    [SerializeField] private TMP_InputField amountInput;

    [Header("버튼")]
    [SerializeField] private Button InMoneyButton;  // 입금
    [SerializeField] private Button OutMoneyButton; // 출금
    [SerializeField] private Button closeButton;

    [Header("애니메이션")]
    [SerializeField] private float animDuration = 0.25f;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();

        InMoneyButton.onClick.AddListener(OnDeposit);
        OutMoneyButton.onClick.AddListener(OnWithdraw);
        closeButton.onClick.AddListener(Close);

        gameObject.SetActive(false);
    }

    // ── 열기/닫기 ────────────────────────────────────────────────────────────

    public void Open()
    {
        gameObject.SetActive(true);
        UpdateUI();
        StopAllCoroutines();
        StartCoroutine(PopIn());
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(PopOut());
    }

    private IEnumerator PopIn()
    {
        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animDuration);
            // 살짝 오버슈트
            float scale = t < 0.7f
                ? Mathf.Lerp(0f, 1.1f, t / 0.7f)
                : Mathf.Lerp(1.1f, 1f, (t - 0.7f) / 0.3f);
            _rect.localScale = Vector3.one * scale;
            yield return null;
        }
        _rect.localScale = Vector3.one;
    }

    private IEnumerator PopOut()
    {
        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animDuration);
            _rect.localScale = Vector3.one * (1f - t);
            yield return null;
        }
        _rect.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    // ── 입금/출금 ────────────────────────────────────────────────────────────

    private void OnDeposit()
    {
        if (!TryParseAmount(out int amount)) return;
        bool success = BankManager.instance.Deposit(amount);
        SetFeedback(success ? $"{amount}$ 입금 완료" : "입금 실패: 금액을 확인하세요", success);
        amountInput.text = "";
        UpdateUI();
    }

    private void OnWithdraw()
    {
        if (!TryParseAmount(out int amount)) return;
        bool success = BankManager.instance.Withdraw(amount);
        SetFeedback(success ? $"{amount}$ 출금 완료" : "출금 실패: 잔액을 확인하세요", success);
        amountInput.text = "";
        UpdateUI();
    }

    // ── 헬퍼 ─────────────────────────────────────────────────────────────────

    private bool TryParseAmount(out int amount)
    {
        if (!int.TryParse(amountInput.text, out amount) || amount <= 0)
        {
            SetFeedback("올바른 금액을 입력하세요", false);
            return false;
        }
        return true;
    }

    private void UpdateUI()
    {
        Debug.Log($"moneyText: {moneyText}, balanceText: {balanceText}");
        if (BankManager.instance == null) return;
        if (moneyText != null)   moneyText.text   = $"보유 금액: {BankManager.instance.GetMoney()}$";
        if (balanceText != null) balanceText.text = $"은행 잔액: {BankManager.instance.GetBalance()}$";
        if (feedbackText != null) feedbackText.text = "";
    }
    private void SetFeedback(string message, bool success)
    {
        if (feedbackText == null) return;
        feedbackText.text  = message;
        feedbackText.color = success ? Color.green : Color.red;
    }
}