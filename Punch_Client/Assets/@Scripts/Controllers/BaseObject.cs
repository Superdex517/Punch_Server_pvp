using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public int ObjectId { get; set; }
    public virtual EGameObjectType ObjectType { get { return EGameObjectType.None; } }
    public Animator Anim { get; set; }
    public CharacterController controller;

    public float MoveSpeed = 5.0f;

    bool _lockLeft = true;

    PositionInfo _positionInfo = new PositionInfo();
    public PositionInfo PosInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;

            Position = new Vector3(value.PosX, value.PosY, value.PosZ);

            bool isMyHero = this is MyHero;
            if (isMyHero == false)
                ObjectState = value.State;

            Dir = value.MoveDir;
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

    [SerializeField]
    protected EMoveDir _dir = EMoveDir.None;
    public EMoveDir Dir
    {
        get { return PosInfo.MoveDir; }
        set
        {
            if (_dir == value)
                return;

            _dir = value;
            PosInfo.MoveDir = value;
        }
    }


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    #region AI (FSM)
    protected virtual void UpdateAI()
    {
        switch (ObjectState)
        {
            case EObjectState.Idle:
                UpdateIdle();
                break;
            case EObjectState.Move:
                UpdateMove();
                break;
            case EObjectState.Skill:
                UpdateSkill();
                break;
            case EObjectState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDead() { }
    #endregion

    #region Animation
    protected virtual void UpdateAnimation()
    {
        switch (ObjectState)
        {
            case EObjectState.Idle:
                //PlayAnimation(0, AnimName.IDLE, true);
                break;
            case EObjectState.Skill:
                break;
            case EObjectState.Move:
                //PlayAnimation(0, AnimName.MOVE, true);
                break;
            case EObjectState.Dead:
                //PlayAnimation(0, AnimName.DEAD, false);
                break;
        }
    }
    #endregion

    public bool LerpPosCompleted { get; protected set; }
    [SerializeField] Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        protected set
        {
            _position = value;
            LerpPosCompleted = false;
        }
    }

    public void SetPosition(Vector3 pos, bool forceMove = false)
    {
        Position = pos;
        LerpPosCompleted = false;

        if (forceMove)
        {
            transform.position = Position;
            LerpPosCompleted = true;
        }
    }

    public void UpdateLerpToPos(float moveSpeed, bool canFlip = true)
    {
        //if (LerpPosCompleted)
        //    return;

        Vector3 destPos = transform.position;
        Vector3 dir = destPos - transform.position;

        float moveDist = moveSpeed * Time.deltaTime;

        SyncPosition();

        controller.Move(transform.position);
    }

    public void SyncPosition()
    {
        Managers.Move.MoveTo(this, Position, forceMove: true);
    }
}
