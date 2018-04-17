using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text m_HealthText, m_ScoreText, m_AmmoCountText, m_GunPickupText;


    public void ScoreUpdate(int score)
    {
        m_ScoreText.text = score.ToString();
    }

    public void HealthUpdate(int health)
    {
        m_HealthText.text = health.ToString();
    }

    public void AmmoUpdate(string ammo)
    {
        m_AmmoCountText.text = ammo;
    }
}
