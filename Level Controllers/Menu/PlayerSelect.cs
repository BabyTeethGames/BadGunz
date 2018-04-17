using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//{ "Jump" 0, "Reload" 1, "X" 2, "Switch" 3, "Grenade" 4, "Fire" 5, "Horizontal" 6, "Vertical" 7, "LHorizontal" 8, "LVertical" 9};
// Pass the prefab game object to the game manager. On Instantiation pass variables. Might have to use a dictionary

public class PlayerSelect : MonoBehaviour
{

    public int m_PlayerNum;
    private string[] m_Inputs = { "Jump", "Horizontal", "Reload", "LVertical" };
    private PlayerChoice[] m_PlayerChoices;
    private List<GameObject> m_PlayerCharacters;
    private PlayerMenu m_PlayerMenu;
    public Transform m_PlayerStand;
    public Quaternion m_PlayerStandRot;
    public Image m_AButt;
    public Image m_Check;
    public Image m_NoImg;
    public bool m_Selecting = false;
    public bool m_Ready = false;

    private int m_CharacterNumber = 0;
    private float m_ChoiceTime = 0.3f;
    private float m_NextChoice = 0;
    public int m_CharacterChoice;

    public void Init(PlayerChoice[] playerChoices, PlayerMenu playerMenu)
    {
        m_PlayerMenu = playerMenu;
        m_PlayerChoices = playerChoices;
        m_PlayerStandRot = m_PlayerStand.rotation;
        m_PlayerCharacters = new List<GameObject>();

        for (int i = 0; i < m_PlayerChoices.Length; i++)
        {
            m_PlayerCharacters.Add(Instantiate(m_PlayerChoices[i].character, m_PlayerStand.position, m_PlayerStand.rotation) as GameObject);
            m_PlayerCharacters[i].transform.SetParent(m_PlayerStand, true);
            m_PlayerCharacters[i].SetActive(false);
            m_PlayerCharacters[i].GetComponent<Player>().enabled = false;
            m_PlayerCharacters[i].GetComponent<FPSController>().enabled = false;
            foreach (Camera cam in m_PlayerCharacters[i].GetComponentsInChildren<Camera>())
            {
                cam.enabled = false;
            }
        }

        for (int i = 0; i < m_Inputs.Length; i++)
        {
            m_Inputs[i] = string.Concat("P", m_PlayerNum.ToString(), "_", m_Inputs[i]);
        }
    }

    private void Update()
    {
        if (m_Ready)
        {
            if (Input.GetButtonDown(m_Inputs[0]))
            {
                m_PlayerMenu.SubmitCheck();
            }

            if (Input.GetButtonDown(m_Inputs[2]))
            {
                PlayerCommit(false);
            }
        }

        if (m_Selecting)
            CharacterSelect();

        if (!m_Selecting && !m_Ready)
        {
            if (Input.GetButtonDown(m_Inputs[0]))
            {
                SelectionEnable();
            }
            if (Input.GetButtonDown(m_Inputs[2]))
            {
                m_PlayerMenu.Back();
            }
        }
    }




    private void CharacterSelect()
    {
        float spinAxis = (Input.GetAxis(m_Inputs[3]));
        CharacterCheck();

        if (Input.GetButtonDown(m_Inputs[2]))
        {
            m_Selecting = false;
            m_PlayerCharacters[m_CharacterNumber].SetActive(false);
            m_AButt.enabled = true;
            m_CharacterNumber = 0;
            m_PlayerStand.rotation = m_PlayerStandRot;
        }

        int choice = AxisInt();

        if (choice != 0 && Time.time > m_NextChoice)
        {
            m_PlayerCharacters[m_CharacterNumber].SetActive(false);
            m_CharacterNumber += choice;

            if (m_CharacterNumber < 0)
                m_CharacterNumber = m_PlayerCharacters.Count - 1;
            else
                m_CharacterNumber = m_CharacterNumber % (m_PlayerCharacters.Count);

            m_NextChoice = Time.time + m_ChoiceTime;
            m_PlayerCharacters[m_CharacterNumber].SetActive(true);
        }

        if (Input.GetButtonDown(m_Inputs[0]) && !m_NoImg.enabled)
        {
            PlayerCommit(true);
        }

        m_PlayerStand.Rotate(Vector3.up * spinAxis * -60 * Time.deltaTime);
    }

    private int AxisInt()
    {
        float choiceAxis = (Input.GetAxis(m_Inputs[1]));
        int choice;
        if (choiceAxis > 0.15f)
        {
            choice = 1;
        }
        else if (choiceAxis < -0.15f)
        {
            choice = -1;
        }
        else choice = 0;

        return choice;
    }

    private void SelectionEnable()
    {
        m_AButt.enabled = false;
        m_PlayerCharacters[m_CharacterNumber].SetActive(true);
        m_Selecting = true;
    }

    private void CharacterCheck()
    {
        if (m_PlayerChoices[m_CharacterNumber].selected)
        {
            m_NoImg.enabled = true;
        }
        else
        {
            m_NoImg.enabled = false;
        }
    }

    private void PlayerCommit(bool commit)
    {
        m_Ready = commit;
        m_Check.enabled = commit;
        m_PlayerChoices[m_CharacterNumber].selected = commit;
        m_Selecting = !commit;
        if (commit)
            m_CharacterChoice = m_CharacterNumber;
        else
            m_CharacterChoice = 0;
    }
}
