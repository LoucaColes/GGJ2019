using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    #region
    [Header("References")]
    [SerializeField] private Transform refPoint = null;

    private const bool USE_POSITION_Z = false;

    private SpriteRenderer mSpriteRenderer;
    #endregion

    #region Unity Events
    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        if (!refPoint)
            refPoint = transform;
    }

    public void Update()
    {
        if(transform.hasChanged)
        {
            if(USE_POSITION_Z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, refPoint.position.y);
            }
            else
            {
                mSpriteRenderer.sortingOrder = -Mathf.FloorToInt(refPoint.position.y * 1000);
            }
        }
    }
    #endregion
}
