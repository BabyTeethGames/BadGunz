using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour {

    //If you return to the Main Menu, if the GameManager is preloaded there will be two. 
    //A Singleton to make sure there aren't multiple gameManagers when switching through scenes

    public GameObject gameManager;
    private void Awake()
    {
        if (GameManager.m_Instance == null)
            Instantiate(gameManager);
    }
}
