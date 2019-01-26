using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Placeable
{
    #region Variables
    [SerializeField] private CampfireAnimation campfireAnimation = null;
    #endregion

    #region Unity Events
    private void Awake()
    {

    }
    #endregion

    public void Initialise()
    {
        //objectHealth = startHealth;
        campfireAnimation.Restart();
    }

    public bool CanRespawn()
    {
        return alive;
    }

    public override void TakeDamage()
    {
        if (alive)
        {
            objectHealth -= 1;
            if (objectHealth <= 0)
            {
                Extinguish();
            }
        }
    }

    public void Extinguish()
    {
        if(alive)
        {
            alive = false;
            campfireAnimation.Extinguish();
        }
    }
}
