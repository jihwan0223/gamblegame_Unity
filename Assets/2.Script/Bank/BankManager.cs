using System;
using TMPro;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    public static BankManager instance;

    public string LastError { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // ── 입금 ────────────────────────────────────────────────────────────────
    /// <summary>
    /// 입금 시도. 성공 여부 반환.
    /// 조건: amount > 0, amount <= 보유 금액
    /// </summary>
    public bool Deposit(long amount)
    {
        if (amount <= 0)
        {
            Debug.Log("입금 실패: 0 이하는 입금 불가");

            LastError = LanguageToggle.Instance._isKorean
                ? "입금 실패: 금액은 0보다 커야 합니다"
                : "Deposit Failed: Amount must be greater than 0";

            return false;
        }

        if (amount > DataManager.instance.gameData.money)
        {
            Debug.Log("입금 실패: 보유 금액 초과");

            LastError = LanguageToggle.Instance._isKorean
                ? "입금 실패: 잔액이 부족합니다"
                : "Deposit Failed: Insufficient funds";

            return false;
        }

        DataManager.instance.gameData.money -= amount;
        DataManager.instance.gameData.bankBalance += amount;
        DataManager.instance.SaveGameData();

        Debug.Log($"입금 완료: {amount}$ / 잔액: {DataManager.instance.gameData.bankBalance}$");

        LastError = "";
        return true;
    }

    // ── 출금 ────────────────────────────────────────────────────────────────
    /// <summary>
    /// 출금 시도. 성공 여부 반환.
    /// 조건: amount > 0, amount <= 은행 잔액
    /// </summary>
    public bool Withdraw(long amount)
    {
        if (amount <= 0)
        {
            Debug.Log("출금 실패: 0 이하는 출금 불가");

            LastError = LanguageToggle.Instance._isKorean
                ? "출금 실패: 금액은 0보다 커야 합니다"
                : "Withdrawal Failed: Amount must be greater than 0";

            return false;
        }

        if (amount > DataManager.instance.gameData.bankBalance)
        {
            Debug.Log("출금 실패: 은행 잔액 초과");

            LastError = LanguageToggle.Instance._isKorean
                ? "출금 실패: 잔액이 부족합니다"
                : "Withdrawal Failed: Insufficient bank balance";

            return false;
        }

        DataManager.instance.gameData.bankBalance -= amount;
        DataManager.instance.gameData.money += amount;
        DataManager.instance.SaveGameData();

        Debug.Log($"출금 완료: {amount}$ / 은행 잔액: {DataManager.instance.gameData.bankBalance}$");

        LastError = "";
        return true;
    }

    // ── 이자 (나중에 구현) ──────────────────────────────────────────────────
    /// <summary>
    /// 이자 지급. 추후 이자율/주기 등 구현 예정.
    /// </summary>
    public void AddInterest()
    {
        // TODO: 이자율 적용 로직
    }

    // ── 조회 ────────────────────────────────────────────────────────────────
    public long GetBalance() => DataManager.instance.gameData.bankBalance;

    public long GetMoney()
    {
        return DataManager.instance.gameData.money;
    }
}