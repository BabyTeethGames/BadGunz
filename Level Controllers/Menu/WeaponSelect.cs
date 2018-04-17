using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{

    public Toggle m_Toggle;
    public GameObject m_Gun;
    private List<GameObject> m_Gunz;
    
    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
    }

    public void Init(List<GameObject>gunz)
    {
        m_Gunz = gunz;       
        m_Toggle.onValueChanged.AddListener(delegate {ToggleValueChanged(m_Toggle);});
    }

    void ToggleValueChanged(Toggle change)
    {
        if (m_Gunz.Contains(m_Gun))
            m_Gunz.Remove(m_Gun);
        else
            m_Gunz.Add(m_Gun);
    }
}
