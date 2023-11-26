using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1HoldAttack : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    private const int damageAmount = 20;

    private void Start()
    {
        targets.Clear();
    }

    private void OnEnable()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void OnDisable()
    {
        foreach (Transform t in targets)
        {
            var playerInformation = t.GetComponent<PlayerInformation>();
            if (playerInformation != null)
            {
                playerInformation.Damage(damageAmount);
            }
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
