using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField] private int startHealth;

    protected int objectHealth;

    protected bool alive;

    public bool isAlive { get { return alive; } }

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
    public virtual bool TakeDamage()
    {
        objectHealth -= 1;
        if (objectHealth <= 0)
        {
            alive = false;
        }
        return alive;
    }

    public virtual bool CanRespawn()
    {
        return alive;
    }
}
