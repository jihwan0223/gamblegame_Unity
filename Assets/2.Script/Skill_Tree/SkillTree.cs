using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Plugins;
using System;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    [Header("skill data")]
    public int[] SkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    [Header("한 / 영")]
    private string[] SkillNamesEN;
    private string[] SkillNamesKR;
    
    private string[] SkillDescriptionsEN;
    private string[] SkillDescriptionsKR;

    [Header("오브젝트 연결")]
    public List<Skill> SkillList = new List<Skill>();
    public GameObject SkillHolder;

    public List<GameObject> ConnectorList = new List<GameObject>();
    public GameObject ConnectorHolder;

    private void Start()
    {
        SkillCaps = new[] { 1, 3, 3, 3, 5, 5, 1, 5, 5, 1, 1, 1 };
        SkillNamesEN = new[] { "Upgrade!", "Loss Reduction", "Win Reward Boost", "Blackjack Bonus", "Work Boost", "Upgrade Discount", "Next Card Reveal", "All In Bonus", "Double Reward Chance", "Loss Refund", "???", "Jackpot Chance" };
        SkillNamesKR = new[] { "업그레이드!","패배 손실 감소", "승리 보상 증가", "블랙잭 보너스", "알바 수입 증가", "업그레이드 할인", "다음 카드 확인", "올인 보너스", "보상 2배 확률", "패배 환급", "???", "잭팟 확률" };

        SkillDescriptionsKR = new[] { "업그레이드를 해금합니다", "패배 손실이 n%만큼 감소합니다", "승리 보상이 n%만큼 증가합니다", "블랙잭 보상이 n%만큼 증가합니다", "알바 수입이 n%만큼 증가합니다", "업그레이드 비용이 n%만큼 감소합니다", "덱의 다음 카드를 확인합니다", "올인 보상이 n%만큼 증가합니다", "보상 2배 확률이 n%만큼 증가합니다", "패배 금액을 모두 돌려받습니다", "???", "잭팟 확률이 n%만큼 증가합니다" };

        SkillDescriptionsEN = new[] { "Unlocks upgrades", "Reduces defeat loss by n%", "Increases win reward by n%", "Increases Blackjack reward by n%", "Increases work income by n%", "Reduces upgrade cost by n%", "Reveals the next card in the deck", "Increases all-in reward by n%", "Increases double reward chance by n%", "Receive all the lost money back.", "???", "Increases jackpot chance by n%" };
        
        // 번역
        SkillNames = LanguageToggle.Instance._isKorean ? SkillNamesKR : SkillNamesEN;
        SkillDescriptions = LanguageToggle.Instance._isKorean ? SkillDescriptionsKR : SkillDescriptionsEN;

        if (DataManager.instance != null && DataManager.instance.gameData != null)
            SkillLevels = DataManager.instance.gameData.skillLevels;

        // 비활성화 오브젝트도 포함해서 수집
        SkillList.Clear();
        Skill[] foundSkills = SkillHolder.GetComponentsInChildren<Skill>(true);
        for (int i = 0; i < foundSkills.Length; i++)
        {
            if (i < 12)
            {
                foundSkills[i].id = i;
                SkillList.Add(foundSkills[i]);
            }
            else foundSkills[i].gameObject.SetActive(false);
        }

        SetSkillConnections();
        UpdateAllSkillUI();
    }

    private void SetSkillConnections()
{
    if (SkillList.Count < 12) return;
    SkillList[0].ConnectedSkills = new[] { 1, 2, 3 };
    SkillList[1].ConnectedSkills = new[] { 4, 5 };
    SkillList[2].ConnectedSkills = new[] { 6 };
    SkillList[3].ConnectedSkills = new[] { 7, 8 };
    SkillList[4].ConnectedSkills = new[] { 9 };
    SkillList[5].ConnectedSkills = new[] { 9 };  // 4,5 둘다 필요
    SkillList[6].ConnectedSkills = new[] { 10 };
    SkillList[7].ConnectedSkills = new[] { 11 };
    SkillList[8].ConnectedSkills = new[] { 11 }; // 7,8 둘다 필요
}

    public void UpdateAllSkillUI()
    {
        foreach (var skill in SkillList)
        {
            if (skill == null) continue;

            // 잠깐 켜서 UpdateUI 호출 후 다시 끄기
            bool wasActive = skill.gameObject.activeSelf;
            skill.gameObject.SetActive(true);
            skill.UpdateUI();

            if (skill.id != 0)
            {
                bool shouldBeActive = IsSkillUnlocked(skill.id);
                skill.gameObject.SetActive(shouldBeActive);
            }
        }
    }

    public bool IsSkillUnlocked(int skillId)
    {
        List<int> parents = new List<int>();
        foreach (var skill in SkillList)
        {
            if (skill == null || skill.ConnectedSkills == null) continue;
            foreach (int connected in skill.ConnectedSkills)
            {
                if (connected == skillId)
                    parents.Add(skill.id);
            }
        }

        if (parents.Count == 0) return false;

        foreach (int parentId in parents)
        {
            if (DataManager.instance.gameData.skillLevels[parentId] <= 0)
                return false;
        }
        return true;
    }
}