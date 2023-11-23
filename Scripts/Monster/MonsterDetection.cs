using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 100f;
    [SerializeField] private float toAttackRadius = 10f;


    public Collider DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.gameObject.activeSelf)
            {
                return collider;
            }
        }

        return null;
    }

    public bool GetIsPossibleAttack(Collider target)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, toAttackRadius);

        foreach (Collider collider in colliders)
        {
            if (collider == target)
            {
                return true;
            }
        }

        return false;
    }
}
