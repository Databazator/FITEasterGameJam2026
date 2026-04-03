using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Movement : MonoBehaviour
{
    CharacterController _controller;
    public PlayerController Player;

    [SerializeField] private float _speed = 8f;
    public float Speed => _speed;
    [SerializeField] private float _sprintSpeed = 12f;
    public float SprintSpeed => _sprintSpeed;

    [SerializeField] private float _jumpForce = 2f;
    public float JumpForce => _jumpForce;

    [SerializeField] private float _launchForce = 15f;
    public float LaunchForce => _launchForce;

    [SerializeField] private float _fallingMultiplier = 1.5f;
    public float FallingMultiplier => _fallingMultiplier;
    [SerializeField] private float _gravityMultiplier = 1.5f;
    public float GravityMultiplier => _gravityMultiplier;
    [SerializeField] private float _dynamicJumpReduction = 0.75f;
    public float DynamicJumpReduction => _dynamicJumpReduction;
    [SerializeField] private float _groundedRaycastExtraDistance = 0.1f;
    public float GroundedRaycastExtraDistance => _groundedRaycastExtraDistance;
    [SerializeField] private float _airControlMultiplier = 1f;
    public float AirControlMultiplier => _airControlMultiplier;

    [SerializeField] private LayerMask _groundLayerMask;
    public LayerMask GroundLayerMask => _groundLayerMask;


    public bool IsInGravityAttractionZone = false;

    private Vector3 _targetUp;
    public Vector3 UpTarget => _targetUp;
    [SerializeField] private float _bodyRotationSpeed = 3f;

    private Vector3 _currentGravity;
    public Vector3 CurrentGravity => _currentGravity;
    private PlayerInput _currentInput;
    private Vector3 _jumpVelocity;
    public Vector3 JumpVelocity => _jumpVelocity;
    public Vector3 SetJumpVelocity(Vector3 velo) => _jumpVelocity = velo;
    public PlayerInput CurrentInput => _currentInput;

    [Header("MovementStates")]
    private MovementStateMachine _stateMachine = new MovementStateMachine();
    public MovementStateMachine StateMachine => _stateMachine;
    public GroundState Ground;
    public IdleState Idle;
    public AirborneState Airborne;

    [Header("Debug")]
    public bool IsGrounded;
    public Vector3 VerticalVelocity;
    public bool VerticalVelocityUp;
    public bool VerticalVelocityDown;
    public Vector3 CurrentGravityDebug;
    public Vector3 HorizontalVelocity;
    public Vector2 MoveInput;
    public Vector3 Velocity;
    public MovementState CurrentState;
    public bool IsAttracted;
    public GravityAttractor CurrentAttractor;

    void Awake()
    {
        if (Player == null)
            Player = GetComponent<PlayerController>();

        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        SetGravity(Physics.gravity);
        _targetUp = _currentGravity * -1f;

        _stateMachine.SetupStates(Player);
        _stateMachine.SetState(Idle);
    }

    public void SetGravity(Vector3 currentGravity)
    {
        _currentGravity = currentGravity;
        CurrentGravityDebug = _currentGravity;
    }

    public void UpdateMovement(PlayerInput input)
    {
        _currentInput = input;
        MoveInput = input.move.normalized;

        IsGrounded = CharacterIsGrounded() && JumpVelocity == Vector3.zero;

        VerticalVelocityUp = VerticalVelocityIsUp();
        VerticalVelocityDown = VerticalVelocityIsDown();


        // handle Jump
        if (_currentInput.jumpHeld && IsGrounded)
            _jumpVelocity = transform.up * _jumpForce;

        SelectState();
        StateMachine.CurrentState?.StateUpdate();

        IsAttracted = Player.GravAttractee.IsAttracted;
        CurrentAttractor = Player.GravAttractee.CurrentAttractor;

        //update target up based on attractor
        if(IsInGravityAttractionZone && Player.GravAttractee.IsAttracted)
        { 
            if(!IsGrounded)
            {
                SetGravity(Player.GravAttractee.CurrentGravity);
                _targetUp = _currentGravity * -1f;
            }
            else
            {
                SetGravity(Player.GravAttractee.CurrentGravity);
                _targetUp = _currentGravity * -1f;
            }
        }

        // update body rotation
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, _targetUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _bodyRotationSpeed);
    }

    void SelectState()
    {
        if (IsGrounded && MoveInput == Vector2.zero && VerticalVelocityDown)
        {
            CurrentState = Idle;
            StateMachine.SetState(Idle);
            return;
        }
        if (IsGrounded && VerticalVelocityDown)
        {
            CurrentState = Ground;
            StateMachine.SetState(Ground);
            return;
        }
        else
        {
            CurrentState = Airborne;
            StateMachine.SetState(Airborne);
            return;
        }
    }

    bool CharacterIsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _currentGravity, out hit, _controller.height / 2f + _groundedRaycastExtraDistance, _groundLayerMask))
        {
            return true;
        }
        return false;
    }

    bool VerticalVelocityIsDown()
    {
        if (_currentGravity != Vector3.zero)
        {
            float gravVeloDot = Vector3.Dot(_currentGravity, VerticalVelocity);
            if (gravVeloDot > 0)
                return true;
            return false;
        }
        return false;
    }

    bool VerticalVelocityIsUp()
    {
        if (_currentGravity != Vector3.zero)
        {
            float gravVeloDot = Vector3.Dot(_currentGravity * -1f, VerticalVelocity);
            if (gravVeloDot > 0)
                return true;
            return false;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + _currentGravity);

        if (_controller)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _currentGravity.normalized * (_controller.height / 2f + _groundedRaycastExtraDistance));
        }
    }


}
