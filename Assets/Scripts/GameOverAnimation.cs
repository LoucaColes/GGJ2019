using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameOverAnimation : MonoBehaviour
{
    [SerializeField]
    private DOTweenAnimation fadeoutTweenAnimation;

    [SerializeField] private GameObject image;

    [SerializeField] private ParticleSystem[] particles;

    // Start is called before the first frame update
    void Awake()
    {
    }

    public void GameOver()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        image.SetActive(true);
        fadeoutTweenAnimation.DORestart();
        fadeoutTweenAnimation.DOPlay();
        AudioManager.instance.Play("GBP", Vector3.zero);
    }
}
