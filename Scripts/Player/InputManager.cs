using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    private PlayerControl playerControl;
    private AnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerBehaviorController playerBehaviorController;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 mouseInput;

    public float moveAmount { get; private set; }
    public float verticalInput;
    public float horizontalInput;
    public float mouseXInput;
    public float mouseYInput;

    private bool b_input;
    public bool isBasicAttackInput { get; private set; }

    private void Awake()
    {
        playerBehaviorController = GetComponent<PlayerBehaviorController>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        playerControl = new PlayerControl();
        playerControl.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerControl.PlayerMovement.MouseInput.performed += i => mouseInput = i.ReadValue<Vector2>();

        playerControl.PlayerActions.B.performed += i => SetInputFlag(true);
        playerControl.PlayerActions.B.canceled += i => SetInputFlag(false);

        playerControl.PlayerAttack.BasicAttack.performed += i => SetBasicAttackInputFlag(true);
        playerControl.PlayerAttack.BasicAttack.canceled += i => SetBasicAttackInputFlag(false);

        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void SetInputFlag(bool value)
    {
        b_input = value;
    }

    private void SetBasicAttackInputFlag(bool value)
    {
        isBasicAttackInput = value;
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleMouseInput();
        HandleSprintInput();
        HandleBasicAttackInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = b_input ? Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput)) :
                               Mathf.Clamp01((Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput)) / 2);

        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }

    private void HandleMouseInput()
    {
        mouseXInput = mouseInput.x;
        mouseYInput = mouseInput.y;
    }

    private void HandleSprintInput()
    {
        playerLocomotion.isSprinting = b_input ? true : false;
    }

    private void HandleBasicAttackInput()
    {
        if (isBasicAttackInput)
        {
            playerLocomotion.HandleMoveAndRotationToAttack();
            playerBehaviorController.BasicAttackCombo();
        }
    }

    public void HandleAllSetZero()
    {
        verticalInput = 0;
        horizontalInput = 0;
        moveAmount = 0;
        animatorManager.AllSetZero();
    }
}