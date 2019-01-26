using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player[] alivePlayers = new Player[4];
    

    private Player[] deadPlayers = new Player[4];

    // Start is called before the first frame update
    void Start()
    {
        EnableDisablePlayerInput(true);

        InitPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EnableDisablePlayerInput(false);
        }
    }

    private void InitPlayers()
    {
        for (int i = 0; i < alivePlayers.Length; i++)
        {
            if (alivePlayers[i] != null)
            {
                alivePlayers[i].Init(this);
            }
        }
    }

    public void EnableDisablePlayerInput(bool _enable)
    {
        for (int i = 0; i < alivePlayers.Length; i++)
        {
            if (alivePlayers[i] != null)
            {
                alivePlayers[i].AllowInput = _enable;
            }

            if (deadPlayers[i] != null)
            {
                deadPlayers[i].AllowInput = _enable;
            }
        }
    }

    public void SetPlayerAlive(int _playerId, Player _player)
    {
        alivePlayers[_playerId] = _player;
        deadPlayers[_playerId] = null;
    }

    public void SetDeadPlayer(int _playerId, Player _player)
    {
        alivePlayers[_playerId] = null;
        deadPlayers[_playerId] = _player;
    }

    //Puts out fires
    public void KillFire(int _playerid)
    {
        alivePlayers[_playerid].Campfire.SetDead();
        deadPlayers[_playerid].Campfire.SetDead();
    }
}
