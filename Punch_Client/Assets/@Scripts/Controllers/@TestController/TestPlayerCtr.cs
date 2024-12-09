using Cinemachine;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TestPlayerCtr : MonoBehaviour
{
    public PunchInputAction punchInput;
    private CharacterController _cc;
    private Animator _animator;
    public Transform cam;
    public CinemachineFreeLook freeLookCam;
    public EObjectState ObjectState = EObjectState.None;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _appliedMovement;
    private Vector3 currentRunMovement;
    private float _runMultiplier = 3.0f;

    private float _gravity = -9.8f;
    private float _groundedGravity = -0.05f;
    private string currentAnimation = "";

    private bool isRun;
    private bool isSprint;

    private float initalJumpVelocity;
    private float maxJumpHeight = 2.0f;
    private float maxJumpTime = 0.75f;

    private bool _isJumpPressed = false;
    private bool _isJumping = false;
    private bool _isMovementPressed = false;

    private bool isPunch;

    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public CharacterController CC { get { return _cc; } }
    public bool IsMovementPressed { get; set; }
    public bool IsRunPressed { get; set; }
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } set { _isJumpPressed = value; } }
    public float InitalJumpVelocity { get { return initalJumpVelocity; } set { initalJumpVelocity = value; } }
    public float GroundedGravity { get { return _groundedGravity; } }
    public float Gravity { get { return _gravity; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float RunMultiplier { get { return _runMultiplier; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }

    protected void Awake()
    {
        punchInput = new PunchInputAction();
        _cc = GetComponent<CharacterController>();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        punchInput.Player.Move.started += Move;
        punchInput.Player.Move.performed += Move;
        punchInput.Player.Move.canceled += Move;
            
        punchInput.Player.Sprint.started += Sprint;
        punchInput.Player.Sprint.canceled += Sprint;

        punchInput.Player.Jump.started += Jump;
        punchInput.Player.Jump.canceled += Jump;

        punchInput.Player.Punch.started += Punch;
        punchInput.Player.Punch.performed += Punch;
        punchInput.Player.Punch.canceled += Punch;

        SetJumpVariables();
    }

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        _gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initalJumpVelocity = (2 * maxJumpHeight) / timeToApex;
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

        ObjectState = EObjectState.Idle;
    }
    private void Update()
    {
        if (!isRun && !isPunch)
            ObjectState = EObjectState.Idle;

        HandleRotation();
        _currentState.UpdateStates();
        _cc.Move(_appliedMovement * Time.deltaTime * 2);
    }

    //private void Update()
    //{
    //    if (!isRun && !isPunch)
    //        ObjectState = EObjectState.Idle;

    //    HandleRotation();
    //    _currentState.UpdateState();
    //    UpdateAnimation();

    //    if (isSprint)
    //    {
    //        _appliedMovement.x = currentRunMovement.x;
    //        _appliedMovement.z = currentRunMovement.z;
    //    }
    //    else
    //    {
    //        _appliedMovement.x = _currentMovement.x;
    //        _appliedMovement.z = _currentMovement.z;
    //    }

    //    _cc.Move(_appliedMovement * Time.deltaTime * 2);

    //    HandleGravity();
    //    HandleJump();
    //}

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _currentMovement.z;

        //Quaternion currentRotation = transform.rotation;

        if (isRun)
        {
            //Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        bool isFalling = _currentMovement.y <= 0.0f || !_isJumpPressed;
        float fallMultiplier = 2.0f;
        if (IsGrounded())
        {
            _currentMovement.y = _groundedGravity;
            _appliedMovement.y = _groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y =  _currentMovement.y + (_gravity * fallMultiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((previousYVelocity + _currentMovement.y) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_gravity *  Time.deltaTime);
            _appliedMovement.y = (previousYVelocity + _currentMovement.y) * 0.5f;
        }
    }

    private void HandleJump()
    {
        if (!_isJumping && IsGrounded() && _isJumpPressed)
        {
            _isJumping = true;
            _currentMovement.y = initalJumpVelocity * 0.5f;
            _appliedMovement.y = initalJumpVelocity * 0.5f;
        }
        else if (!_isJumpPressed && _isJumping && IsGrounded())
        {
            _isJumping = false;
        }
    }

    #region Input
    public void Move(InputAction.CallbackContext context)
    {
        ObjectState = EObjectState.Move;

        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
        currentRunMovement.z = _currentMovementInput.y * _runMultiplier;

        isRun = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    public void Punch(InputAction.CallbackContext context)
    {
        isPunch = context.ReadValueAsButton(); ;

        ObjectState = EObjectState.Skill;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        isSprint = context.ReadValueAsButton();
    }

    public bool IsGrounded()
    {
        return _cc.isGrounded;
    } 
    #endregion

    public void UpdateAnimation()
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
        if (_animator == null) return;

        if (currentAnimation != anim)
        {
            currentAnimation = anim;
            _animator.CrossFade(anim, crossfade);
        }
    }
}
