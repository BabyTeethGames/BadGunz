using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GameMode : ScriptableObject {
    public delegate void GameOver();
    public static event GameOver onGameOver;
    
    public int m_ScoreLimit = 5;
    public bool m_Teams = false;
    public List<Player> m_Players;
    public int m_WinNumber;


    public abstract void Init(List<Player> players);

    public abstract void ScoreUpdate(int playerDead, int playerKill);

    public abstract void CheckWinner();

    public abstract bool RespawnCheck(int playerNum);

    protected bool EventIsNull()
    {
        if (onGameOver == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void GameOverEvent()
    {
        onGameOver();
    }

}
