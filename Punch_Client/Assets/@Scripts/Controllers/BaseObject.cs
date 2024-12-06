using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public int ObjectId { get; set; }
    public virtual EGameObjectType ObjectType { get; protected set; } = EGameObjectType.None;
    public Animator Anim;
    private string currentAnimation = "";
    public CharacterController cc;

    public float MoveSpeed = 5.0f;

    PositionInfo _positionInfo = new PositionInfo();
    public PositionInfo PosInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;

            Position = new Vector3(value.PosX, value.PosY, value.PosZ);
            Direction = value.Dir;

            bool isMyHero = this is MyPlayer;
            if (isMyHero == false)
                ObjectState = value.State;

            //Dir = value.MoveDir;
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

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
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
        if (Anim == null) return;

        if (currentAnimation != anim)
        {
            currentAnimation = anim;
            Anim.CrossFade(anim, crossfade);
        }
    }
    #endregion

    public bool MoveCompleted { get; protected set; }

    [SerializeField] Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        protected set
        {
            _position = value;
            //MoveCompleted = false;
        }
    }

    [SerializeField] float _direction;
    public float Direction
    {
        get { return _direction; }
        protected set
        {
            _direction = value;
        }
    }

    public void SetPosition(Vector3 pos, bool forceMove = false)
    {
        Position = pos;
        //MoveCompleted = false;

        if (forceMove)
        {
            transform.position = Position;
            //MoveCompleted = true;
        }
    }

    public void UpdateLerpToPos(float moveSpeed, bool canFlip = true)
    {
        //if (MoveCompleted)
        //    return;

        //SyncPosition();

        transform.position = Position;
        transform.rotation = Quaternion.Euler(0f, Direction, 0f);
    }

    public void SyncPosition()
    {
        Managers.Move.MoveTo(this, Position, forceMove: true);
    }
}
