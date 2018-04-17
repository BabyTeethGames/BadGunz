using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private GameManager m_GameManager;
    private PlayerSelect[] m_PlayerSelects;
    public PlayerChoice[] PlayerChoices; // Struct at the bottom of this class that holds a Player gameobject and a bool for selection check
    public delegate void PlayerMenuBackAction();
    public static event PlayerMenuBackAction OnPlayerMenuBack;

    void Awake()
    {
        m_PlayerSelects = GetComponentsInChildren<PlayerSelect>(true);
        m_GameManager = FindObjectOfType<GameManager>();

        foreach (PlayerSelect playerSelect in m_PlayerSelects)
        {
            playerSelect.Init(PlayerChoices, this);
        }

    }

    private void OnEnable()
    {
        m_GameManager.PlayerListVoid();
    }

    public void SubmitCheck()
    {
        bool ready = true;
        foreach (PlayerSelect playerSelect in m_PlayerSelects)
        {
            if (!playerSelect.m_Ready && playerSelect.m_Selecting)
            {
                ready = false;
            }
        }

        if (ready)
        {
            CommitToGameManager();
        }
    }

    private void CommitToGameManager()
    {
        foreach (PlayerSelect playerSelect in m_PlayerSelects)
        {
            if (playerSelect.m_Ready)
            {
                m_GameManager.PlayerAdd(PlayerChoices[playerSelect.m_CharacterChoice].character, playerSelect.m_PlayerNum);
            }
        }
        m_GameManager.LoadScene();
    }

    public void Back()
    {
        if (OnPlayerMenuBack != null)
        {
            OnPlayerMenuBack();
        }
    }
}

[System.Serializable]
public struct PlayerChoice
{
    [SerializeField] public GameObject character;
    [SerializeField] public bool selected;
}
