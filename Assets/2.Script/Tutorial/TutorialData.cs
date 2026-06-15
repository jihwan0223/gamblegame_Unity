/// <summary>
/// 튜토리얼 메시지 모음.
/// 필요한 곳에서 TutorialManager.instance.ShowTutorial() 호출.
/// </summary>
public static class TutorialData
{
    // ── 게임씬 첫 진입 ──────────────────────────────────────────────────────
    public static string[] GameRulesKR = new[]
    {
        "블랙잭에 오신 걸 환영합니다!",
        "목표: 딜러보다 21에 가까운 숫자를 만드세요.",
        "숫자 카드는 그대로, J/Q/K는 10, A는 1 또는 11입니다.",
        "HIT: 카드를 한 장 더 받습니다.",
        "STAY: 현재 패로 승부합니다.",
        "21을 초과하면 버스트! 자동으로 패배합니다.",
        "베팅 버튼으로 금액을 선택한 뒤 BET 버튼을 누르세요.",
        "올인 버튼을 누르면 전 재산을 베팅합니다!",
    };

    public static string[] GameRulesEN = new[]
    {
        "Welcome to Blackjack!",
        "Goal: Get closer to 21 than the dealer without going over.",
        "Number cards = face value, J/Q/K = 10, A = 1 or 11.",
        "HIT: Draw another card.",
        "STAY: Stand with your current hand.",
        "Bust over 21 and you lose automatically.",
        "Select your bet amount then press the BET button.",
        "Press ALL IN to bet everything you have!",
    };

    // ── 한 판 후 - 다른 씬 안내 ─────────────────────────────────────────────
    public static string[] AfterFirstGameKR = new[]
    {
        "첫 판이 끝났습니다!",
        "로비로 돌아가면 다양한 기능을 이용할 수 있습니다.",
        "알바 씬: 카드를 뽑아 돈을 벌 수 있습니다.",
        "업그레이드 씬: 스킬 트리로 능력을 강화하세요.",
        "컴퓨터 씬: 은행에서 돈을 관리할 수 있습니다.",
    };

    public static string[] AfterFirstGameEN = new[]
    {
        "First game complete!",
        "Return to the lobby to access more features.",
        "Work Scene: Draw cards to earn money.",
        "Upgrade Scene: Strengthen your abilities with the skill tree.",
        "Computer Scene: Manage your money at the bank.",
    };
}