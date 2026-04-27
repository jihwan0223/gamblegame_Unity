using UnityEngine;
using TMPro;
public class MoneyUI : MonoBehaviour
{
    [Header("Money Text")]
    public TMP_Text moneyText;

    void Update()
    {
        if(DataManager.instance == null || DataManager.instance.gameData == null)
        {
            return;
        }

        // 실제 데이터에 들어있는 money 값을 표시
        moneyText.text = $"Money : {DataManager.instance.gameData.money.ToString()}$";
    }
}
