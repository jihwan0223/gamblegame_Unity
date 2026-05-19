using UnityEngine;
using UnityEngine.UI;

public class ComputerCloseButton : MonoBehaviour
{
    [SerializeField] private ComputerZoom computerZoom;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => computerZoom.ZoomOut());
    }
}