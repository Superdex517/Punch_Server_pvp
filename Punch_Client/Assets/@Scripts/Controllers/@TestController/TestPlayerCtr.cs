using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerCtr : MonoBehaviour
{
    public CharacterController cc;
    public Transform cam;
    public CinemachineFreeLook freeLookCam;
    public float gravityMultiplier = 3.0f;
    public float jumpPower;

    private float lookAngle;
    private float turnSmoothTime = 500;
    private float turnSmoothVelocity;
    private float speed = 5f;
    private float gravity = -9.81f;
    private float velocity;
    private Vector3 moveVelocity;
    private Vector3 direction;
    private Vector2 input;

    [SerializeField] private Movement movement;
    

    private void Start()
    {
        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;
    }

    private void Update()
    {
        PlayerRotation();
        PlayerGravity();
        PlayerMovement();
    }
    private void PlayerGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        direction.y = velocity;
    }

    private void PlayerRotation()
    {
        if (input.sqrMagnitude == 0)
            return;

        direction = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f) * new Vector3(input.x, 0f, input.y);
        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSmoothTime * Time.deltaTime);
    }

    private void PlayerMovement()
    {
        //float targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;
        //movement.currentSpeed= Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

        //cc.Move(direction * movement.currentSpeed * Time.deltaTime);
    }



    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y).normalized;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (!IsGrounded())
            return;

        velocity += jumpPower;
    }

    public void Punch(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        //movement.isSprinting = context.started || context.performed;
    }

    private bool IsGrounded() => cc.isGrounded;
}

//[Serializable]
//public struct Movement
//{
//    public float speed;
//    public float multiplier;
//    public float acceleration;

//    [HideInInspector] public bool isSprinting;
//    [HideInInspector] public float currentSpeed;
//}
