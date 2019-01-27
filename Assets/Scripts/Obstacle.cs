using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Placeable
{
    public override bool TakeDamage()
    {
        objectHealth -= 1;
        if (objectHealth <= 0)
        {
            alive = false;
            Destroy(gameObject);
        }
        return alive;
    }
}
