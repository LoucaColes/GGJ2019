using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private PlayerManager playerManager = null;
    [SerializeField] private CameraShake cameraShake = null;
    [SerializeField] private GameOverAnimation gameOver = null;
    [SerializeField] private GameObject innerBounds;
    [Header("Parameters")]
    [SerializeField] private float startupDuration = 3.0f;
    [SerializeField] private float preparationDuration = 30.0f;
    [SerializeField] private float surviveDuration = 120.0f;

    private GameState mGameState;
    private float mTimer; //Might as well be used for all tracked times
    private Coroutine mCurrentUpdate_Coroutine;
    private AudioSource musicAudioSource;
    #endregion

    #region Unity Events
    private void Awake()
    {
        GridHelper.Define(1, new Vector2(8, 4.5f));
        mGameState = GameState.JOIN;
        playerManager.UpdateGameState(mGameState);

        if (mCurrentUpdate_Coroutine != null)
            StopCoroutine(mCurrentUpdate_Coroutine);

        mCurrentUpdate_Coroutine = StartCoroutine(Join());
    }

    private void Update()
    {
        if (mGameState == GameState.GAMEOVER)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (musicAudioSource != null && musicAudioSource.gameObject.activeSelf)
                {
                    musicAudioSource.Stop();
                }
                ReloadGame();
            }
        }
    }
    #endregion

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Join()
    {
        //AudioManager.instance.Play("Atmos", Vector3.zero, true);
        //AudioManager.instance.Play("Campfire", Vector3.zero, true);
        if (musicAudioSource != null && musicAudioSource.gameObject.activeSelf)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource = AudioManager.instance.Play("Music", Vector3.zero, true);
        mGameState = GameState.JOIN;
        playerManager.UpdateGameState(mGameState);
        innerBounds.SetActive(true);

        yield return new WaitForSeconds(1);

        while (true)
        {
            //if (playerManager.IsEveryoneReady())
            //{
            //    break;
            //}
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }
            yield return null;
        }

        mCurrentUpdate_Coroutine = StartCoroutine(Startup());
    }

    private IEnumerator Startup()
    {
        mGameState = GameState.STARTUP;

        playerManager.UpdateGameState(mGameState);
        mTimer = 0;

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
        playerManager.UpdateGameState(mGameState);
        mTimer = 0;

        while (mTimer < preparationDuration)
        {
            mTimer += Time.deltaTime;
            yield return null;
        }

        innerBounds.SetActive(false);

        mCurrentUpdate_Coroutine = StartCoroutine(Survive());
    }

    private IEnumerator Survive()
    {
        mGameState = GameState.SURVIVE;
        playerManager.UpdateGameState(mGameState);
        innerBounds.SetActive(false);
        mTimer = 0;

        while (mTimer < surviveDuration)
        {
            mTimer += Time.deltaTime;
            CheckGameOver();
            yield return null;
        }

        for (int i = 0; i < 4; i++)
        {
            playerManager.ExtinguishAllFires();
            if (musicAudioSource != null && musicAudioSource.gameObject.activeSelf)
            {
                musicAudioSource.Stop();
            }
            AudioManager.instance.StopCampFireAudio();
        }
        cameraShake.ShakeCamera(); 

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
        if(playerManager.AlivePlayersCount() <= 1)
        {
            if (mCurrentUpdate_Coroutine != null)
                StopCoroutine(mCurrentUpdate_Coroutine);

            mGameState = GameState.GAMEOVER;
            cameraShake.ShakeCamera();
            gameOver.GameOver();

        }

    }
}
