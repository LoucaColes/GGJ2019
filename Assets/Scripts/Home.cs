using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    #region Variables
    private const int X = 5;
    private const int Y = 7;

    [SerializeField] private GridHelper.Corner corner = GridHelper.Corner.NORTH_WEST;

    private string mPlayerID;
    private Vector2 mCentre;
    private Vector2 mExtents;
    #endregion

    #region Unity Events
    private void Start()
    {
        GridHelper.HomeBounds(corner, X, Y, out mCentre, out mExtents);
    }

    private void Update()
    {
        Debug.DrawLine(new Vector3(mCentre.x - mExtents.x, mCentre.y + mExtents.y), new Vector3(mCentre.x - mExtents.x, mCentre.y - mExtents.y), Color.green);
        Debug.DrawLine(new Vector3(mCentre.x - mExtents.x, mCentre.y + mExtents.y), new Vector3(mCentre.x + mExtents.x, mCentre.y + mExtents.y), Color.green);
        Debug.DrawLine(new Vector3(mCentre.x + mExtents.x, mCentre.y - mExtents.y), new Vector3(mCentre.x + mExtents.x, mCentre.y + mExtents.y), Color.green);
        Debug.DrawLine(new Vector3(mCentre.x + mExtents.x, mCentre.y - mExtents.y), new Vector3(mCentre.x - mExtents.x, mCentre.y - mExtents.y), Color.green);
    }
    #endregion

    public Vector2 Initialise(string _id)
    {
        mPlayerID = _id;
        GridHelper.HomeBounds(corner, X, Y, out mCentre, out mExtents);
        return mCentre;
    }

    private bool WithinHome(string _playerID, Vector2 _pos)
    {
        if (mPlayerID == _playerID)
        {
            //Assume the position has already been fed through the grid snapping
            if (_pos.x < mCentre.x + mExtents.x && _pos.x > mCentre.x - mExtents.x)
            {
                if (_pos.y < mCentre.y + mExtents.y && _pos.y > mCentre.y - mExtents.y)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
