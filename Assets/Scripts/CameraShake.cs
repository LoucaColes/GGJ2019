using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{

    private DOTweenAnimation fadeoutTweenAnimation;

    // Start is called before the first frame update
    void Awake()
    {
        fadeoutTweenAnimation = GetComponent<DOTweenAnimation>();
    }

    public void ShakeCamera()
    {
        fadeoutTweenAnimation.DORestart();
        fadeoutTweenAnimation.DOPlay();
    }
}
