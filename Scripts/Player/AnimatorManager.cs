using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    private int horizontal;
    private int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("horizontal");
        vertical = Animator.StringToHash("vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        /*float snappedHorizontal;
        float snappedVertical;

        #region Snap Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f)
            snappedHorizontal = 1f;
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f)
            snappedHorizontal = -1f;
        else
            snappedHorizontal = 0;
        #endregion

        #region Snap Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
            snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f)
            snappedVertical = 1f;
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
            snappedVertical = -0.5f;
        else if (verticalMovement < -0.55f)
            snappedVertical = -1f;
        else
            snappedVertical = 0;
        #endregion */

        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
    }

    public void AllSetZero()
    {
        animator.SetFloat(horizontal, 0);
        animator.SetFloat(vertical, 0);
    }
}
