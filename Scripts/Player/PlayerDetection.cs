using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static PlayerDetection Instance; 

    [SerializeField] private float detectionRadius = 100f;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Collider DetectMonsterInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            // "Monster" 태그를 가진 오브젝트인지 확인
            if (collider.CompareTag("Monster"))
            {
                return collider;
            }
        }

        return null;
    }
}
