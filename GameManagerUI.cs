using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManagerUI : MonoBehaviour {

    public Canvas m_MessageCanvas;
    public Text m_MessageText;
    public float m_PanelFlashTime = 3;


    private void Awake()
    {
        Player.onKilled += OnKilled;
    }

    public void Init()
    {
        m_MessageCanvas.enabled = false;
    }

    private void OnKilled(int playerDead, int playerKill)
    {
        if (playerDead != playerKill)
        {
            m_MessageText.text = string.Format("Player {0} obliterated player {1}", playerKill, playerDead);
        }
        else
        {
            m_MessageText.text = string.Format("Player{0} Killed himself", playerDead);
        }
            StartCoroutine(PanelFlash(m_MessageCanvas, m_PanelFlashTime));
    }

    private void OnDestroy()
    {
        Player.onKilled -= OnKilled;
    }

    public void OnWin(int playerWin)
    {
        m_MessageText.text = string.Format("Player {0} has emerged victorious", playerWin);
        m_MessageCanvas.enabled = true;
    }

    IEnumerator PanelFlash (Canvas panel, float panelFlashTime)
    {
        panel.enabled = true;
        yield return new WaitForSeconds(panelFlashTime);
        panel.enabled = false;
    }

    public void InGameMenu()
    {

    }
}
