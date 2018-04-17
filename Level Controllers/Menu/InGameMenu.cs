using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InGameMenu : MonoBehaviour {

    public Slider m_ScoreSlider;
    private GameManager m_GameManager;
    private Vector3 m_CamRotOrig;
    public GameObject m_PlayerMenu;
    public Camera m_Cam;
    public GameObject m_MainMenu;
    public int m_ScoreLimit = 1;
    public bool m_Team = false;


    private void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_CamRotOrig = m_Cam.transform.eulerAngles;
        m_ScoreSlider.onValueChanged.AddListener(delegate { SetScoreLimit(); });
    }

    private void OnEnable()
    {
        PlayerMenu.OnPlayerMenuBack += PlayerMenuBack;
    }

    public void PlayerMenuStart()
    {
        StopAllCoroutines();
        m_PlayerMenu.SetActive(true);
        StartCoroutine(CameraTurn(m_Cam.transform, new Vector3(0, 90, 0)));
    }

    public void PlayerMenuBack()
    {
        StopAllCoroutines();
        m_PlayerMenu.SetActive(false);
        m_MainMenu.SetActive(true);    
        StartCoroutine(CameraTurn(m_Cam.transform, m_CamRotOrig));
    }

   

    public void SetSceneIndex(int scene)
    {
        m_GameManager.m_SceneToLoad = scene;
    }

    IEnumerator CameraTurn(Transform camera, Vector3 turn)
    {
        while (camera.eulerAngles != turn)
        {
            camera.eulerAngles = Vector3.Slerp(camera.eulerAngles, turn, Time.deltaTime * 5);
            yield return null;
        }
            yield return null;
    }

    public void SetGameMode(GameMode gameMode)
    {
        m_GameManager.m_GameMode = gameMode;
        m_GameManager.m_GameMode.m_ScoreLimit = m_ScoreLimit;
        m_GameManager.m_GameMode.m_Teams = m_Team;
    }

    public void SetScoreLimit()
    {
        m_ScoreLimit = Mathf.RoundToInt(m_ScoreSlider.value);
    }
}
