[System.Serializable]
public class GameData
{
    public int Streak;
    public int WinStreak;
    public bool FirstMeeting;
    public bool FirstGame;
    public bool FirstWin;
    public bool FirstFriend;

    public GameData(GameManager G)
    {
        Streak = G.Streak;
        WinStreak = G.WinStreak;
        FirstGame = G.FirstGame;
        FirstWin = G.FirstWin;
        FirstFriend = G.FirstFriend;
        FirstMeeting = G.FirstMeeting;
    }
}
