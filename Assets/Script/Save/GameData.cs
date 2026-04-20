[System.Serializable]

public class GameData
{   
    // 게임자원
    public int coin;
    
    public int[] skillLevels;

    // 설정
    public float Volume;
    public bool isMute;

    // 기본값 설정
    public GameData()
    {
        coin = 1000;
        skillLevels = new int[11];
        Volume = 1.0f;
        isMute = false;
    }
}