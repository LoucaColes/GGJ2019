using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should ideally only contain static functionality in the form of
//helper functions that can be used to snap and sort within the grid
public class GridHelper
{
    #region Enums
    public enum Corner
    {
        NORTH_WEST,
        SOUTH_EAST,
        NORTH_EAST,
        SOUTH_WEST
    }
    #endregion

    private static Vector2 size;
    private static Vector2 extents;

    //Extents is actually half extents but whatever
    public static void Define(float _size, Vector2 _extents)
    {
        size = new Vector2(1 * _size, 0.5f * _size) * _size;
        extents = _extents;
    }

    public static void Update()
    {
        return;
        for (float i = -extents.x; i <= extents.x; i += size.x)
        {
            Debug.DrawLine(new Vector3(i, -extents.y), new Vector3(i, extents.y));
        }

        for (float i = -extents.y; i <= extents.y; i += size.y)
        {
            Debug.DrawLine(new Vector3(-extents.x, i), new Vector3(extents.x, i));
        }

        //Vector3 test = SnapToGrid(new Vector3(-3.1f, -1.6f, 0));
        //Debug.DrawLine(new Vector3(test.x - 0.5f, test.y), new Vector3(test.x + 0.5f, test.y), Color.red);
        //Debug.DrawLine(new Vector3(test.x, test.y - 0.5f), new Vector3(test.x, test.y + 0.5f), Color.red);
    }

    public static Vector3 SnapToGrid(Vector3 _pos)
    {
        _pos.x /= size.x;
        _pos.y /= size.y;
        _pos.x = (Mathf.FloorToInt(_pos.x) * size.x) + (size.x * 0.5f);
        _pos.y = (Mathf.FloorToInt(_pos.y) * size.y) + (size.y * 0.5f);
        return _pos;
    }

    public static void HomeBounds(Corner _corner, int _x, int _y, out Vector2 _centre, out Vector2 _extents)
    {
        _centre = Vector2.zero;
        //Use half extents for ease
        _extents = new Vector2(_x * size.x * 0.5f, _y * size.y * 0.5f);

        switch (_corner)
        {
            case Corner.NORTH_WEST:
                _centre = new Vector2(-extents.x + _extents.x, extents.y - _extents.y);
                break;
            case Corner.SOUTH_EAST:
                _centre = new Vector2(extents.x - _extents.x, -extents.y + _extents.y);
                break;
            case Corner.NORTH_EAST:
                _centre = new Vector2(extents.x - _extents.x, extents.y - _extents.y);
                break;
            case Corner.SOUTH_WEST:
                _centre = new Vector2(-extents.x + _extents.x, -extents.y + _extents.y);
                break;
            default:
                break;
        }
    }
}
