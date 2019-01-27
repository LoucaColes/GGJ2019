using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite[] sprites = null;

    [SerializeField] private Material prefab = null;
    [SerializeField] private Texture normal = null;
    #endregion

    //Called from animator
    public void SetAnimationIndex(int _index)
    {
        spriteRenderer.sprite = sprites[_index];

        if(_index == 15)
        {
            spriteRenderer.material = prefab;
            spriteRenderer.sprite = sprites[_index];
            spriteRenderer.material.EnableKeyword("_NORMALMAP");
            spriteRenderer.material.SetTexture("_BumpMap", normal);
        }
    }

    public void Extinguish()
    {
        animator.SetTrigger("Extinguish");
    }

    public void Restart()
    {
        animator.SetTrigger("Restart");
    }

}
