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
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCamera();

        isInteracting = animator.GetBool("isInteracting");
    }
}
