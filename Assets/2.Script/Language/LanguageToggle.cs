using Lean.Localization;
using UnityEngine;

public class LanguageToggle : MonoBehaviour
{
    private bool _isKorean = false;

    public void Toggle()
    {
        _isKorean = !_isKorean;
        LeanLocalization.SetCurrentLanguageAll(_isKorean ? "Korean" : "English");
    }
}