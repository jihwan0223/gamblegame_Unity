using UnityEngine;
using TMPro;
public class MoneyUI : MonoBehaviour
{
    [Header("Money Text")]
    public TextMeshProUGUI moneyText;

    void Update()
    {
        // 실제 데이터에 들어있는 money 값을 표시
        moneyText.text = DataManager.instance.gameData.money.ToString("N0") + "$";
    }
}
