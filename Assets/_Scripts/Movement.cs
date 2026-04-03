using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    CharacterController _controller;
    public PlayerController PlayerController;

    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _sprintSpeed = 12f;
    [SerializeField] private float _jumpForce = 2f;
    [SerializeField] private float _fallingMultiplier = 1.5f;
    [SerializeField] private float _gravityMultiplier = 1.5f;
    [SerializeField] private float _dynamicJumpReduction = 0.75f;
    [SerializeField] private float _groundedRaycastExtraDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayerMask;
    
    private Vector3 _verticalVelocity;
    private Vector3 _currentGravity;
    private PlayerInput _currentInput;

    [Header("Debug")]
    public bool IsGrounded;
    public Vector3 CurrentGravity;
    public Vector3 VerticalVelocity;
    public bool VerticalVelocityUp;
    public bool VerticalVelocityDown;
    public Vector3 HorizontalVelocity;
    public Vector2 MoveInput;
    public Vector3 Velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentGravity = Physics.gravity;
        _controller = GetComponent<CharacterController>();
    }

    public void SetGravity(Vector3 currentGravity)
    {

        _currentGravity = currentGravity;
    }

    public void UpdateInput(PlayerInput input)
    {
        _currentInput = input;
        MoveInput = input.move.normalized;
    }

    private void Update()
    {
        IsGrounded = CharacterIsGrounded();
        Vector3 movement = _controller.transform.forward * MoveInput.y +
        _controller.transform.right * MoveInput.x;

        VerticalVelocityUp = VerticalVelocityIsUp();
        VerticalVelocityDown = VerticalVelocityIsDown();


        if (IsGrounded && VerticalVelocityDown)
            _verticalVelocity = _currentGravity * 0.25f;

        if (_currentInput.jumpHeld && IsGrounded)
            _verticalVelocity = transform.up * _jumpForce;

        if (_currentInput.jumpReleased && VerticalVelocityUp)
            _verticalVelocity *= _dynamicJumpReduction;

        float fallMultiplier = (VerticalVelocityDown && IsGrounded) ? _fallingMultiplier : 1f;
        _verticalVelocity += _currentGravity * fallMultiplier * _gravityMultiplier * Time.deltaTime;

        float speed = _currentInput.sprint ? _sprintSpeed : _speed;
        HorizontalVelocity = movement * speed;
        VerticalVelocity = _verticalVelocity;

        Velocity = HorizontalVelocity + VerticalVelocity;        

        _controller.Move(Velocity * Time.deltaTime);
    }

    bool CharacterIsGrounded()
    {
        return Physics.Raycast(transform.position, _currentGravity, _controller.height / 2f + _groundedRaycastExtraDistance, _groundLayerMask);
    }

    bool VerticalVelocityIsDown()
    {
        if(_currentGravity != Vector3.zero)
        {
            float gravVeloDot = Vector3.Dot(_currentGravity, _verticalVelocity);
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
            float gravVeloDot = Vector3.Dot(_currentGravity * -1f, _verticalVelocity);
            if (gravVeloDot > 0)
                return true;
            return false;
        }
        return false;
    }




}
