[System.Serializable]

public class GameData
{   
    // 게임자원
    public long money;
    public int skillPoint;
    public long bankBalance = 0;
    public int[] skillLevels;

    // 설정
    public float Volume;
    public bool isMute;

    // 기본값 설정
    public GameData()
    {
        money = 10000000;   // 기본값 1000
        skillPoint = 1;
        skillLevels = new int[11];
        Volume = 1.0f;
        isMute = false;
    }
}