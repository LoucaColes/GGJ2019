using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField] private int startHealth;

    protected int objectHealth;

    protected bool alive;

    // Start is called before the first frame update
    void Start()
    {
        objectHealth = startHealth;
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Take Damage")]
    public virtual void TakeDamage()
    {
        objectHealth -= 1;
        if (objectHealth <= 0)
        {
            alive = false;
        }
    }

    public bool CanRespawn()
    {
        return alive;
    }
}
