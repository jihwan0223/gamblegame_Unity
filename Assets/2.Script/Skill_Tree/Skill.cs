using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public TMP_Text TItleText;
    public TMP_Text DescriptionText;
    public int[] ConnectedSkills;

    [Header("이 스킬을 배우면 나타날 선들")]
    public GameObject[] MyLines;

    [Header("상태별 스프라이트")]
    [SerializeField] private Sprite normalSprite;  // 돈 부족
    [SerializeField] private Sprite affordSprite;  // 구매 가능
    [SerializeField] private Sprite maxSprite;     // 최대 레벨

    [Header("업그레이드 비용")]
    public int baseCost = 500;          // 기본 가격 (Inspector에서 설정)
    public float costMultiplier = 1.5f; // 레벨당 배율

    private Image _image;

    private void Awake()
    {
        if (_image == null) _image = GetComponent<Image>();
        Debug.Log($"{id}번 _image 오브젝트 이름: {_image?.gameObject.name}");
    }

    public void UpdateUI()
    {
        int currentLevel = DataManager.instance.gameData.skillLevels[id];
        int maxCap       = skillTree.SkillCaps[id];
        int cost         = currentLevel >= maxCap ? 0 : GetUpgradeCost();

        TItleText.text       = $"{currentLevel}/{maxCap}\n{skillTree.SkillNames[id]}";
        DescriptionText.text = currentLevel >= maxCap ? "MAX" : $"Cost : {cost}$";

        // 스프라이트 교체
        if (_image != null)
        {
            if (currentLevel >= maxCap)
            {
                _image.sprite = maxSprite;
                Debug.Log($"{id}번 sprite 변경됨: {_image.sprite.name}");
            }
            else if (DataManager.instance.gameData.money >= baseCost) _image.sprite = affordSprite;
            else _image.sprite = normalSprite;
        }
        else
        {
            Debug.Log($"{id}번 스킬 _image가 null");
        }

        // 선 활성화 (SetActive는 SkillTree.cs가 담당, 선만 여기서 처리)
        bool isLearned = currentLevel > 0;
        if (MyLines != null)
        {
            foreach (GameObject line in MyLines)
            {
                if (line != null) line.SetActive(isLearned);
            }
        }

        // 커서 올린 상태면 툴팁 실시간 갱신
        if (SkillTooltip.instance.hoveredSkill == this)
        ShowTooltip();
    }

    // Upgarde
    private int GetUpgradeCost()
    {
        int currentLevel = DataManager.instance.gameData.skillLevels[id];
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
    }

    public void Buy()
    {
        if (id == 10) SceneManager.LoadScene("EndingScene");

        // 스킬트리가 없으면 찾기
        // if (skillTree == null) skillTree = SkillTree.skillTree;

        if (DataManager.instance == null || DataManager.instance.gameData == null || skillTree == null)
        {
            Debug.LogError("DataManager 또는 SkillTree가 씬에 없습니다!");
            return;
        }

        int cost = GetUpgradeCost();
        if (DataManager.instance.gameData.money >= cost && DataManager.instance.gameData.skillLevels[id] < skillTree.SkillCaps[id])
        {
            DataManager.instance.gameData.money -= cost;
            DataManager.instance.gameData.skillLevels[id]++;
            DataManager.instance.SaveGameData();
            skillTree.UpdateAllSkillUI();
            Debug.Log($"{id}번 스킬 업그레이드 완료!");
        }
        else
        {
            Debug.Log("돈이 부족하거나 최대 레벨입니다.");
        }

        skillTree.UpdateAllSkillUI();
    }

    private void ShowTooltip()
    {
        int currentLevel = DataManager.instance.gameData.skillLevels[id];
        int maxCap       = skillTree.SkillCaps[id];
        int nextLevel    = Mathf.Min(currentLevel + 1, maxCap);

        float currentEffect = currentLevel * 5f;
        float nextEffect    = nextLevel * 5f;

        string currentDesc = skillTree.SkillDescriptions[id].Replace("n", currentEffect.ToString());
        string nextDesc    = skillTree.SkillDescriptions[id].Replace("n", nextEffect.ToString());

        string tooltip;
        if (currentLevel <= 0)
        {
            // 아직 업글 안한 경우
            tooltip = $"{skillTree.SkillNames[id]}\n{nextDesc}";
        }
        else if (currentLevel >= maxCap)
        {
            // 만렙
            tooltip = $"{skillTree.SkillNames[id]}\n{currentDesc}";
        }
        else
        {
            // 업글 가능 → 현재 → 다음 비교
            tooltip = $"{skillTree.SkillNames[id]}\n{currentDesc}\n→ {nextDesc}";
        }

        SkillTooltip.instance.Show(tooltip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTooltip.instance.hoveredSkill = this;
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTooltip.instance.hoveredSkill = null;
        SkillTooltip.instance.Hide();
    }
}