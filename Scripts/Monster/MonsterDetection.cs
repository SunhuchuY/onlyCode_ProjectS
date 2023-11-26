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

    // �÷��̾ �����Ͽ� �̵��� �� ���
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

    // Ư�� ����� ���� ������ ���� ���� �ִ��� Ȯ��
    public bool GetIsPossibleAttack(Collider target)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, toAttackRadius);

        return System.Array.Exists(colliders, collider => collider == target);
    }

    // Ư�� ������ ���� ���⿡ �÷��̾ �ִ��� Ȯ��
    public bool GetIsForwardPlayer(Vector3 targetPosition, Collider target)
    {
        Vector3 raycastDirection = targetPosition - transform.position;

        // ����ĳ��Ʈ�� �̿��Ͽ� Ư�� ������ �÷��̾ �ִ��� Ȯ��
        return Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit) && IsPlayerCollider(hit.collider);
    }

    // Collider�� �÷��̾����� Ȯ��
    private bool IsPlayerCollider(Collider collider)
    {
        return collider.CompareTag("Player") && collider.gameObject.activeSelf;
    }
}
