using TMPro;
using UnityEngine;

public class WorkDeck : MonoBehaviour
{
    public int maxCards = 52;
    private int currentCards;

    [Header("돈 Text")]
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        ResetDeck();
    }

    public void OnClickDeck()
    {
        if (currentCards <= 0)
        {
            ResetDeck();
            return;
        }

        if (DataManager.instance == null) return;

        moneyText.text = DataManager.instance.gameData.money.ToString();
        DataManager.instance.SaveGameData();

        currentCards--;

        Debug.Log("+1 money / 남은 카드: " + currentCards);
    }

    public void ResetDeck()
    {
        currentCards = maxCards;
        Debug.Log("새 덱 생성됨");
    }
}