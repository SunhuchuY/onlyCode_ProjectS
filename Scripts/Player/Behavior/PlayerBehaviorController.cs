using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;


public class PlayerBehaviorController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private PlayerBehaviorContext context;

    private BasicAttackCombo_Cancle basicAttackCombo_Cancle = new BasicAttackCombo_Cancle();
    private BasicAttackCombo_Execute basicAttackCombo_Execute = new BasicAttackCombo_Execute();

    public bool isBasicAttack;
    private bool isOverlapBasicAttack = false;

    [SerializeField] private float basicAttackCoolTime = 3;
    private float basicAttackCurTime = 0;

    private const float minBasicAttackTime = 0.1f;
    private const int maxBasicAttackState = 3;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("BasicAttackState", 1);
    }

    private void Update()
    {
        StartCoroutine(BasicAttackUpdateCoroutine());
    }

    private System.Collections.IEnumerator BasicAttackUpdateCoroutine()
    {
        while (!isBasicAttack)
        {
            basicAttackCurTime += Time.deltaTime;

            if (basicAttackCurTime > basicAttackCoolTime)
            {
                animator.SetInteger("BasicAttackState", 1);
                basicAttackCurTime = 0;
            }

            yield return null;
        }
    }

    public void BasicAttackCombo()
    {
        if (!isBasicAttack && basicAttackCurTime > minBasicAttackTime)
        {
            isBasicAttack = true;
            return;
        }

        context?.Transition(basicAttackCombo_Execute);
        context?.BehaviorStart();
    }

    private void BasicAttackCombo_Cancle()
    {
        context?.Transition(basicAttackCombo_Cancle);
        context?.BehaviorStart();

        isBasicAttack = false;
    }

    public void BasicAttack_TurnState()
    {
        animator.SetInteger("BasicAttackState", animator.GetInteger("BasicAttackState") + 1);

        if (animator.GetInteger("BasicAttackState") < 1 || animator.GetInteger("BasicAttackState") > maxBasicAttackState)
            animator.SetInteger("BasicAttackState", 1);

        BasicAttackCombo_Cancle();
    }

    public void AttackStart()
    {
        PlayerSword.instance?.AttackStart();
    }

    public void AttackEnd()
    {
        PlayerSword.instance?.AttackEnd();
    }
}