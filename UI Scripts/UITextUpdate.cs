using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextUpdate : MonoBehaviour {

    public Slider m_ScoreSlider;
    public Text players;
    public Text rules;
    public Text level;
    public Text score;
    public Text buttonRules;

    private void Awake()
    {
        m_ScoreSlider.onValueChanged.AddListener(delegate { SetScoreText(); });
    }

    public void SetPlayerText(string text)
    {
        players.text = "Players: " + text;
    }


    public void SetRulesText(string text)
    {
        rules.text = "Rules: " + text;
    }


    public void SetLevelText(string text)
    {

        level.text = "Level: " + text;
    }

    public void SetScoreText()
    {
        score.text = m_ScoreSlider.value.ToString();
    }

    public void SetButtonRulesText(string text)
    {
        buttonRules.text = "Game Mode: " + text;
    }
}
