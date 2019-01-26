using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private GameObject transitionObject;

    [SerializeField] private DOTweenAnimation fadeoutTweenAnimation;

    [SerializeField] private DOTweenAnimation fadeinTweenAnimation;

    [SerializeField] private Image fadeinImage;

    private void Start()
    {
        FadeIn();
    }

    public void FadeOut()
    {
        fadeoutTweenAnimation.DORestart();
        fadeoutTweenAnimation.DOPlay();
    }

    public void FadeIn()
    {
        fadeinImage.enabled = true;
        fadeinTweenAnimation.DORestart();
        fadeinTweenAnimation.DOPlay();
    }
}
