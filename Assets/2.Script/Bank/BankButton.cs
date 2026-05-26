using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 은행 버튼 오브젝트에 붙입니다.
/// </summary>
public class BankButton : MonoBehaviour
{
    [SerializeField] private BankPopup bankPopup;

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(() => bankPopup.Open());
    }
}