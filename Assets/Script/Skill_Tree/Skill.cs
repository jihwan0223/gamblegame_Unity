using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;

public class Skill : MonoBehaviour
{
    public int id;

    public TMP_Text TItleText;
    public TMP_Text DescriptionText;
    
    public int[] ConnectedUpgrades;

    public void UpdateUI()
    {
        TItleText.text = $"{skillTree.SkillLevels[id]}/{skillTree.SkillCaps[id]}\n{skillTree.SkillNames[id]}";
        DescriptionText.text = $"{skillTree.SkillDescriptions[id]}\nCost : {skillTree.SkillPoint}/1 SP";

        GetComponent<Image>().color = skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ? Color.yellow
            : skillTree.SkillPoint >= 1 ? Color.green : Color.white;
    }
}
