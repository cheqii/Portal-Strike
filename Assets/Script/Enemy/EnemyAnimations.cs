using System;
using System.Collections;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Blend Tree Variables")]
    [SerializeField] private float idleBlend = 0f;
    [SerializeField] private float runBlend = 0.5f;
    [SerializeField] private float attackBlend = 1.0f;

    private float lerpTarget;
    
    #region - Normal Animation -

    public void TriggerIdleAnim()
    {
        lerpTarget = idleBlend;
    }
    
    public void TriggerAttackAnim()
    {
        lerpTarget = attackBlend;
    }

    public void TriggerWalkAnim()
    {
        animator.SetTrigger("isWalk");
    }

    public void TriggerRunAnim()
    {
        lerpTarget = runBlend;
    }

    public void TriggerGetHitAnim()
    {
        animator.SetTrigger("isGetHit");
    }

    //public void TriggerDieAnim()
    //{
    //    animator.SetTrigger("isDie");
    //}

    public void BlendTree()
    {
        animator.SetTrigger("isBlendTree");
    }

    #endregion


    private void FixedUpdate()
    {
        if (animator.GetFloat("Blend") / lerpTarget * 100 <= 97)
        {
            animator.SetFloat("Blend", Mathf.Lerp(animator.GetFloat("Blend"), lerpTarget, 0.1f));
        }
    }

}
