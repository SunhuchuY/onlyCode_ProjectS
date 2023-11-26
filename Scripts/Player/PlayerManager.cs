using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;   

    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private CameraManager cameraManager;

    public Animator animator;

    public bool isInteracting;
    public bool isSturn;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        animator.SetBool("isSturn", isSturn);

        if (isSturn)
            return;

        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        if (isSturn)
            return;

        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if (isSturn)
            return;

        cameraManager.HandleAllCamera();

        isInteracting = animator.GetBool("isInteracting");
    }
}
