using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    [Header("skill data")]
    public int[] SkillLevels;               // 스킬 레벨
    public int[] SkillCaps;                 // 스킬 최대 레벨
    public string[] SkillNames;             // 스킬 이름
    public string[] SkillDescriptions;      // 스킬 설명
    
    [Header("오브젝트 연결")]
    public List<Skill> SkillList = new List<Skill>();
    public GameObject SkillHolder;

    public List<GameObject> ConnectorList = new List<GameObject>();
    public GameObject ConnectorHolder;

    private void Start()
    {
        // 1. 데이터 초기화
        SkillCaps = new[] { 1, 3, 3, 3, 5, 5, 5, 5, 10, 10, 10, 10};
        SkillNames = new[] {"기본 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5", "Upgrade 6", "Upgrade 7", "Upgrade 8", "Upgrade 9", "Upgrade 10", "Upgrade 11", "Upgrade 12"};
        SkillDescriptions = new[] { "설명1", "설명2", "설명3", "설명4", "설명5", "설명6", "설명7", "설명8", "설명9", "설명10", "설명11", "설명12" };

        // 2. DataManager 연결
        if (DataManager.instance != null && DataManager.instance.gameData != null)
        {
            SkillLevels = DataManager.instance.gameData.skillLevels;
        }

        // 3. 스킬 수집 및 ID 부여 (12개 제한)
        SkillList.Clear();
        Skill[] foundSkills = SkillHolder.GetComponentsInChildren<Skill>(true);
        for (int i = 0; i < foundSkills.Length; i++)
        {
            if (i < 12)
            {
                foundSkills[i].id = i; 
                SkillList.Add(foundSkills[i]);
            }
            else foundSkills[i].gameObject.SetActive(false); // 초과분 비활성화
        }

        SetSkillConnections();
        UpdateAllSkillUI();
    }

    private void SetSkillConnections()
    {
        if (SkillList.Count < 12) return;
        SkillList[0].ConnectedSkills = new[] {1, 2, 3};
        SkillList[1].ConnectedSkills = new[] {4, 5};
        SkillList[2].ConnectedSkills = new[] {6};
        SkillList[3].ConnectedSkills = new[] {7, 8};
        SkillList[4].ConnectedSkills = new[] {9};
        SkillList[5].ConnectedSkills = new[] {9};
        SkillList[6].ConnectedSkills = new[] {10};
        SkillList[7].ConnectedSkills = new[] {11};
        SkillList[8].ConnectedSkills = new[] {11};
    }

    public void UpdateAllSkillUI()
    {
        foreach (var skill in SkillList) 
        {
            if(skill != null) skill.UpdateUI();
            else Debug.Log("skill is Null");
        }
    }
}