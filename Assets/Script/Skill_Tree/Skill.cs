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
        DescriptionText.text = $"{skillTree.SkillDescriptions[id]}\nCost : {skillTree.SkillPoint}/1 SP";
        
        // 2. 이미지 색상 변경
        GetComponent<Image>().color = skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ? Color.yellow
            : skillTree.SkillPoint >= 1 ? Color.green : Color.white;

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

    public void Buy()
    {
        if (skillTree.SkillPoint < 1 || skillTree.SkillLevels[id] >= skillTree.SkillCaps[id]) return;
        skillTree.SkillPoint -= 1;
        skillTree.SkillLevels[id]++;
        skillTree.UpdateAllSkillUI();
    }
}