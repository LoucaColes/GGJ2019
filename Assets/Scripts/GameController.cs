using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Splash splash = null;
    [Header("Parameters")]
    [SerializeField] private float startupDuration = 3.0f;
    [SerializeField] private float preparationDuration = 30.0f;
    [SerializeField] private float surviveDuration = 120.0f;

    private Player[] mPlayers;
    private GameState mGameState;
    private float mTimer; //Might as well be used for all tracked times
    private Coroutine mCurrentUpdate_Coroutine;
    #endregion

    #region Unity Events
    private void Awake()
    {
        mGameState = GameState.SPLASH;
        splash.ShowSplash(BeginGame);
    }
    #endregion

    private void BeginGame()
    {
        if (mCurrentUpdate_Coroutine != null)
            StopCoroutine(mCurrentUpdate_Coroutine);

        mCurrentUpdate_Coroutine = StartCoroutine(Startup());
    }

    private void ReloadGame()
    {
        if (mCurrentUpdate_Coroutine != null)
            StopCoroutine(mCurrentUpdate_Coroutine);

        mCurrentUpdate_Coroutine = StartCoroutine(Startup());
    }

    private IEnumerator Startup()
    {
        mGameState = GameState.STARTUP;
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
        mTimer = 0;

        while (mTimer < surviveDuration)
        {
            mTimer += Time.deltaTime;
            CheckGameOver();
            yield return null;
        }

        //Automatically put out the campfires

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
        int remainingPlayers = mPlayers.Length;
        for(int i = 0; i < mPlayers.Length; ++i)
        {
            //if (mPlayers[i].isOut)
                //--remainingPlayers;
        }

        if(remainingPlayers <= 1)
        {
            if (mCurrentUpdate_Coroutine != null)
                StopCoroutine(mCurrentUpdate_Coroutine);

            mGameState = GameState.GAMEOVER;
            ReloadGame();
        }
    }

}
