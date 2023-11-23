using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss1 : MonoBehaviour, IMonsterAI
{
    [SerializeField] private MonsterDetection monsterDetection;

    [SerializeField] private Transform jumpAttackTransform;
    [SerializeField] private Transform attack1Transform;

    private Animator animator;
    private Collider targetCollider;
    private Rigidbody rigidbody;

    private bool isDetect = false;
    private bool isAttacking = false;

    [Header("Speed")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float jumpPower= 5;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isAttacking)
            return;

        Detect();

        if (isDetect)
        {
            if (monsterDetection.GetIsPossibleAttack(targetCollider))
                Attack();
            else
                Move();
        }
        else
            Idle();

    }

    public void Attack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isMove", false);
    }

    public void Detect()
    {
        var temp = monsterDetection.DetectPlayer();

        if (temp != null)
        {
            isDetect = true;
            targetCollider = temp;
        }
        else
            isDetect = false;
    }

    public void Idle()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isMove", false);
    }

    public void Move()
    {
        animator.SetBool("isMove", true);

        Vector3 direction = targetCollider.transform.position - transform.position;

        Vector3 moveDirection = (direction.normalized) * moveSpeed;
        moveDirection.y = 0;
        rigidbody.velocity = moveDirection;

        Vector3 rotationDirection = direction.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;
    }


    public void JumpAttackStartEvent()
    {
        Vector3 rotationDirection = (targetCollider.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = targetRotation;

        jumpAttackTransform.position = PlayerInformation.instance.transform.position;
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
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                //jumpAttackTransform.GetComponent<BoxCollider>().enabled = true;
                animator.SetBool("isJumpAttack", true);

            });

        //yield return new WaitForSeconds(delay + 0.5f);
        //jumpAttackTransform.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
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
}
