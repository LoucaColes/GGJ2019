using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastUtility : MonoBehaviour
{
    public static bool TrySnapToPosition(Vector2 _position, Vector2 _direction, out Vector3 _snapped)
    {
        _direction.y *= 0.5f;
        _snapped = GridHelper.SnapToGrid(_position + _direction);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(_snapped, GridHelper.Size(), 0, Vector2.zero);
        return hits.Length == 0 ? true : false;
    }

    public static bool TryDamageAtPosition(Vector2 _position, Vector2 _direction)
    {
        _direction.y *= 0.5f;
        bool atLeastOneHit = false;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(_position + _direction, GridHelper.Size() * 0.5f, 0, Vector2.zero);
        for(int i = 0; i < hits.Length; ++i)
        {
            Player player = hits[i].transform.parent.GetComponent<Player>();
            if(player)
            {
                player.TakeDamage();
                atLeastOneHit = true;
                continue;
            }
            Placeable placeable = hits[i].transform.parent.GetComponent<Placeable>();
            if (placeable)
            {
                placeable.TakeDamage();
                atLeastOneHit = true;
                continue;
            }
        }
        return atLeastOneHit;
        
    }
}
