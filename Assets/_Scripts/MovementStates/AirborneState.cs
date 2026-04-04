using UnityEngine;

public class AirborneState : MovementState
{
    private Vector3 _initialVerticalVelocity;
    private Vector3 _horizontalVelocity;

    public override void Enter()
    {
        _initialVerticalVelocity = _movement.JumpVelocity;
        _movement.SetJumpVelocity(Vector3.zero);

        //_horizontalVelocity = _movement.HorizontalVelocity;
    }

    public override void Exit()
    {
        
    }

    public override void StateFixedUpdate()
    {     
        // ---------------------------------------
        // Input movement (local ? world)
        Vector3 movementVector = new Vector3(_movement.MoveInput.x, 0f, _movement.MoveInput.y);
        Vector3 movement = _movement.CamTransform.TransformDirection(movementVector);

        float speed = _movement.CurrentInput.sprint ? _movement.SprintSpeed : _movement.Speed;
        Vector3 targetHorizontalVelocity = movement * speed * _movement.AirControlMultiplier;

        // ---------------------------------------
        // Get current velocity
        Vector3 velocity = _rigidbody.linearVelocity;

        // ---------------------------------------
        // Separate vertical component
        Vector3 gravityDir = _movement.CurrentGravity.normalized;

        Vector3 verticalVel = Vector3.Project(velocity, gravityDir) + _initialVerticalVelocity;
        Vector3 horizontalVel = velocity - verticalVel;

        // ---------------------------------------
        // Apply air control (horizontal)
        horizontalVel = Vector3.ProjectOnPlane(targetHorizontalVelocity, gravityDir);

        // ---------------------------------------
        // Dynamic jump reduction
        if (_movement.CurrentInput.jumpReleased && _movement.VerticalVelocityUp)
        {
            verticalVel *= _movement.DynamicJumpReduction;
        }

        // ---------------------------------------
        // Gravity
        float fallMultiplier = _movement.VerticalVelocityDown ? _movement.FallingMultiplier : 1f;

        verticalVel += _movement.CurrentGravity * fallMultiplier * _movement.GravityMultiplier * Time.fixedDeltaTime;

        _movement.VerticalVelocity = verticalVel;
        _movement.HorizontalVelocity = horizontalVel;

        // ---------------------------------------
        // Final velocity
        _rigidbody.linearVelocity = horizontalVel + verticalVel;
    }
}
