using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;

public class Skill : MonoBehaviour
{
    public int id;
    public TMP_Text TItleText;
    public TMP_Text DescriptionText;        
    public int[] ConnectedSkills;

    [Header("이 스킬을 배우면 나타날 선들")]
    public GameObject[] MyLines;
    public int upgradeCost = 500;

    public void UpdateUI()
    {   
        // 핵심: NullReferenceException 방지
        if (skillTree == null || DataManager.instance == null || DataManager.instance.gameData == null) return;
        if (id < 0 || id >= skillTree.SkillCaps.Length) return;

        int currentLevel = DataManager.instance.gameData.skillLevels[id];
        int maxCap = skillTree.SkillCaps[id];

        TItleText.text = $"{currentLevel}/{maxCap}\n{skillTree.SkillNames[id]}";
        DescriptionText.text = $"{skillTree.SkillDescriptions[id]}\nMoney : {DataManager.instance.gameData.money}$";
        
        GetComponent<Image>().color = currentLevel >= maxCap ? Color.yellow
            : DataManager.instance.gameData.money >= upgradeCost ? Color.green : Color.white;

        bool isLearned = currentLevel > 0;

        foreach (int connectedSkill in ConnectedSkills)
        {   
            if (connectedSkill < skillTree.SkillList.Count)
                skillTree.SkillList[connectedSkill].gameObject.SetActive(isLearned);
        }

        if (MyLines != null)
        {
            foreach (GameObject line in MyLines)
            {
                if (line != null) line.SetActive(isLearned);
            }
        }
    }

    public void Buy()
    {
        // 1. 안전 장치: skillTree가 없으면 다시 찾아오기
    if (skillTree == null) skillTree = SkillTree.skillTree;

    // 2. 모든 필수 객체가 존재하는지 확인
    if (DataManager.instance == null || DataManager.instance.gameData == null || skillTree == null)
    {
        Debug.LogError("DataManager 또는 SkillTree가 씬에 없습니다!");
        return;
    }

    // 3. 구매 로직 진행
    if (DataManager.instance.gameData.money >= upgradeCost && 
        DataManager.instance.gameData.skillLevels[id] < skillTree.SkillCaps[id])
    {
        // 데이터 수정
        DataManager.instance.gameData.money -= upgradeCost;
        DataManager.instance.gameData.skillLevels[id]++;
        
        // 저장
        DataManager.instance.SaveGameData();
        
        // ★ 화면 갱신 (UpdateAllSkillUI 호출)
        skillTree.UpdateAllSkillUI();
        
        Debug.Log($"{id}번 스킬 업그레이드 완료!");
    }
    else
    {
        Debug.Log("돈이 부족하거나 최대 레벨입니다.");
    }
    }
}