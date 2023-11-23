using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1JumpAttack : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();

    private void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = false;
        targets.Clear();        
    }

    private void OnDisable()
    {
        foreach (Transform t in targets)
        {
            t.GetComponent<PlayerInformation>().Damage(20);
            Debug.Log(PlayerInformation.instance.curHp);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        var targetObject = other.gameObject;

        if (targetObject.CompareTag("Player"))
        {
            if(!targets.Contains(targetObject.transform))
                targets.Add(targetObject.transform);

        }
    }
}
