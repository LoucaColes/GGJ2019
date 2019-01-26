using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Placeable
{
    #region Variables


    #endregion

    #region Unity Events
    private void Awake()
    {

    }
    #endregion

    void CanRespawn()
    {

    }

    public void SetDead()
    {
        Debug.Log("Fire is Dead");
    }
}
