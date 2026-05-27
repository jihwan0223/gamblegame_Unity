using UnityEngine;
using UnityEngine.UI;

public class BankButton : MonoBehaviour
{
    [SerializeField] private BankPopup bankPopup;

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(() => bankPopup.Open());
    }
}