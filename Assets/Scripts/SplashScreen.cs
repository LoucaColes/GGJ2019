using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private FadeTransition fadeTransition;

    [SerializeField] private string sceneName;

    [SerializeField] private float loadTime;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("Atmos", Vector3.zero, true);
        AudioManager.instance.Play("Campfire", Vector3.zero, true);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(loadTime);
        fadeTransition.FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
