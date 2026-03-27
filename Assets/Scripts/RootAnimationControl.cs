using UnityEngine;

public class RootAnimationControl : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        Vector3 delta = animator.deltaPosition;

        transform.position += delta;
        transform.rotation = animator.rootRotation;
    }
}
