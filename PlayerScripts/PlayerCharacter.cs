using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Character")]
//CharacterFactory
public class PlayerCharacter : ScriptableObject
{

    public GameObject m_Character;
    public GameObject m_CharacterModel;
    private int m_ControllerNum;
    public int m_TeamNumber = 0;

    public void Init(int controllerNum)
    {
        m_ControllerNum = controllerNum;
    }

    public Player PlayerInit(int playerCount, int playerNum)
    {
        var playerChar = Instantiate(m_Character);
        Player player = playerChar.GetComponent<Player>();
        Vector2[] camRects = PlayerCams(playerCount, playerNum);
        player.Init(playerNum, m_ControllerNum, camRects[0], camRects[1]);
        player.gameObject.SetActive(false);
        return player;
    }

    private Vector2[] PlayerCams(int players, int playerNum)
    {
        Vector2 position = new Vector2(0f, 0f);
        Vector2 size = new Vector2(1f, 1f);

        if (players > 1)
        {
            if (playerNum == 1)
            {
                position.y = 0.5f;
            }

            size.y = 0.5f;

            if (players > 2)
            {
                size.x = (0.5f);

                if (playerNum == 2 || playerNum == 4)
                {
                    position.x = 0.5f;
                    if (playerNum == 2)
                    {
                        position.y = 0.5f;
                    }
                }
            }
        }
        Vector2[] camsRect = new[] { position, size };
        return camsRect;
    }
}
