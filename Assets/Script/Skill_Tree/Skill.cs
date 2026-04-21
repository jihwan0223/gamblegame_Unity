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
    public GameObject[] MyLines; // 유니티 에디터에서 직접 드래그해서 넣으세요!

    public void UpdateUI()
    {   
        // 1. 텍스트 업데이트
        TItleText.text = $"{skillTree.SkillLevels[id]}/{skillTree.SkillCaps[id]}\n{skillTree.SkillNames[id]}";
        DescriptionText.text = $"{skillTree.SkillDescriptions[id]}\nCost : {skillTree.skillPoint}/1 SP";
        
        // 2. 이미지 색상 변경
        GetComponent<Image>().color = skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ? Color.yellow
            : skillTree.skillPoint >= 1 ? Color.green : Color.white;

        // 내가 이 스킬을 배웠는지 확인
        bool isLearned = skillTree.SkillLevels[id] > 0;

        // 3. 자식 스킬들 활성화/비활성화
        foreach (int connectedSkill in ConnectedSkills)
        {   
            if (connectedSkill < skillTree.SkillList.Count)
                skillTree.SkillList[connectedSkill].gameObject.SetActive(isLearned);
        }

        // 4. 나랑 연결된 선들만 활성화 (인덱스 꼬임 방지)
        if (MyLines != null)
        {
            foreach (GameObject line in MyLines)
            {
                if (line != null) line.SetActive(isLearned);
            }
        }
    }

    public int upgradeCost = 500; // 업그레이드 비용 (레벨마다 늘어나게 할 수도 있음)

public void Buy()
{
    // 현재 잔액이 비용보다 많은지 확인
    if (DataManager.instance.gameData.money >= upgradeCost && 
        skillTree.SkillLevels[id] < skillTree.SkillCaps[id])
        {
            // 1. 돈 차감
            DataManager.instance.gameData.money -= upgradeCost;
            
            // 2. 레벨업
            skillTree.SkillLevels[id]++;
            
            // 3. 저장
            DataManager.instance.SaveGameData();
            
            // 4. UI 갱신
            skillTree.UpdateAllSkillUI();
            Debug.Log("업그레이드 성공!");
        }
        else
        {
            Debug.Log("돈이 부족하거나 이미 마스터했습니다.");
        }
    }
}