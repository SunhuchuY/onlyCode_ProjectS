using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss1JumpAttack : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    private const int damageAmount = 20;
    private const float knockbackForce = 20f;

    private void Start()
    {
        targets.Clear();
    }

    private void OnEnable()
    {
        GetComponent<Collider>().enabled = false;
        targets.Clear();
    }

    private void OnDisable()
    {
        foreach (var t in targets)
        {
            var playerInformation = t.GetComponent<PlayerInformation>();
            var playerRigidbody = t.GetComponent<Rigidbody>();

            if (playerInformation != null)
            {
                playerInformation.Damage(damageAmount);
                ApplyKnockback(t.transform, playerRigidbody);
            }
        }
    }

    private void ApplyKnockback(Transform targetTransform, Rigidbody targetRigidbody)
    {
        if (targetTransform != null && targetRigidbody != null)
        {
            Vector3 knockbackDirection = (targetTransform.position - transform.position).normalized;
            targetRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var targetObject = other.gameObject;

        if (targetObject.CompareTag("Player"))
        {
            if (!targets.Contains(targetObject.transform))
                targets.Add(targetObject.transform);
        }
    }
}