using UnityEngine;

public class WorkCardCleaner : MonoBehaviour
{
    [Header("Target")]
    public PlayerHand playerHand;

    [Header("Settings")]
    public int maxCards = 10;

    // 카드 수 체크 후 오래된 카드 정리
    public void CheckAndClear()
    {
        if (playerHand == null) return;

        Transform parent = playerHand.transform;

        if (parent.childCount < maxCards) return;

        // 가장 최근 카드 하나는 남김
        for (int i = 0; i < parent.childCount - 1; i++)
        {
            Transform card = parent.GetChild(i);
            Destroy(card.gameObject);
        }
    }

    // 강제 마지막 1장만 남기기 (선택용)
    public void KeepOnlyLast()
    {
        if (playerHand == null) return;

        Transform parent = playerHand.transform;

        for (int i = 0; i < parent.childCount - 1; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}