using System;
using UnityEngine;

public class RootAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition;
    }
}
