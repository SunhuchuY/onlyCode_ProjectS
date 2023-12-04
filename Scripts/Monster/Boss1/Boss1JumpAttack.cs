using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Boss1JumpAttack : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float knockbackForce = 0.001f;
    [SerializeField] private int sturnDuration = 3000;

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
                Debug.Log("Test");
                ApplyKnockback(t.transform, playerRigidbody);
            }
        }
    }

    private async void ApplyKnockback(Transform targetTransform, Rigidbody targetRigidbody)
    {
        if (targetTransform != null && targetRigidbody != null)
        {
            Vector3 knockbackDirection = (targetTransform.position - transform.position).normalized;
            knockbackDirection.y += knockbackForce;
            targetRigidbody.AddForce(knockbackDirection, ForceMode.Impulse);

            await Sturn();
        }
    }

    private async Task Sturn()
    {
        PlayerManager.instance.isSturn = true;
        await Task.Delay(sturnDuration); 
        PlayerManager.instance.isSturn = false;
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