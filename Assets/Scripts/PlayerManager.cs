using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private Player[] players = null;

    public int numAlivePlayers { get { return alivePlayers.Length; } }

    //This should probably be done differently now
    private Player[] alivePlayers = new Player[4];
    private Player[] deadPlayers = new Player[4];

    private Coroutine mDetectPlayers_Coroutine;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        //Inject playermanager into the players
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                players[i].Init(this);
            }
        }
    }

    public void EnableDisablePlayerInput(bool _enable)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                players[i].AllowInput = _enable;
            }

            if (players[i] != null)
            {
                players[i].AllowInput = _enable;
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
    public void ExtinguishAllFires()
    {
        for(int i = 0; i < alivePlayers.Length; ++i)
        {
            if (alivePlayers[i])
                alivePlayers[i].Campfire.Extinguish();
        }
    }

    private IEnumerator DetectPlayers()
    {
        while (true)
        {
            string[] joysticks = Input.GetJoystickNames();
            for (int i = 0; i < 4; ++i)
            {
                if(i < joysticks.Length)
                {
                    if (!string.IsNullOrEmpty(joysticks[i]))
                    {
                        //Controller not active becomes active
                        if (!players[i].gameObject.activeSelf)
                        {
                            players[i].gameObject.SetActive(true);
                            players[i].Campfire.gameObject.SetActive(true);
                            players[i].transform.position = players[i].Campfire.transform.position + Vector3.left;
                            players[i].CharacterAnimator.Sit();
                        }
                        continue;
                    }
                }

                //Controller not active
                players[i].gameObject.SetActive(false);
                players[i].Campfire.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    public void UpdateGameState(GameState _state)
    {
        switch (_state)
        {
            case GameState.JOIN:
                EnableDisablePlayerInput(false);
                if (mDetectPlayers_Coroutine == null)
                    mDetectPlayers_Coroutine = StartCoroutine(DetectPlayers());
                break;
            case GameState.STARTUP:
                if (mDetectPlayers_Coroutine != null)
                    StopCoroutine(mDetectPlayers_Coroutine);

                alivePlayers = new Player[4];
                deadPlayers = new Player[4];

                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].gameObject.activeSelf)
                    {
                        alivePlayers[i] = players[i];
                        alivePlayers[i].CharacterAnimator.StandUp();
                    }
                }
                break;
            case GameState.PREPARATION:
                EnableDisablePlayerInput(true);
                break;
            case GameState.SURVIVE:
                break;
            case GameState.GAMEOVER:
                break;
            default:
                Debug.LogError("Invalid state");
                break;
        }
    }
}
