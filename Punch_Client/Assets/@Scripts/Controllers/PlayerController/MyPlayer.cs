using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Google.Protobuf.Protocol;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static UI_GameScene;

public class MyPlayer : Player
{
    public EGameSceneType SceneType { get; set; } = EGameSceneType.Lobby;

    protected bool _sendMovePacket = false;

    [SerializeField]
    private Vector3 _destPos;
    public Vector3 DestPos
    {
        get { return _destPos; }
        set
        {
            if (_destPos == value) return;

            _destPos = value;
            _sendMovePacket = true;
        }
    }

    private float _playerDir;
    public float PlayerDir
    {
        get { return _playerDir; }
        set
        {
            if(_playerDir == value) return;

            _playerDir = value;
            _sendMovePacket = true;
        }
    }

    #region Character Prop
    public PunchInputAction _punchInput;
    private Animator _animator;
    private string currentAnimation = "";

    public Transform cam;
    public CinemachineFreeLook freeLookCam;

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
    private bool _requireNewJumpPress = false;
    private bool _requireNewPunchPress = false;
    private float _initalJumpVelocity;
    private float maxJumpHeight = 3.0f;
    private float maxJumpTime = 0.75f;

    private bool _isPunchPress = false;

    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private bool isGameOver = false;

    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    private Coroutine _currentPunchResetRoutine;
    public Coroutine CurrentPunchResetRoutine { get { return _currentPunchResetRoutine; } set { _currentPunchResetRoutine = value; } }

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public CharacterController CC { get { return _cc; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsPunchPressed { get { return _isPunchPress; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
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
    #endregion
    public bool SendMovePacket { set { _sendMovePacket = value; } }

    protected override void Awake()
    {
        base.Awake();
        //new input system Àû¿ë
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
        _punchInput.Player.Punch.canceled += Punch;

        SetJumpVariables();
    }

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        _gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initalJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    protected override void Start()
    {
        base.Start();

        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;

    }

    protected override void Update()
    {
        //base.Update();
        transform.position = Position;


        UpdateAI();

        _currentState.UpdateStates();

        if (!Managers.Room.IsResult)
        {
            HandleRotation();

            HandleMovement();
        }

        UpdateSendMovePacket();

        SendResult();
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
            PlayerDir = transform.localEulerAngles.y;
        }
    }

    void HandleMovement()
    {
        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _cc.Move(_cameraRelativeMovement * Time.deltaTime * 2);
        DestPos = transform.position;
    }

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


    #region UpdateState
    protected override void UpdateIdle()
    {
        base.UpdateIdle();
    }

    protected override void UpdateMove()
    {
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();
    }

    protected override void UpdateDead()
    {
        base.UpdateDead();

    }
    #endregion

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

    void UpdateSendMovePacket()
    {
        if (_sendMovePacket)
        {
            C_Move movePacket = new C_Move() { PosInfo = new PositionInfo() };
            movePacket.PosInfo.MergeFrom(PosInfo);
            movePacket.PosInfo.PosX = DestPos.x;
            movePacket.PosInfo.PosY = DestPos.y;
            movePacket.PosInfo.PosZ = DestPos.z;
            movePacket.PosInfo.Dir = PlayerDir;
            Managers.Network.GameServer.Send(movePacket);
            _sendMovePacket = false;
            Debug.Log("SendPacket");
        }
    }

    void SendResult()
    {
        if (isDead)
        {
            C_GameResult gameResult = new C_GameResult();
            Managers.Network.Send(gameResult);
            FindObjectOfType<UI_GameScene>().Result = GameResult.Lose;

            isDead = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyWeapon")
        {
            if (!isGameOver)
            {
                _sendMovePacket = true;
                isDead = true;
                ObjectState = EObjectState.Dead;
                isGameOver = Managers.Room.IsResult;
            }
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
}



[Serializable]
public struct Movement
{

}