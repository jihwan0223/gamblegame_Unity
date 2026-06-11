using UnityEngine;

/// <summary>
/// 업그레이드 효과 관리. 게임 어디서든 UpgradeManager.instance로 접근.
/// 각 효과는 레벨당 5% 증가 (effectPerLevel로 조절 가능)
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] private float effectPerLevel = 0.05f; // 레벨당 5%

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // 레벨 가져오기 헬퍼
    private int Level(int id) => DataManager.instance.gameData.skillLevels[id];

    // ── 각 업그레이드 효과 ──────────────────────────────────────────────────

    /// <summary>1번 - 패배 손실 감소 (0.1 = 10% 감소)</summary>
    public float LossReduction      => Level(1) * effectPerLevel;

    /// <summary>2번 - 승리 보상 증가 (0.1 = 10% 증가)</summary>
    public float WinRewardBoost     => Level(2) * effectPerLevel;

    /// <summary>3번 - 블랙잭 보상 증가 (0.1 = 10% 증가)</summary>
    public float BlackjackBonus     => Level(3) * effectPerLevel;

    /// <summary>4번 - 알바 수입 증가 (0.1 = 10% 증가)</summary>
    public float WorkBoost          => Level(4) * effectPerLevel;

    /// <summary>5번 - 업그레이드 비용 감소 (0.1 = 10% 감소)</summary>
    public float UpgradeCostReduce  => Level(5) * effectPerLevel;

    /// <summary>7번 - 올인 보너스 (0.1 = 10% 추가)</summary>
    public float AllInBonus         => Level(7) * effectPerLevel;

    /// <summary>8번 - 보상 2배 확률 (0.1 = 10% 확률)</summary>
    public float DoubleRewardChance => Level(8) * effectPerLevel;

    /// <summary>9번 - 패배 금액 환급 (0.1 = 10% 환급)</summary>
    public float LossRefund         => Level(9) * effectPerLevel;

    /// <summary>11번 - 잭팟 확률 증가 (0.1 = 10% 증가)</summary>
    public float JackpotChance      => Level(11) * effectPerLevel;

    // ── 실제 계산 헬퍼 ──────────────────────────────────────────────────────

    /// <summary>패배 시 실제 잃는 금액 계산</summary>
    public long CalcLoss(long amount)
    {
        // 9번 스킬 1레벨 이상이면 손실 없음
        if (Level(9) >= 1) return 0;
        return (long)(amount * (1f - LossReduction));
    }

    /// <summary>승리 시 실제 받는 금액 계산</summary>
    public long CalcWinReward(long amount)
    {
        // 보상 2배 확률 체크
        if (Random.value < DoubleRewardChance)
            return (long)(amount * 2f * (1f + WinRewardBoost));
        return (long)(amount * (1f + WinRewardBoost));
    }

    /// <summary>블랙잭 보상 계산</summary>
    public long CalcBlackjackReward(long amount)
    {
        return (long)(amount * (1f + BlackjackBonus));
    }

    /// <summary>알바 수입 계산</summary>
    public long CalcWorkIncome(long baseIncome)
    {
        return (long)(baseIncome * (1f + WorkBoost));
    }

    /// <summary>업그레이드 비용 계산 (Skill.cs의 GetUpgradeCost()에서 호출)</summary>
    public int CalcUpgradeCost(int baseCost)
    {
        return Mathf.RoundToInt(baseCost * (1f - UpgradeCostReduce));
    }

    /// <summary>올인 보너스 적용 금액 계산</summary>
    public long CalcAllInBonus(long amount)
    {
        return (long)(amount * (1f + AllInBonus));
    }

    /// <summary>패배 환급 금액 계산</summary>
    public long CalcLossRefund(long lostAmount)
    {
        return (long)(lostAmount * LossRefund);
    }

    /// <summary>잭팟 발생 여부 체크</summary>
    public bool IsJackpot(float baseChance)
    {
        return Random.value < baseChance + JackpotChance;
    }
}