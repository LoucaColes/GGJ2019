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
        Debug.Log("Attempting to damage: " + hits.Length);
        for (int i = 0; i < hits.Length; ++i)
        {
            Debug.Log("Hit: " + hits[i].transform.root.name);
            Player player = hits[i].transform.root.GetComponent<Player>();
            if (player)
            {
                player.TakeDamage();
                atLeastOneHit = true;
                continue;
            }
            Placeable placeable = hits[i].transform.root.GetComponent<Placeable>();
            if (placeable)
            {
                placeable.TakeDamage();
                atLeastOneHit = true;
                continue;
            }
        }
        return atLeastOneHit;

    }

    public static bool TryDamageAtPosition(Vector2 _position, Vector2 _direction, string _ignoreTag)
    {
        _direction.y *= 0.5f;
        bool atLeastOneHit = false;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(_position + _direction, GridHelper.Size() * 0.5f, 0, Vector2.zero);
        Debug.Log("Attempting to damage: " + hits.Length);
        Debug.Log("Ignore Tag: " + _ignoreTag);
        for (int i = 0; i < hits.Length; ++i)
        {
            Debug.Log("Hit: " + hits[i].transform.root.name);
            Debug.Log("Hit Tag: " + hits[i].transform.root.tag);
            Player player = hits[i].transform.root.GetComponent<Player>();
            if (player && hits[i].transform.root.tag != _ignoreTag)
            {
                player.TakeDamage();
                atLeastOneHit = true;
                continue;
            }
            Placeable placeable = hits[i].transform.root.GetComponent<Placeable>();
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
