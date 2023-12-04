using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;



[Serializable]
public abstract class VMonsterAI : MonoBehaviour
{
    protected MonsterDetection monsterDetection;
    [SerializeField] protected float rotationSpeed, moveSpeed;

    protected Animator animator;
    protected Rigidbody rigidbody;
    protected Collider targetCollider;
    protected bool isDetect;

    protected virtual void Awake()
    {
        monsterDetection = GetComponent<MonsterDetection>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
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

    protected virtual void Attack()
    {
        SetAnimatorParameters(isAttack: true, isMove: false);
    }

    protected virtual void Detect()
    {
        Collider temp = monsterDetection.DetectPlayerToMove();
        isDetect = temp != null;

        if (isDetect)
            targetCollider = temp;
    }

    protected virtual void Idle()
    {
        SetAnimatorParameters(isAttack: false, isMove: false);
    }

    protected virtual void Move()
    {
        SetAnimatorParameters(isAttack: false, isMove: true);

        var direction = targetCollider.transform.position - transform.position;
        var moveDirection = (direction.normalized) * moveSpeed;
        moveDirection.y = 0;
        rigidbody.velocity = moveDirection;

        Debug.Log(moveDirection);

        var rotationDirection = direction.normalized;
        var targetRotation = Quaternion.LookRotation(rotationDirection);
        var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;
    }

    public void Dead()
    {
        
    }

    protected void SetAnimatorParameters(bool isAttack, bool isMove)
    {
        animator.SetBool("isAttack", isAttack);
        animator.SetBool("isMove", isMove);
    }
}

public class MonsterAI : MonoBehaviour
{
    
}
