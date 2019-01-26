using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite[] sprites = null;
    #endregion

    //Called from animator
    public void SetAnimationIndex(int _index)
    {
        spriteRenderer.sprite = sprites[_index];
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
