using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] private Transform followTransform;

    private void Update()
    {
        Vector3 targetPosition = followTransform.position;
        targetPosition.y = transform.position.y;

        transform.position = targetPosition;    
    }
}
