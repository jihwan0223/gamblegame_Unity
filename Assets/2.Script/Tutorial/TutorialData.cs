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
        "베팅 버튼으로 금액을 선택한 뒤 배팅 닫기 버튼을 누르면 게임이 시작됩니다.",
        "올인 버튼을 누르면 전 재산을 베팅합니다!",
        "+/- 버튼으로 베팅 금액을 조절할 수 있습니다.",
    };

    public static string[] GameRulesEN = new[]
    {
        "Welcome to Blackjack!",
        "Goal: Get closer to 21 than the dealer without going over.",
        "Number cards = face value, J/Q/K = 10, A = 1 or 11.",
        "HIT: Draw another card.",
        "STAY: Stand with your current hand.",
        "Bust over 21 and you lose automatically.",
        "Select your bet using +/- buttons, then close the bet panel to start.",
        "Press ALL IN to bet everything you have!",
        "Use +/- buttons to fine-tune your bet amount.",
    };

    // ── 한 판 후 - 다른 씬 안내 ─────────────────────────────────────────────
    public static string[] AfterFirstGameKR = new[]
    {
        "첫 판이 끝났습니다!",
        "로비로 돌아가면 다양한 기능을 이용할 수 있습니다.",
        "알바 씬: 카드를 뽑아 돈을 벌 수 있습니다. 높은 카드일수록 더 많이 벌어요!",
        "업그레이드 씬: 스킬 트리로 능력을 강화하세요.",
        "업그레이드로 승리 보상 증가, 패배 손실 감소, 블랙잭 보너스 등을 강화할 수 있습니다.",
        "스킬을 해금하면 새로운 능력이 열립니다. 먼저 기본 업그레이드를 찍어보세요!",
    };

    public static string[] AfterFirstGameEN = new[]
    {
        "First game complete!",
        "Return to the lobby to access more features.",
        "Work Scene: Draw cards to earn money. Higher cards earn more!",
        "Upgrade Scene: Strengthen your abilities with the skill tree.",
        "Upgrades include win reward boost, loss reduction, blackjack bonus, and more.",
        "Unlock skills to gain new abilities. Start with the basic upgrade first!",
    };
}