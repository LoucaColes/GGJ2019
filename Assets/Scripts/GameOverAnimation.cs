using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOverAnimation : MonoBehaviour
{
    private DOTweenAnimation fadeoutTweenAnimation;

    // Start is called before the first frame update
    void Awake()
    {
        fadeoutTweenAnimation = GetComponent<DOTweenAnimation>();
    }

    public void GameOver()
    {
        fadeoutTweenAnimation.DORestart();
        fadeoutTweenAnimation.DOPlay();
    }
}
