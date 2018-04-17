using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeathMatch GameMode")]
public class DeathMatch : GameMode
{
    
    public override void Init(List<Player> players)
    {
        Player.onKilled -= ScoreUpdate;
        Player.onKilled += ScoreUpdate;
        m_Players = players;
        foreach (Player player in m_Players)
        {
            player.m_Score = 0;
            player.m_UI.ScoreUpdate(0);
        }
    }

    public override void CheckWinner()
    {
        foreach (Player player in m_Players)
        {
            if(player.m_Score >= m_ScoreLimit)
            {
                m_WinNumber = player.m_PlayerNum;
                if (!EventIsNull())
                {
                    GameOverEvent();
                }
            }
        }
    }

    public override void ScoreUpdate(int playerDead, int playerKill)
    {
        if (playerKill != 0)
        {
            if (playerDead != playerKill)
            {
                m_Players[playerKill -1].m_Score += 1;
                m_Players[playerKill - 1].m_UI.ScoreUpdate(m_Players[playerKill - 1].m_Score);

            }

            else
            {
                m_Players[playerDead -1].m_Score -= 1;
                m_Players[playerDead - 1].m_UI.ScoreUpdate(m_Players[playerDead - 1].m_Score);
            }
        }

        else
        {
            Debug.Log("The Playershot = 0");
        }

        CheckWinner();

    }

    public override bool RespawnCheck(int playerNum)
    {
        return true;
    }


}
