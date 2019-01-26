using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite[] sprites = null;
    [SerializeField] private Texture2D[] normal_textures = null;
    #endregion

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        Stop();
        yield return new WaitForSeconds(1.0f);
        Stab();
    }
    //Called from animator
    public void SetAnimationIndex(int _index)
    {
        spriteRenderer.sprite = sprites[_index];
        //spriteRenderer.material.SetTexture("_NORMALMAP", normal_textures[_index]);
        //Also change material
    }

    public void Sit()
    {
        animator.SetTrigger("Sit");
    }

    public void Walk()
    {
        animator.SetTrigger("WalkAndStop");
        animator.SetBool("IsMoving", true);
    }

    public void Stop()
    {
        animator.SetTrigger("WalkAndStop");
        animator.SetBool("IsMoving", false);
    }

    public void Stab()
    {
        animator.SetTrigger("Stab");
    }

    public void Water()
    {
        animator.SetTrigger("Water");
    }
}
