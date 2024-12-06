using Cinemachine;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TestPlayerCtr : MonoBehaviour
{

    public PunchInputAction punchInput;
    public Transform cam;
    public CinemachineFreeLook freeLookCam;


    private float turnSmoothTime = 700;

    private Vector3 direction;
    private Vector2 input;
    private float smoothTime = 0.05f;
    private float currentVelocity;

    public float gravityMultiplier = 1.0f;
    private float gravity = -9.81f;
    private float velocity;

    private float jumpPower = 4;

    [SerializeField] private Movementa movement;

    public CharacterController cc;

    PositionInfo _positionInfo = new PositionInfo();
    public PositionInfo PosInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;
        }
    }

    [SerializeField]
    protected EObjectState _objectState = EObjectState.None;
    public virtual EObjectState ObjectState
    {
        get { return PosInfo.State; }
        set
        {
            if (_objectState == value)
                return;

            _objectState = value;
            PosInfo.State = value;
            UpdateAnimation();
        }
    }


    protected void Awake()
    {
        punchInput = new PunchInputAction();
        punchInput.Player.Move.started += Move;
        punchInput.Player.Move.performed += Move;
        punchInput.Player.Move.canceled += Move;

        punchInput.Player.Jump.started += Jump;
        punchInput.Player.Jump.performed += Jump;
        punchInput.Player.Jump.canceled += Jump;

        punchInput.Player.Sprint.started += Sprint;
        punchInput.Player.Sprint.performed += Sprint;
        punchInput.Player.Sprint.canceled += Sprint;

        punchInput.Player.Punch.started += Punch;
        punchInput.Player.Punch.performed += Punch;
        punchInput.Player.Punch.canceled += Punch;
    }

    private void OnEnable()
    {
        punchInput.Player.Enable();
    }

    private void OnDisable()
    {
        punchInput.Player.Disable();
    }

    private void Start()
    {
        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;
    }

    private void Update()
    {
        //PlayerRotation();
        //PlayerGravity();
        //PlayerMovement();

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cc.Move(move * Time.deltaTime * 5);
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
        float targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

        cc.Move(direction * movement.currentSpeed * Time.deltaTime);
    }



    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        direction = new Vector3(input.x, 0f, input.y).normalized;

        cc.Move(direction * movement.currentSpeed * Time.deltaTime);
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

    protected virtual void UpdateAnimation()
    {
        switch (ObjectState)
        {
            case EObjectState.Idle:
                ChangeAnimation("Idle");
                break;
            case EObjectState.Move:
                ChangeAnimation("Run");
                break;
            case EObjectState.Skill:
                ChangeAnimation("Punch");
                break;
            case EObjectState.Dead:
                ChangeAnimation("Dead");
                break;
        }
    }

    public void ChangeAnimation(string anim, float crossfade = 0.01f)
    {
        //if (Anim == null) return;

        //if (currentAnimation != anim)
        //{
        //    currentAnimation = anim;
        //    Anim.CrossFade(anim, crossfade);
        //}
    }

}

[Serializable]
public struct Movementa
{
    public float speed;
    public float multiplier;
    public float acceleration;

    [HideInInspector] public bool isSprinting;
    [HideInInspector] public float currentSpeed;
}
