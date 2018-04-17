using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Survival GameMode")]
public class Survival : GameMode
{

    public override void Init(List<Player> players)
    {
        Player.onKilled -= ScoreUpdate;
        Player.onKilled += ScoreUpdate;
        m_Players = players;
        foreach (Player player in m_Players)
        {
            player.m_Score = m_ScoreLimit;
            player.m_UI.ScoreUpdate(m_ScoreLimit);
        }
    }
    
    public override void ScoreUpdate(int playerDead, int playerKill)
    {
        if (playerDead != 0)
        {
            m_Players[playerDead -1].m_Score -= 1;
            m_Players[playerDead - 1].m_UI.ScoreUpdate(m_Players[playerDead - 1].m_Score);
        }
        else
        {
            Debug.Log("The Player who died is = 0");
        }

        CheckWinner();
    }


    public override void CheckWinner()
    {
        int playersLeft = 0;

        foreach (Player player in m_Players)
        {
            if (player.m_Score > 0)
            {
                m_WinNumber = player.m_PlayerNum;
                playersLeft += 1;
            }            
        }
        if (playersLeft == 1)
        {
            if (!EventIsNull())
            {
                GameOverEvent();
            }
        }

        if (playersLeft < 1)
        {
            Debug.Log("Draw Event Here");
        }
    }

    public override bool RespawnCheck(int playerNum)
    {
        if(m_Players[playerNum-1].m_Score > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
