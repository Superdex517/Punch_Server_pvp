using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorContorller : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    public Transform tr;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    void Start()
    {

    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);
    }

    //public bool useRootMotion = false;

    //public Animator animator;

    //public float groundMinDistance = 0.25f;
    //public float groundMaxDistance = 0.5f;

    //public float moveSpeed;
    //public float groundDistance;
    //public RaycastHit groundHit;

    //private Rigidbody _rb;
    //private CapsuleCollider _capsuleCollider;

    //private bool _isStrafing;
    //public bool isStrafing
    //{
    //    get
    //    {
    //        return _isStrafing;
    //    }
    //    set
    //    {
    //        _isStrafing = value;
    //    }
    //}
    //private bool isGrounded { get; set; }
    //private bool isSprinting { get; set; }
    //private bool stopMove { get; set; }

    //public Transform rotateTarget;
    //public Vector3 moveDir;

    //public vMovementSpeed freeSpeed, strafeSpeed;

    //[System.Serializable]
    //public class vMovementSpeed
    //{
    //    [Range(1f, 20f)]
    //    public float movementSmooth = 6f;
    //    [Range(0f, 1f)]
    //    public float animationSmooth = 0.2f;
    //    [Tooltip("Rotation speed of the character")]
    //    public float rotationSpeed = 16f;
    //    [Tooltip("Character will limit the movement to walk instead of running")]
    //    public bool walkByDefault = false;
    //    [Tooltip("Rotate with the Camera forward when standing idle")]
    //    public bool rotateWithCamera = false;
    //    [Tooltip("Speed to Walk using rigidbody or extra speed if you're using RootMotion")]
    //    public float walkSpeed = 2f;
    //    [Tooltip("Speed to Run using rigidbody or extra speed if you're using RootMotion")]
    //    public float runningSpeed = 4f;
    //    [Tooltip("Speed to Sprint using rigidbody or extra speed if you're using RootMotion")]
    //    public float sprintSpeed = 6f;
    //}

    //public void Init()
    //{
    //    animator = GetComponent<Animator>();
    //    _rb = GetComponent<Rigidbody>();
    //    _capsuleCollider = GetComponent<CapsuleCollider>();
    //    isGrounded = true;
    //}

    //public void UpdateMotor()
    //{
    //    CheckGround();
    //}

    //public void ControllerMoveSpeed(vMovementSpeed speed)
    //{
    //    if (speed.walkByDefault)
    //        moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.runningSpeed : speed.walkSpeed, speed.movementSmooth * Time.deltaTime);
    //    else
    //        moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.sprintSpeed : speed.runningSpeed, speed.movementSmooth * Time.deltaTime);
    //}

    //public void MoveCharactor(Vector3 _dir)
    //{
    //    if (!isGrounded) return;

    //    _dir.y = 0;
    //    _dir.x = Mathf.Clamp(_dir.x, -1f, 1f);
    //    _dir.z = Mathf.Clamp(_dir.z, -1f, 1f);

    //    if (_dir.magnitude > 1f)
    //        _dir.Normalize();

    //    Vector3 targetPosition = (useRootMotion ? animator.rootPosition : _rb.position) + _dir * (stopMove ? 0 : moveSpeed) * Time.deltaTime;
    //    Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

    //    bool useVerticalVelocity = true;
    //    if (useVerticalVelocity) targetVelocity.y = _rb.velocity.y;
    //    _rb.velocity = targetVelocity;
    //}

    //public void RotateToPosition(Vector3 position)
    //{
    //    Vector3 desiredDirection = position - transform.position;
    //    RotateToDirection(desiredDirection.normalized);
    //}

    //public virtual void RotateToDirection(Vector3 direction)
    //{
    //    RotateToDirection(direction, isStrafing ? strafeSpeed.rotationSpeed : freeSpeed.rotationSpeed);
    //}

    //public virtual void RotateToDirection(Vector3 direction, float rotationSpeed)
    //{
    //    if (!isGrounded) return;
    //    direction.y = 0f;
    //    Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, .1f);
    //    Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
    //    transform.rotation = _newRotation;
    //}

    //private void CheckGround()
    //{

    //}
}
