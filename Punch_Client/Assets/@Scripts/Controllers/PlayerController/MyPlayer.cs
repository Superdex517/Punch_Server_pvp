using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Google.Protobuf.Protocol;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MyPlayer : Player
{
    public EGameSceneType SceneType { get; set; } = EGameSceneType.Lobby;

    protected bool _sendMovePacket = false;

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
        }
    }

    public PunchInputAction punchInput;
    public Transform cam;
    public CinemachineFreeLook freeLookCam;

    public float gravityMultiplier = 1.0f;
    public float jumpPower = 4;

    private float lookAngle;
    private float turnSmoothTime = 700;
    private float turnSmoothVelocity;
    private float speed = 5f;
    private float gravity = -9.81f;
    private float velocity;
    private Vector3 direction;
    private Vector2 input;

    private float multiplier = 2;
    private float acceleration = 20;

    private bool isPunch;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public float currentSpeed;

    [SerializeField] private Movement movement;


    #region newMovement
    public float newMoveSpeed = 7;

    public float jumpForce = 10;
    public float jumpCooldown = 0.5f;
    public float airMultiplier = 0.06f;

    public float groundDrag = 10;
    public float playerHeight = 2;
    public LayerMask whatisGround;
    
    public bool isJump;
    public bool grounded;

    public Transform newOrientation;
    public Vector3 newMoveDir;
    Rigidbody rb;
    #endregion

    //new input system 적용
    protected override void Awake()
    {
        base.Awake();
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

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        whatisGround = LayerMask.GetMask("whatisGround");
        isJump = true;

        newOrientation = transform.Find("orientation");
        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;
    }

    protected override void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatisGround);
        
        MovementPackage();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
            ObjectState = EObjectState.Idle;
        }

        
        MovementPackage();

        UpdateAI();

        UpdateAnimation();

        //UpdateLerpToPos(MoveSpeed);

        UpdateSendMovePacket();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        
    }

    #region movement
    void MovementPackage()
    {
        PlayerRotation();
        PlayerGravity();
    }

    private void PlayerGravity()
    {
        //if (IsGrounded() && velocity < 0.0f)
        //{
        //    velocity = -1.0f;
        //}
        //else
        //{
        //    velocity += gravity * gravityMultiplier * Time.deltaTime;
        //}
        //direction.y = velocity;
    }

    public float turns;

    private void PlayerRotation()
    {
        //if (input.sqrMagnitude == 0)
        //    return;

        //direction = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f) * new Vector3(input.x, 0f, input.y);
        //var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSmoothTime * Time.deltaTime);



        Vector3 currentLookDir = cam.forward;
        currentLookDir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(currentLookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turns);
        //rotation값 packet 전달
        PlayerDir = transform.localEulerAngles.y;
    }

    private void PlayerMovement()
    {
        float targetSpeed = isSprinting ? speed * multiplier : speed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        
        newMoveDir = newOrientation.forward * input.y + newOrientation.right * input.x;

        if (grounded)
        {
            rb.AddForce(newMoveDir.normalized * newMoveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            Debug.Log("not grounded");
            rb.AddForce(newMoveDir.normalized * newMoveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //cc.Move(direction * currentSpeed * Time.deltaTime);

        //position값 packet 전달
        DestPos = transform.position;
    }
    #endregion

    #region UpdateState
    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        if (MoveCompleted)
        {
            ObjectState = EObjectState.Move;
            return;
        }
    }

    protected override void UpdateMove()
    {
        if (MoveCompleted == false)
        {
            ObjectState = EObjectState.Idle;
            return;
        }
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
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y).normalized;

        MoveCompleted = false;

        if (input.magnitude != 0)
            MoveCompleted = true;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if(isJump && grounded)
        {
            Debug.Log("jump");
            isJump = false;
            NewJump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //if (!IsGrounded())
        //    return;
        //velocity += jumpPower;
    }
    private void NewJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isJump = true;
    }

    public void Punch(InputAction.CallbackContext context)
    {
        Debug.Log("punch");
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = context.started || context.performed;
    }

    //private bool IsGrounded() => cc.isGrounded;
    #endregion

    void UpdateSendMovePacket()
    {
        C_Move movePacket = new C_Move() { PosInfo = new PositionInfo() };
        movePacket.PosInfo.MergeFrom(PosInfo);
        movePacket.PosInfo.PosX = DestPos.x;
        movePacket.PosInfo.PosY = DestPos.y;
        movePacket.PosInfo.PosZ = DestPos.z;

        movePacket.PosInfo.Dir = PlayerDir;

        Managers.Network.GameServer.Send(movePacket);
        _sendMovePacket = false;

        //Debug.Log($"{DestPos.x}, {DestPos.y}, {DestPos.z}, {PlayerDir}");
    }


}

[Serializable]
public struct Movement
{

}