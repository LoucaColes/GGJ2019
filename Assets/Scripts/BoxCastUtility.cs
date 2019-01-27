using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastUtility : MonoBehaviour
{
    public enum BoxCastHitType
    {
        NOTHING,
        PLAYER,
        PLACEABLE,
        FIRE
    }

    public static bool TrySnapToPosition(Vector2 _position, Vector2 _direction, out Vector3 _snapped)
    {
        _direction.y *= 0.5f;
        _snapped = GridHelper.SnapToGrid(_position + _direction);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(_snapped, GridHelper.Size(), 0, Vector2.zero);
        return hits.Length == 0 ? true : false;
    }

    //public static bool TryDamageAtPosition(Vector2 _position, Vector2 _direction)
    //{
    //    _direction.y *= 0.5f;
    //    bool atLeastOneHit = false;
    //    RaycastHit2D[] hits = Physics2D.BoxCastAll(_position + _direction, GridHelper.Size() * 0.5f, 0, Vector2.zero);
    //    Debug.Log("Attempting to damage: " + hits.Length);
    //    for (int i = 0; i < hits.Length; ++i)
    //    {
    //        Debug.Log("Hit: " + hits[i].transform.root.name);
    //        Player player = hits[i].transform.root.GetComponent<Player>();
    //        if (player)
    //        {
    //            player.TakeDamage();
    //            atLeastOneHit = true;
    //            continue;
    //        }
    //        Placeable placeable = hits[i].transform.root.GetComponent<Placeable>();
    //        if (placeable)
    //        {
    //            placeable.TakeDamage();
    //            atLeastOneHit = true;
    //            continue;
    //        }
    //    }
    //    return atLeastOneHit;

    //}

    public static BoxCastHitType TryActionAtPosition(Vector2 _position, Vector2 _direction, string _ignoreTag)
    {
        BoxCastHitType hitType = BoxCastHitType.NOTHING;
        _direction.y *= 0.5f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(_position + _direction, GridHelper.Size() * 0.5f, 0, Vector2.zero);

        for (int i = 0; i < hits.Length; ++i)
        {
            Campfire campfire = hits[i].transform.GetComponent<Campfire>();
            if (campfire && hits[i].transform.tag != _ignoreTag)
            {
                if (campfire.isAlive)
                {
                    campfire.TakeDamage();
                    hitType = BoxCastHitType.FIRE;
                    break;
                }
                continue;
            }

            Obstacle placeable = hits[i].transform.root.GetComponent<Obstacle>();
            if (placeable)
            {
                placeable.TakeDamage();
                hitType = BoxCastHitType.PLACEABLE;
                break;
            }

            Player player = hits[i].transform.root.GetComponent<Player>();
            if (player && hits[i].transform.root.tag != _ignoreTag)
            {
                player.TakeDamage();
                hitType = BoxCastHitType.PLAYER;
                break;
            }
        }
        print(hitType);
        return hitType;

    }
}
