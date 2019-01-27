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
    [SerializeField] private Material materialPrefab = null;
    #endregion

    //Called from animator
    public void SetAnimationIndex(int _index)
    {
        //spriteRenderer.material = new Material(materialPrefab);
        spriteRenderer.sprite = sprites[_index];
        spriteRenderer.material.EnableKeyword("_NORMALMAP");
        spriteRenderer.material.SetTexture("_BumpMap", normal_textures[_index]);
        //Also change material
    }

    public void Sit()
    {
        animator.SetTrigger("Sit");
    }

    public void Walk()
    {
        animator.SetBool("IsWalking", true);
    }

    public void StandUp()
    {
        animator.SetTrigger("Standup");
        animator.SetBool("IsWalking", false);
    }

    public void Stop()
    {
        animator.SetBool("IsWalking", false);
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
