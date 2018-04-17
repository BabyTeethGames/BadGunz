using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnAdd : MonoBehaviour {

    public EventSystem m_EventSystem;
    public GameObject selectedObject;
    private bool buttonSelected;
	// Use this for initialization
	void Start () {
        m_EventSystem = FindObjectOfType<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (buttonSelected == false)
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            m_EventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
