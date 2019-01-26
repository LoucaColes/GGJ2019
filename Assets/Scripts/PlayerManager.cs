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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EnableDisablePlayerInput(false);
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
}
