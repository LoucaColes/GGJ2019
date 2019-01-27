using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Placeable
{
    #region Variables
    [SerializeField] private CampfireAnimation campfireAnimation = null;
    [SerializeField] private Light campfireLight = null;
    [SerializeField] private ParticleSystem campfireParticle = null;
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
        campfireParticle.Play();
        campfireLight.gameObject.SetActive(true);
    }

    public override bool TakeDamage()
    {
        if (alive)
        {
            objectHealth -= 1;
            if (objectHealth <= 0)
            {
                Extinguish();
            }
        }
        return alive;
    }

    public void Extinguish()
    {
        if(alive)
        {
            alive = false;
            campfireAnimation.Extinguish();
            campfireParticle.Stop();
            campfireLight.gameObject.SetActive(false);
        }
    }
}
