using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    //[Header("References")]
    [SerializeField] private PlayerManager playerManager = null;
    [SerializeField] private CameraShake cameraShake = null;
    [SerializeField] private GameOverAnimation gameOver = null;
    [Header("Parameters")]
    [SerializeField] private float startupDuration = 3.0f;
    [SerializeField] private float preparationDuration = 30.0f;
    [SerializeField] private float surviveDuration = 120.0f;

    [SerializeField] private GameObject innerBounds;

    private Player[] mPlayers;
    private GameState mGameState;
    private float mTimer; //Might as well be used for all tracked times
    private Coroutine mCurrentUpdate_Coroutine;

    #endregion

    #region Unity Events
    private void Awake()
    {
        GridHelper.Define(1, new Vector2(8, 4.5f));
        mGameState = GameState.JOIN;
        //playerManager.UpdateGameState(mGameState);

        if (mCurrentUpdate_Coroutine != null)
            StopCoroutine(mCurrentUpdate_Coroutine);

        mCurrentUpdate_Coroutine = StartCoroutine(Join());
    }

    public void Update()
    {
        GridHelper.Update();
    }
    #endregion

    private void ReloadGame()
    {
        if (mCurrentUpdate_Coroutine != null)
            StopCoroutine(mCurrentUpdate_Coroutine);

        mCurrentUpdate_Coroutine = StartCoroutine(Join());
    }

    private IEnumerator Join()
    {
        mGameState = GameState.JOIN;
        //playerManager.UpdateGameState(mGameState);
        playerManager.EnableDisablePlayerInput(false);

        yield return new WaitForSeconds(3);

        //while (true)
        //{
        //    //if (playerManager.IsEveryoneReady())
        //    //{
        //    //    break;
        //    //}
        //    yield return null;
        //}

        mCurrentUpdate_Coroutine = StartCoroutine(Startup());
    }

    private IEnumerator Startup()
    {
        mGameState = GameState.STARTUP;
        innerBounds.SetActive(true);
        //playerManager.UpdateGameState(mGameState);
        mTimer = 0;

        //Get number of players

        while (mTimer < startupDuration)
        {
            mTimer += Time.deltaTime;
            yield return null;
        }

        mCurrentUpdate_Coroutine = StartCoroutine(Preparation());
    }

    private IEnumerator Preparation()
    {
        mGameState = GameState.PREPARATION;
        playerManager.EnableDisablePlayerInput(true);
        //playerManager.UpdateGameState(mGameState);
        mTimer = 0;

        while (mTimer < preparationDuration)
        {
            mTimer += Time.deltaTime;
            yield return null;
        }

        //Drop grid limits

        mCurrentUpdate_Coroutine = StartCoroutine(Survive());
    }

    private IEnumerator Survive()
    {
        mGameState = GameState.SURVIVE;
        //playerManager.UpdateGameState(mGameState);
        innerBounds.SetActive(false);
        mTimer = 0;

        while (mTimer < surviveDuration)
        {
            mTimer += Time.deltaTime;
            CheckGameOver();
            yield return null;
        }

        //Automatically put out the campfires
        if (mTimer >= surviveDuration)
        {
            CheckGameOver();
            for (int i = 0; i < 4; i++)
            {
                playerManager.KillFire(i);
            }
            cameraShake.ShakeCamera(); 
        }

        while (true)
        {
            CheckGameOver();
            yield return null;
        }

        //mCurrentUpdate_Coroutine = null;
    }

    /// <summary>
    /// Checks if the remaining players, if there is 1 or less end the game
    /// </summary>
    private void CheckGameOver()
    {
        //if(playerManager.GetNumAlivePlayers() <= 1)
        //{
        //    if (mCurrentUpdate_Coroutine != null)
        //        StopCoroutine(mCurrentUpdate_Coroutine);

        //    mGameState = GameState.GAMEOVER;
        //    cameraShake.ShakeCamera();
        //    gameOver.GameOver();
        //    ReloadGame();
        //}

    }
}
