using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GroundState : MovementState
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void StateUpdate()
    {
        Vector3 movementVector = new Vector3(_movement.MoveInput.x, 0f, _movement.MoveInput.y);

        Vector3 movement = transform.TransformDirection(movementVector);

        float speed = _movement.CurrentInput.sprint ? _movement.SprintSpeed : _movement.Speed;
        _movement.HorizontalVelocity = movement * speed;

        Vector3 verticalVelocity = _movement.CurrentGravity * 0.25f;
        _movement.VerticalVelocity = verticalVelocity;

        _movement.Velocity = _movement.HorizontalVelocity + verticalVelocity;

        _controller.Move(_movement.Velocity * Time.deltaTime);

    }
}
