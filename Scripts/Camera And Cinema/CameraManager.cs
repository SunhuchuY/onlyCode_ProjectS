using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private InputManager inputManager;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    [SerializeField] private LayerMask collisionLayers;

    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cameraTransform;
    private Transform targetTransform;

    private float defaultPosition = 0;

    [SerializeField] private float cameraCollisionRadius = 2;
    [SerializeField] private float cameraCollisionOffSet = 0.2f;
    [SerializeField] private float minimumCollisionOffSet = 0.2f;

    [SerializeField] private float cameraFollowSpeed = 0.2f;
    [SerializeField] private float cameraPivotSpeed = 2f;
    [SerializeField] private float cameraLookSpeed = 2f;

    [SerializeField] private float lookAngle;
    [SerializeField] private float pivotAngle;
    [SerializeField] private float mininmumPivotAngle = -35;
    [SerializeField] private float maximumPivotAngle = 35;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();
    }

    public void HandleAllCamera()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += (inputManager.mouseXInput * cameraLookSpeed);
        pivotAngle -= (inputManager.mouseYInput * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, mininmumPivotAngle, maximumPivotAngle);

        #region Mouse X
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        #endregion

        #region Mouse Y
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
        #endregion
    }

    private void HandleCameraCollision()
    {

        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = (cameraTransform.position - cameraPivot.position);
        direction.Normalize();

        if(Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            Debug.Log("Hit");
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition -= distance - cameraCollisionOffSet;
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffSet)
        {
            targetPosition -= minimumCollisionOffSet;
        }


        Debug.Log(targetPosition);
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

}
