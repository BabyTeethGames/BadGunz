using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectInit : MonoBehaviour {

    private WeaponSelect[] m_Weapons;
    public List<GameObject> m_Gunz;

    public void Start()
    {
        m_Weapons = GetComponentsInChildren<WeaponSelect>(true);
        m_Gunz = FindObjectOfType<GameManager>().m_Gunz;

        foreach (WeaponSelect weapon in m_Weapons)
        {
            if (m_Gunz.Contains(weapon.m_Gun))
            {
                weapon.m_Toggle.isOn = true;
            }
            else
            {
                weapon.m_Toggle.isOn = false;
            }

            weapon.Init(m_Gunz);
        }
    }
}
