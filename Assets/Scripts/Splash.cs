using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Splash : MonoBehaviour
{
    #region Variables
    private Coroutine mUpdate_Coroutine;
    private Action mOnContinue;
    #endregion

    public void ShowSplash(Action _onContinue)
    {
        mOnContinue = _onContinue;
        if(mUpdate_Coroutine != null)
        {
            Debug.LogWarning("Splash: Somehow the update coroutine has already set");
            StopCoroutine(mUpdate_Coroutine);
        }
        mUpdate_Coroutine = StartCoroutine(UpdateViaCoroutine());
    }

    private IEnumerator UpdateViaCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mOnContinue();
                break;
            }
            yield return null;
        }
        mUpdate_Coroutine = null;
    }
}
