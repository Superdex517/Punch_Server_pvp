using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Google.Protobuf.Protocol;

public class MyPlayer : Player
{
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

   
    public Transform cam;
    public CinemachineFreeLook freeLookCam;
    
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        freeLookCam.Follow = this.gameObject.transform;
        freeLookCam.LookAt = this.gameObject.transform;
    }

    protected override void Update()
    {
        UpdateInput();

        UpdateAI();

        UpdateSendMovePacket();
    }

    void UpdateInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            DestPos = transform.position;
        }
    }

    protected override void UpdateMove()
    {
        
    }

    void UpdateSendMovePacket()
    {
        C_Move movePacket = new C_Move() { PosInfo = new PositionInfo() };
        movePacket.PosInfo.MergeFrom(PosInfo);
        movePacket.PosInfo.PosX = DestPos.x;
        movePacket.PosInfo.PosY = DestPos.y;
        movePacket.PosInfo.PosZ = DestPos.z;
        Managers.Network.GameServer.Send(movePacket);
        _sendMovePacket = false;

        Debug.Log($"{DestPos.x}, {DestPos.y}, {DestPos.z}");
    }
}
