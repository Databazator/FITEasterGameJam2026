using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GroundState : MovementState
{

    Vector3 _horizontalVelocity;
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void StateFixedUpdate()
    {
        Vector3 movementVector = new Vector3(_movement.MoveInput.x, 0f, _movement.MoveInput.y);
        Vector3 movement = _movement.CamTransform.TransformDirection(movementVector);

        float speed = _movement.CurrentInput.sprint ? _movement.SprintSpeed : _movement.Speed;
        _horizontalVelocity = movement * speed;

        Vector3 gravityDir = _movement.CurrentGravity.normalized;
        _horizontalVelocity = Vector3.ProjectOnPlane(_horizontalVelocity, gravityDir);

        Vector3 velocity = _rigidbody.linearVelocity;

        // Split velocity
        //Vector3 verticalVel = Vector3.Project(velocity, gravityDir);
        //Vector3 horizontalVel = velocity - verticalVel;
               

        Vector3 verticalVelocity = gravityDir;
        _movement.HorizontalVelocity = _horizontalVelocity;
        _movement.VerticalVelocity = verticalVelocity;

        _movement.Velocity = _horizontalVelocity + verticalVelocity;

        _rigidbody.linearVelocity = _horizontalVelocity + verticalVelocity;

    }
}
