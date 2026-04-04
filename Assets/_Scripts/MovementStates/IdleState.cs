using UnityEngine;

public class IdleState : MovementState
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void StateFixedUpdate()
    {
        _rigidbody.linearVelocity = _movement.CurrentGravity.normalized;
        _movement.VerticalVelocity = _movement.CurrentGravity.normalized;
    }
}
