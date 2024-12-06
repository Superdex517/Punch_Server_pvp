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
        }
    }

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

        //newOrientation = transform.Find("orientation");
        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;
    }

    protected override void Update()
    {
        PlayerRotation();
        PlayerGravity();
        PlayerMovement();

        UpdateAI();

        //UpdateLerpToPos(MoveSpeed);

        UpdateSendMovePacket();
    }

    #region movement
    private void PlayerGravity()
    {
        if(IsGrounded() && velocity < 0.0f)
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

        //rotation값 packet 전달
        PlayerDir = transform.localEulerAngles.y;
    }

    private void PlayerMovement()
    {
        if (direction.magnitude >= 0.1f)
        {
            Debug.Log("Move");

            ObjectState = EObjectState.Move;

            cc.Move(direction * MoveSpeed * Time.deltaTime);

            DestPos = transform.position;
        }
        else
        {
            Debug.Log("Idle");
            ObjectState = EObjectState.Idle;
        }
    }
    #endregion

    #region UpdateState
    protected override void UpdateIdle()
    {
        //base.UpdateIdle();

        //if (MoveCompleted == false)
        //{
        //    ObjectState = EObjectState.Move;
        //    return;
        //}
    }

    protected override void UpdateMove()
    {
        //if (MoveCompleted)
        //{
        //    ObjectState = EObjectState.Idle;
        //    return;
        //}
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
        Debug.Log("punch");
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        //isSprinting = context.started || context.performed;
    }

    private bool IsGrounded() => cc.isGrounded;
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

            Debug.Log("sendPacket");
        }
    }
}



[Serializable]
public struct Movement
{

}