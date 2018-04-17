using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitTest : MonoBehaviour
{
    [SerializeField] public List<GameObject> gunz;
    public LevelManager lm;
    private void Awake()
    {
        lm.Init(gunz);
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            player.Init(1, 1, new Vector2(0f, 0f), new Vector2(1f, 1f));
        }
    }
}
    