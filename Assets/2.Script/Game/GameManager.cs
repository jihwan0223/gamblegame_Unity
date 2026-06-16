using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

public class GameManager : MonoBehaviour
{
    public PlayerHand playerHand;
    public PlayerHand dealerHand;
    public DealerAI dealerAI;
    public DeckManager deckManager;

    [Header("Manager")]
    public BettingManager bettingManager;
    public GameController gameController;

    [Header("게임 시작시 나타나는 버튼")]
    public Button upgradeButton;
    public Button lobbyButton;
    public Button allInButton;

    [Header("게임 종료 후 나타나는 버튼")]
    public Button nextGameButton;

    [Header("Next Card Button")]
    public GameObject nextCardButton;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("tutorial_game"))
        {
            PlayerPrefs.SetInt("tutorial_game", 1);
            var msgs = LanguageToggle.Instance._isKorean
                ? TutorialData.GameRulesKR
                : TutorialData.GameRulesEN;
            TutorialManager.instance.ShowTutorial(msgs);
        }

        nextGameButton.onClick.AddListener(ResetBoardForNextRound);
        upgradeButton.gameObject.SetActive(true);
        lobbyButton.gameObject.SetActive(true);
        nextGameButton.gameObject.SetActive(false);

        // 버튼 초기 비활성화는 GameController에서 관리
        gameController.hitButton.interactable = false;
        gameController.stayButton.interactable = true; 
    }

    public void StartGame()
    {
        lobbyButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        nextCardButton.gameObject.SetActive(false);

        gameController.SetButtonsInteractable(true);

        StartDealerHand();
        bettingManager.HideBettingUI();
    }

    private void StartDealerHand()
    {
        dealerHand.isDealerTurnStarted = false;
        dealerHand.AddCard(deckManager.DrawCard());
        dealerHand.AddCard(deckManager.DrawCard());
    }

    public void OnGameEnd()
    {
        if (!PlayerPrefs.HasKey("tutorial_after"))
        {
            PlayerPrefs.SetInt("tutorial_after", 1);
            var msgs = LanguageToggle.Instance._isKorean
                ? TutorialData.AfterFirstGameKR
                : TutorialData.AfterFirstGameEN;
            TutorialManager.instance.ShowTutorial(msgs);
        }

        gameController.SetButtonsInteractable(false);

        if (nextGameButton != null) nextGameButton.gameObject.SetActive(true);
        if (upgradeButton != null)  upgradeButton.gameObject.SetActive(true);
        if (lobbyButton != null)    lobbyButton.gameObject.SetActive(true);
    }

    public void ResetBoardForNextRound()
    {
        playerHand.ClearHand();
        dealerHand.ClearHand();
        bettingManager.ShowToggleButton();

        if (deckManager != null) deckManager.ResetDeck();

        bettingManager.ResetBet();

        allInButton.gameObject.SetActive(true);
        lobbyButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
        nextGameButton.gameObject.SetActive(false);

        gameController.SetButtonsInteractable(false);

        if (LanguageToggle.Instance._isKorean)
            dealerAI.speechText.text = LeanLocalization.GetTranslationText("다음턴! 행운을 빕니다!");
        else
            dealerAI.speechText.text = LeanLocalization.GetTranslationText("Next Turn, Good Luck!");

        dealerAI.LWinText.text = "";

        gameController.hitButton.interactable = false;
        gameController.stayButton.interactable = true;

        bettingManager.ShowBettingUI();
    }
}