using UnityEngine;
using System.Collections.Generic;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    [Header("skill data")]
    public int[] SkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    [Header("오브젝트 연결")]
    public List<Skill> SkillList = new List<Skill>();
    public GameObject SkillHolder;

    public List<GameObject> ConnectorList = new List<GameObject>();
    public GameObject ConnectorHolder;

    private void Start()
    {
        SkillCaps        = new[] { 1, 3, 3, 3, 5, 5, 5, 5, 10, 10, 10, 10 };
        SkillNames       = new[] { "기본 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5", "Upgrade 6", "Upgrade 7", "Upgrade 8", "Upgrade 9", "Upgrade 10", "Upgrade 11", "Upgrade 12" };
        SkillDescriptions = new[] { "설명1", "설명2", "설명3", "설명4", "설명5", "설명6", "설명7", "설명8", "설명9", "설명10", "설명11", "설명12" };

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