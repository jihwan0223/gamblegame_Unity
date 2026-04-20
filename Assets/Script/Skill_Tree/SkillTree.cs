using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillTree : MonoBehaviour
{
    // Singleton
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    [Header("skill data")]
    public int[] SkillLevels;                // 각 스킬 레벨
    public int[] SkillCaps;                 // 각 스킬 최대 레벨
    public string[] SkillNames;             // 스킬 이름
    public string[] SkillDescriptions;      // 스킬 설명
    
    [Header("오브젝트 연결")]
    public List<Skill> SkillList;       // Imagees 연결
    public GameObject SkillHolder;      // SkillHolder 연결

    public List<GameObject> ConnectorList;
    public GameObject ConnectorHolder;

    public int skillPoint;      // 유저 스킬 포인트

    private void Start(){
        SkillLevels = DataManager.instance.gameData.skillLevels;
        skillPoint = DataManager.instance.gameData.skillPoint;
           //스킬 포인트 ***************** 테스트용 ********************************
        skillPoint = 100;

        SkillLevels = new int[12];
        SkillCaps = new[] { 1, 3, 3, 3, 5, 5, 5, 5, 10, 10, 10, 10};

        SkillNames = new[] {"Upgrade 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5", "Upgrade 6",
        "Upgrade 7", "Upgrade 8", "Upgrade 9", "Upgrade 10", "Upgrade 11", "Upgrade12"};
        SkillDescriptions = new[]
        {
            "기능 1 설명",
            "기능 2 설명",
            "기능 3 설명",
            "기능 4 설명",
            "기능 5 설명",
            "기능 6 설명",
            "기능 7 설명",
            "기능 8 설명",
            "기능 9 설명",
            "기능 10 설명",
            "기능 11 설명",
            "기능 12 설명"
        };
        
        foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>()) 
        {
            SkillList.Add(skill);
        }

        foreach (RectTransform connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) 
        {
            if (connector.gameObject != ConnectorHolder)
            {
                ConnectorList.Add(connector.gameObject);
            }
        }

        for (var i = 0; i < SkillList.Count; i++) SkillList[i].id = i;

        // 스킬트리 연결
        SkillList[0].ConnectedSkills = new[] {1, 2, 3};     //0번 배우면 1,2,3 나옴
        SkillList[1].ConnectedSkills = new[] {4, 5};
        SkillList[2].ConnectedSkills = new[] {6};
        SkillList[3].ConnectedSkills = new[] {7, 8};

        SkillList[4].ConnectedSkills = new[] {9};
        SkillList[5].ConnectedSkills = new[] {9};

        SkillList[6].ConnectedSkills = new[] {10};

        SkillList[7].ConnectedSkills = new[] {11};
        SkillList[8].ConnectedSkills = new[] {11};


        UpdateAllSkillUI();
    }

    public void UpdateAllSkillUI()
    {
        foreach (var skill in SkillList) skill.UpdateUI();
    }
}
