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
    public bool SendMovePacket = false;

    public PunchInputAction _punchInput;
    private CharacterController _cc;
    private Animator _animator;
    private string currentAnimation = "";

    public Transform cam;
    public CinemachineFreeLook freeLookCam;
    public EObjectState ObjectState = EObjectState.None;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _appliedMovement;
    private Vector3 _cameraRelativeMovement;
    private bool _isMovementPressed;
    private bool _isRunPressed;

    private float _runMultiplier = 3.0f;
    private float _rotationFactorPerFrame = 15.0f;
    private float _gravity = -9.8f;
    private float _groundedGravity = -0.05f;

    private bool _isJumpPressed = false;
    private bool _isJumping = false;
    private bool _requireNewJumpPress = false;
    private float _initalJumpVelocity;
    private float maxJumpHeight = 3.0f;
    private float maxJumpTime = 0.75f;

    [SerializeField]
    private bool _isPunchPress = false;
    private bool _requireNewPunchPress = false;

    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public CharacterController CC { get { return _cc; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public bool IsJumping { set { _isJumping = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public bool IsPunchPressed { get { return _isPunchPress; } }
    public bool RequireNewPunchPress { get { return _requireNewPunchPress; } set { _requireNewPunchPress = value; } }

    public float InitalJumpVelocity { get { return _initalJumpVelocity; } set { _initalJumpVelocity = value; } }
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
        _punchInput = new PunchInputAction();
        _cc = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _punchInput.Player.Move.started += Move;
        _punchInput.Player.Move.performed += Move;
        _punchInput.Player.Move.canceled += Move;
            
        _punchInput.Player.Run.started += Run;
        _punchInput.Player.Run.canceled += Run;

        _punchInput.Player.Jump.started += Jump;
        _punchInput.Player.Jump.canceled += Jump;

        _punchInput.Player.Punch.started += Punch;
        _punchInput.Player.Punch.performed += Punch;
        _punchInput.Player.Punch.canceled += Punch;

        SetJumpVariables();
    }

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        _gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initalJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void Start()
    {
        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;

        _cc.Move(_appliedMovement * Time.deltaTime * 2);
    }

    private void Update()
    {
        UpdateAnimation();

        HandleRotation();
        _currentState.UpdateStates();

        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _cc.Move(_cameraRelativeMovement * Time.deltaTime * 2);
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    #region Input
    public void Move(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();

        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }
    public void Run(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    public void Punch(InputAction.CallbackContext context)
    {
        _isPunchPress = context.ReadValueAsButton();
        _requireNewPunchPress = false;
    }

    public bool IsGrounded()
    {
        return _cc.isGrounded;
    } 
    #endregion

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

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
            case EObjectState.Victory:
                ChangeAnimation("Victory");
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

    private void OnEnable()
    {
        _punchInput.Player.Enable();
    }

    private void OnDisable()
    {
        _punchInput.Player.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyWapeon")
        {
            Debug.Log("damaged");
        }
    }
}
