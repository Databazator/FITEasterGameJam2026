using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody _rigidbody;
    public PlayerController Player;
    public Transform CamTransform;

    [SerializeField] private float _speed = 8f;
    public float Speed => _speed;
    [SerializeField] private float _sprintSpeed = 12f;
    public float SprintSpeed => _sprintSpeed;

    private bool _canJump = true;
    [SerializeField] private float _jumpForce = 0.6f;
    public float JumpForce => _jumpForce;

    private bool _canLaunch = true;
    public bool CanLaunch => _canLaunch;

    private bool _launched = false;
    public bool Launched => _launched;
    public void SetLaunched(bool val) => _launched = val;
    public void SetCanLaunch(bool val) => _canLaunch = val;

    [SerializeField] private float _launchForce = 15f;
    public float LaunchForce => _launchForce;

    [SerializeField] private float _launchMaxDistance = 50f;
    [SerializeField] private LayerMask _launchZoneLayerMask;

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
    public GravityAttractor CurrentlySuppressed;
    public bool ValidLaunchTarget;
    public bool CanLaunchDebug;
    public bool LaunchedDebug;
    public Vector3 LaunchTargetAttractorPos;

    void Awake()
    {
        if (Player == null)
            Player = GetComponent<PlayerController>();

        _rigidbody = GetComponent<Rigidbody>();
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

        bool wasGroundedLastFrame = IsGrounded;

        IsGrounded = CharacterIsGrounded() && VerticalVelocityDown;

        if(!wasGroundedLastFrame && IsGrounded)
        {
            OnGrounded();
        }

        VerticalVelocityUp = VerticalVelocityIsUp();
        VerticalVelocityDown = VerticalVelocityIsDown();
        
        StateMachine.CurrentState?.StateUpdate();

        IsAttracted = Player.GravAttractee.IsAttracted;
        CurrentAttractor = Player.GravAttractee.CurrentAttractor;
        CurrentlySuppressed = Player.GravAttractee.SuppressedAttactor;

        //update target up based on attractor
        if (IsInGravityAttractionZone && Player.GravAttractee.IsAttracted)
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
        else if(IsInGravityAttractionZone && !Player.GravAttractee.IsAttracted)
        {
            SetGravity(Vector3.zero);
            //_targetUp = transform.up;
        }

        // update body rotation
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, _targetUp) * _rigidbody.rotation;
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.deltaTime * _bodyRotationSpeed);
    }

    void OnGrounded()
    {
        Debug.Log("OnGrounded");
        _canJump = true;
        _launched = false;
        //_canLaunch
    }

    private void FixedUpdate()
    {
        // first check for launch
        bool validLaunchTarget = CanLaunchAtAttractor();
        // dbug
        ValidLaunchTarget = validLaunchTarget;
        CanLaunchDebug = CanLaunch;
        LaunchedDebug = Launched;
        //Debug.Log("Launch Target Visible: " + validLaunchTarget);
        if(_currentInput.jumpHeld && IsGrounded && _canLaunch && validLaunchTarget)
        {
            _canLaunch = false;
            _rigidbody.linearVelocity = CamTransform.forward * _launchForce;
            Player.GravAttractee.SetSuppressedAttractor(Player.GravAttractee.CurrentAttractor);
            _launched = true;
            Debug.Log("Launched");
        }
        // handle Jump
        else if (_currentInput.jumpHeld && IsGrounded && _canJump && !_launched)
        {
            _canJump = false;
            _rigidbody.linearVelocity += CurrentGravity * -1f * _jumpForce;
            VerticalVelocity = CurrentGravity * -1f * _jumpForce;
            VerticalVelocityDown = VerticalVelocityIsDown();
            Debug.Log("Jumped " + VerticalVelocityDown);
        }

        if (_launched)
        {
            return;
        }
        SelectState();
        //Debug.Log("Current State: " + StateMachine.CurrentState);
        StateMachine.CurrentState?.StateFixedUpdate();
    }

    void SelectState()
    {
        if (IsGrounded && MoveInput == Vector2.zero && VerticalVelocityDown && !Launched)
        {
            CurrentState = Idle;
            StateMachine.SetState(Idle);
            return;
        }
        if (IsGrounded && VerticalVelocityDown && !Launched)
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
        if (Physics.Raycast(transform.position, _currentGravity, out hit, Player.PlayerCollider.height / 2f + _groundedRaycastExtraDistance, _groundLayerMask))
        {
            return true;
        }
        return false;
    }

    bool CanLaunchAtAttractor()
    {
        RaycastHit hit;
        if( Physics.Raycast(CamTransform.position, CamTransform.forward, out hit, _launchMaxDistance, _launchZoneLayerMask))
        {
            LaunchTargetAttractorPos = hit.point;
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

        if (_rigidbody)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _currentGravity.normalized * (Player.PlayerCollider.height / 2f + _groundedRaycastExtraDistance));
        }

        if(CamTransform)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(CamTransform.position, CamTransform.position + CamTransform.forward * _launchMaxDistance);
        }

        if(LaunchTargetAttractorPos != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(LaunchTargetAttractorPos, Vector3.one * 0.1f);
        }
    }


}
