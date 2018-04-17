using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public float m_AmmoNextSpawn = 0, m_AmmoSpawnMin = 1, m_AmmoSpawnMax = 15, m_WeaponNextSpawn = 0, m_WeaponSpawnMin = 1, m_WeaponSpawnMax = 15;
    private GameObject[] m_AmmoSpawnTargets, m_PlayerSpawns, m_WeaponSpawnTargets;
    [SerializeField] public List<GameObject> m_Gunz, m_AmmoList;

    public void Init(List<GameObject> gunz)
    {
        m_Gunz = new List<GameObject>(gunz);
        m_AmmoSpawnTargets = GameObject.FindGameObjectsWithTag("AmmoSpawn");
        m_PlayerSpawns = GameObject.FindGameObjectsWithTag("Respawn");
        m_WeaponSpawnTargets = GameObject.FindGameObjectsWithTag("WeaponSpawn");

    }


    public virtual void Update()
    {
        if (Time.time > m_AmmoNextSpawn)
        {
            if (m_AmmoList.Count > 0)
            {
                if (Spawner(m_AmmoSpawnTargets, m_AmmoList[Random.Range(0, m_AmmoList.Count)]))
                    m_AmmoNextSpawn += Time.time + Random.Range(m_AmmoSpawnMin, m_AmmoSpawnMax);
                else
                    m_AmmoNextSpawn += 1;
            }
            else
            {
                Debug.Log("There is no Ammo to spawn. Add Ammo to the ammo list in the inspector");
            }
        }

        if (Time.time > m_WeaponNextSpawn)
        {
            if (m_Gunz.Count > 0)
            {
                if (Spawner(m_WeaponSpawnTargets, m_Gunz[Random.Range(0, m_Gunz.Count)]))
                    m_WeaponNextSpawn += Time.time + Random.Range(m_AmmoSpawnMin, m_AmmoSpawnMax);
                else
                    m_WeaponNextSpawn += 1;
            }
            else
            {
                Debug.Log("There are no weapons selected, check GameManager is sending the list/there are any weapons selected in the Main Menu");
            }
        }
    }

    private bool Spawner(GameObject[] spawnTarget, GameObject spawnObject)
    {
        if (spawnTarget.Length > 0)
        {
            Transform spawnTransform = spawnTarget[Random.Range(0, spawnTarget.Length)].transform;
            if (CheckSpawn(spawnTransform))
            {
                var spawn = Instantiate(spawnObject, spawnTransform.position, spawnTransform.rotation);
                spawn.transform.SetParent(spawnTransform);
                return true;
            }
            return false;
        }
        else
        {
            Debug.Log("There are no SpawnPoints for: " + spawnObject.name);
            return false;
        }
    }

    public void SpawnPlayer(Player player)
    {
        if (m_PlayerSpawns.Length > 0)
        {  
            var spawn = m_PlayerSpawns[Random.Range(0, m_PlayerSpawns.Length)].transform;
            player.gameObject.transform.position = spawn.transform.position;
            player.gameObject.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.Log("There are no Player Spawns in this level");
        }
    }
   
    bool CheckSpawn(Transform parent)
    {
        if (parent.childCount > 0)
            return (false);
        else
            return (true);
    }
}
