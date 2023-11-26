using Cinemachine.Utility;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Boss1 : VMonsterAI
{

    [SerializeField] private Transform jumpAttackTransform;
    [SerializeField] private Transform attack1Transform;
    [SerializeField] private Transform holdAttackTransform, holdAttackFillTransform, holdAttackEndPosition;

    [SerializeField] private float jumpPower= 5;

    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Update()
    {
        base.Update();
    }

    public void JumpAttackStartEvent()
    {
        var rotationDirection = (targetCollider.transform.position - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(rotationDirection);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = targetRotation;


        var targetPosition = PlayerInformation.instance.transform.position;
        targetPosition.y += 0.5f;
        jumpAttackTransform.position = targetPosition;
        jumpAttackTransform.gameObject.SetActive(true);
    }

    public void JumpAttackEvent()
    {
        StartCoroutine(JumpAttackCoroutine());
    }

    private IEnumerator JumpAttackCoroutine()
    {
        float delay = 1;

        Vector3 midPoint = (transform.position + jumpAttackTransform.position) / 2f;
        midPoint.y += jumpPower; // 포물선의 높이 조절 (원하는 높이로 조절)

        // DOJump를 사용하여 포물선 이동
        transform.DOJump(jumpAttackTransform.position, jumpPower, 1, delay)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                //jumpAttackTransform.GetComponent<BoxCollider>().enabled = true;
                animator.SetBool("isJumpAttack", true);

            });

        //yield return new WaitForSeconds(delay + 0.5f);
        //jumpAttackTransform.gameObject.SetActive(false);
        yield return new WaitForSeconds(7);
        animator.SetBool("isJumpAttack", false);
    }

    public void JumpAttackDamageEvent()
    {
        jumpAttackTransform.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(JumpAttackDamageEventDelay());
    }

    IEnumerator JumpAttackDamageEventDelay()
    {
        CameraShake.instance.CameraShakeFuntion(1f, 1);
        yield return new WaitForSeconds(0.1f);
        jumpAttackTransform.gameObject.SetActive(false);
    }

    public void Attack1Event()
    {
        StartCoroutine(Attack1Coroutine());
    }

    private IEnumerator Attack1Coroutine()
    {
        attack1Transform.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attack1Transform.gameObject.SetActive(false);
    }

    public void HoldAttackTryStartEvent()
    {
        holdAttackTransform.gameObject.SetActive(true);
        holdAttackFillTransform.gameObject.SetActive(true);

        var targetPosition = transform.position;
        targetPosition.y += 1;
        holdAttackTransform.position = targetPosition;

        var rotationDirection = (targetCollider.transform.position - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(rotationDirection);
        targetRotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        holdAttackTransform.rotation = targetRotation;

        holdAttackFillTransform.localScale = new Vector2(1,0);

        holdAttackFillTransform.DOScaleY(1, 4)
        .SetEase(Ease.Linear)
        .OnStart(() =>
        {
            var rotationDirection = (holdAttackEndPosition.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(rotationDirection);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = targetRotation;
        })
        .OnUpdate(() =>
        {
        })
        .OnComplete(() => 
        {
            animator.CrossFade("holdAttackTry2", 0.1f);
            holdAttackTransform.gameObject.SetActive(false);
        });

    }

    public void HoldAttackTryEvent()
    {
        Tween moveTween = null;
        var targetPosition = holdAttackEndPosition.position;
        targetPosition.y = transform.position.y;

        moveTween = transform.DOMove(targetPosition, 1f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (monsterDetection.GetIsForwardPlayer(transform.position + transform.forward, targetCollider))
                {
                    animator.CrossFade("holdAttacking", 0.1f);
                    moveTween.Kill();
                    return;
                }
            })
            .OnComplete(() =>
            {
                animator.CrossFade("Idle", 0.1f);
            });
    }


    Quaternion originalTargetRotation;
    Vector3 originalTargetPosition;

    public void HoldAttackStartEvent()
    {
        PlayerManager.instance.isSturn = true;

        #region Position
        originalTargetPosition = targetCollider.transform.position;

        var targetPosition = transform.position + (transform.forward );
        targetPosition.y = targetCollider.transform.position.y + 1.2f;
        targetCollider.transform.position = targetPosition;
        #endregion

        #region Rotation
        originalTargetRotation = targetCollider.transform.rotation;

        Vector3 newEulerAngles = new Vector3(-70, originalTargetRotation.eulerAngles.y, originalTargetRotation.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(newEulerAngles);

        targetCollider.transform.rotation = newRotation;
        #endregion 

        targetCollider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void HoldAttackEvent()
    {
        targetCollider.GetComponent<PlayerInformation>().Damage(20);

    }

    public void HoldAttackEndEvent()
    {
        PlayerManager.instance.isSturn = false;
        targetCollider.transform.position = originalTargetPosition;
        targetCollider.transform.rotation = originalTargetRotation;
        animator.CrossFade("Idle", 0.1f);

        targetCollider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        targetCollider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }
} 

/* public class Boss1 : VMonsterAI
{
    [SerializeField] private Transform jumpAttackTransform, attack1Transform;
    [SerializeField] private Transform holdAttackTransform, holdAttackFillTransform, holdAttackEndPosition;
    [SerializeField] private float jumpPower = 5;

    private const float holdAttackTryDuration = 4f;
    private const float holdAttackFillDuration = 4f;
    private const float holdAttackTryFadeDuration = 0.1f;
    private const float holdAttackMoveDuration = 1f;

    private Quaternion originalTargetRotation;
    private Vector3 originalTargetPosition;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void RotateTowardsTarget(Transform targetTransform)
    {
        var rotationDirection = (targetTransform.position - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(rotationDirection);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = targetRotation;
    }

    private IEnumerator JumpAttackCoroutine()
    {
        float delay = 1;

        Vector3 midPoint = (transform.position + jumpAttackTransform.position) / 2f;
        midPoint.y += jumpPower;

        transform.DOJump(jumpAttackTransform.position, jumpPower, 1, delay)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                animator.SetBool("isJumpAttack", true);
            });

        yield return new WaitForSeconds(7);
        animator.SetBool("isJumpAttack", false);
        jumpAttackTransform.gameObject.SetActive(false);
    }

    private void ApplyKnockback(Transform targetTransform, Rigidbody targetRigidbody)
    {
        if (targetTransform != null && targetRigidbody != null)
        {
            Vector3 knockbackDirection = (targetTransform.position - transform.position).normalized;
            targetRigidbody.AddForce(knockbackDirection * 20, ForceMode.Impulse);
        }
    }

    private void ActivateAttack(Transform attackTransform, float delay)
    {
        attackTransform.gameObject.SetActive(true);
        StartCoroutine(DeactivateAfterDelay(attackTransform.gameObject, delay));
    }

    private IEnumerator DeactivateAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private void ScaleFillBar(Transform fillTransform, float duration)
    {
        fillTransform.localScale = new Vector2(1, 0);

        fillTransform.DOScaleY(1, duration)
            .SetEase(Ease.Linear);
    }

    private void MoveToPosition(Transform targetTransform, float duration, TweenCallback onComplete = null)
    {
        transform.DOMove(targetTransform.position, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (monsterDetection.GetIsForwardPlayer(transform.position + transform.forward, targetCollider))
                {
                    onComplete?.Invoke();
                }
            })
            .OnComplete(onComplete);
    }

    public void JumpAttackStartEvent()
    {
        RotateTowardsTarget(targetCollider.transform);

        var targetPosition = PlayerInformation.instance.transform.position;
        targetPosition.y += 0.5f;
        jumpAttackTransform.position = targetPosition;
        jumpAttackTransform.gameObject.SetActive(true);
    }

    public void JumpAttackEvent()
    {
        StartCoroutine(JumpAttackCoroutine());
    }

    public void JumpAttackDamageEvent()
    {
        jumpAttackTransform.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DeactivateAfterDelay(jumpAttackTransform.gameObject, 0.1f));
    }

    public void Attack1Event()
    {
        ActivateAttack(attack1Transform, 0.2f);
    }

    public void HoldAttackTryStartEvent()
    {
        holdAttackTransform.gameObject.SetActive(true);
        holdAttackFillTransform.gameObject.SetActive(true);

        var targetPosition = transform.position;
        targetPosition.y += 1;
        holdAttackTransform.position = targetPosition;

        RotateTowardsTarget(targetCollider.transform);

        holdAttackFillTransform.localScale = new Vector2(1, 0);

        ScaleFillBar(holdAttackFillTransform, holdAttackFillDuration);

        MoveToPosition(holdAttackEndPosition, holdAttackTryDuration, () =>
        {
            animator.CrossFade("holdAttackTry2", holdAttackTryFadeDuration);
            holdAttackTransform.gameObject.SetActive(false);
        });
    }
} */

