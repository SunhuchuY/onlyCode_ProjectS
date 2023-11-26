using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 100f;
    [SerializeField] private float toAttackRadius = 10f;

    // 플레이어를 감지하여 이동할 때 사용
    public Collider DetectPlayerToMove()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (IsPlayerCollider(collider))
            {
                return collider;
            }
        }

        return null;
    }

    // 특정 대상이 공격 가능한 범위 내에 있는지 확인
    public bool GetIsPossibleAttack(Collider target)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, toAttackRadius);

        return System.Array.Exists(colliders, collider => collider == target);
    }

    // 특정 지점에 대한 방향에 플레이어가 있는지 확인
    public bool GetIsForwardPlayer(Vector3 targetPosition, Collider target)
    {
        Vector3 raycastDirection = targetPosition - transform.position;

        // 레이캐스트를 이용하여 특정 지점에 플레이어가 있는지 확인
        return Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit) && IsPlayerCollider(hit.collider);
    }

    // Collider가 플레이어인지 확인
    private bool IsPlayerCollider(Collider collider)
    {
        return collider.CompareTag("Player") && collider.gameObject.activeSelf;
    }
}
