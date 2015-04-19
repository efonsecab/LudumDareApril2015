using UnityEngine;
using System.Collections;

public static class GameController
{
    public enum GameplayStatus
    {
        Play,
        PlayerLost,
        PlayerWon
    }

    public static GameplayStatus CurrentGamePlayStatus = GameplayStatus.Play;


    public static void Win()
    {
        CurrentGamePlayStatus = GameplayStatus.PlayerWon;
    }

    public static void Lose()
    {
        CurrentGamePlayStatus = GameplayStatus.PlayerLost;
    }
}
