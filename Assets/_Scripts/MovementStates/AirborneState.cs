using UnityEngine;

public class AirborneState : MovementState
{
    private Vector3 _verticalVelocity;
    private Vector3 _horizontalVelocity;

    public override void Enter()
    {
        _verticalVelocity = Vector3.zero + _movement.JumpVelocity;
        _movement.SetJumpVelocity(Vector3.zero);

        _horizontalVelocity = _movement.HorizontalVelocity;
    }

    public override void Exit()
    {
        
    }

    public override void StateUpdate()
    {
        //--------------------------------------------------------------------------------------------
        // Air control horizontal movement
        Vector3 movementVector = new Vector3(_movement.MoveInput.x, 0f, _movement.MoveInput.y);

        Vector3 movement = transform.TransformDirection(movementVector);

        float speed = _movement.CurrentInput.sprint ? _movement.SprintSpeed : _movement.Speed;
        _horizontalVelocity = movement * speed * _movement.AirControlMultiplier;
        _movement.HorizontalVelocity = _horizontalVelocity;           

        // ---------------------------------------------------------------
        // dyn jump reduction
        if (_movement.CurrentInput.jumpReleased &&_movement.VerticalVelocityUp)
        {
            _verticalVelocity *= _movement.DynamicJumpReduction;
        }
        // -------------------------------------------------------------------
        // calc falling gravity
        float fallMultiplier = _movement.VerticalVelocityDown ? _movement.FallingMultiplier : 1f;
        _verticalVelocity += _movement.CurrentGravity * fallMultiplier * _movement.GravityMultiplier * Time.deltaTime;

        //  -------------------------------------------------------------------
        // apply move vectors
        _movement.VerticalVelocity = _verticalVelocity;

        _movement.Velocity = _horizontalVelocity + _verticalVelocity;

        _controller.Move((_horizontalVelocity + _verticalVelocity) * Time.deltaTime);
    }
}
