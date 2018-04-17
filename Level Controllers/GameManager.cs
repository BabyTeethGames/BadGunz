using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance = null;

    [SerializeField] public List<GameObject> m_Gunz;
    public List<PlayerManager> m_PlayerManagers;
    private List<Player> m_PlayerInstances;
    public GameMode m_GameMode;
    private GameManagerUI m_UI;
    private LevelManager m_LevelManager;
    public Camera m_EndCam;
    private Transform m_WinPlayerTransform;
    public int m_SceneToLoad;
    private bool m_MainMenu;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (m_Instance == null)
            m_Instance = this;
        else if (m_Instance != null)
            Destroy(this.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        m_PlayerInstances = new List<Player>();
        m_LevelManager = GetComponent<LevelManager>();
        m_UI = GetComponentInChildren<GameManagerUI>();
        Player.onRespawn += Respawn;
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameMode.onGameOver += GameEnd;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_UI.Init();

        if (scene.buildIndex == 0)
        { 
            
        }

        else
        {
            m_LevelManager.Init(m_Gunz);
            m_LevelManager.enabled = true;
            PlayerInit();
            m_GameMode.Init(m_PlayerInstances);
        }
    }
    
    private void Update()
    {
        
        UpdateCursorLock();

    }

    private void GameEnd()
    {
        int winPlayerNum = m_GameMode.m_WinNumber;
        

        foreach (Player player in m_PlayerInstances)
        {
            if (player.m_PlayerNum != winPlayerNum)
            {
                player.GameEnd(false);
            }
            else
            {
                Camera endCam = Instantiate(m_EndCam, transform.position, transform.rotation);
                endCam.GetComponent<CameraFollow>().m_Target = player.transform;
                player.GameEnd(true);
            }
        }
        m_UI.OnWin(winPlayerNum);
    }
    

    public void PlayerAdd(GameObject gO, int controllerNum)
    {
        var playerMan = gameObject.AddComponent<PlayerManager>();
        playerMan.Init(gO, controllerNum);
        m_PlayerManagers.Add(playerMan);
    }

    public void PlayerListVoid()
    {
        foreach(PlayerManager player in m_PlayerManagers)
        {
            Destroy(player);
        }

        m_PlayerManagers.Clear();
    }

    void PlayerInit()
    {
        int playerCount = m_PlayerManagers.Count;
        if (playerCount == 3)
        {
            //instantiate the scorecamera
        }
        for (int i = 0; i < playerCount; i++)
        {
            m_PlayerInstances.Add(m_PlayerManagers[i].PlayerInit(playerCount, i+1));
            m_LevelManager.SpawnPlayer(m_PlayerInstances[i]);
            m_PlayerInstances[i].gameObject.SetActive(true);
        }
    }

    void Respawn(Player player)
    {
        if (m_GameMode.RespawnCheck(player.m_PlayerNum))
        {
            player.Reset();
            m_LevelManager.SpawnPlayer(player);
        }
    }

    void UpdateCursorLock()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;

        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        Player.onRespawn -= Respawn;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
