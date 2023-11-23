using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerManager playerManager;
    private InputManager inputManager;
    private AnimatorManager animatorManager;
    private PlayerBehaviorController playerBehaviorController;

    private Vector3 moveDirection;
    private Rigidbody playerRigidbody;
    public Transform cameraObject;

    [Header("Speed")]
    public float walkSpeed = 2;
    public float sprintSpeed = 7.5f;
    public float rotationSpeed = 15;
    public float MoveToAttackSpeed = 10f;

    [Header("Fall")]
    [SerializeField] private float inAirTime;
    [SerializeField] private float fallingVelocity;
    [SerializeField] private float leapingVelocity = 300;
    [SerializeField] private float rayCastHeightOffSet = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Flag")]
    public bool isSprinting;
    public bool isGrounded;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        animatorManager = GetComponent<AnimatorManager>();  
        playerManager = GetComponent<PlayerManager>();
        playerBehaviorController = GetComponent<PlayerBehaviorController>();
    }   

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
            return;

        if (playerBehaviorController.isBasicAttack)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
            moveDirection *= sprintSpeed;
        else
            moveDirection *= walkSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.rotation = playerRotation;    
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffSet;

        if (!isGrounded)
        {
            if (!playerManager.isInteracting)
                animatorManager.PlayTargetAnimation("Falling", true);

            inAirTime += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTime);
        }

        if(Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if(!isGrounded && !playerManager.isInteracting)
                animatorManager.PlayTargetAnimation("Landing", true);
                
            inAirTime = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    public void HandleMoveAndRotationToAttack()
    {
        var targetColider = PlayerDetection.Instance.DetectMonsterInRadius();

        if (targetColider == null)
            return;

        Transform targetTransform = targetColider.transform;
        Vector3 direction = targetTransform.position - transform.position;

        float backwardForce = 0.5f;
        Vector3 targetPosition = targetTransform.position;
        targetPosition -= direction * backwardForce;
        targetPosition.y = transform.position.y;
        transform.DOMove(targetPosition, 0.1f);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion playerRotation = targetRotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;
    }
}
