using UnityEngine;

public class MovementState : MonoBehaviour
{
    protected PlayerController _playerController;
    protected Movement _movement;
    protected Rigidbody _rigidbody;
    public bool IsComplete { get; protected set; }

    protected float _enterTime;
    public float CurrentTime => Time.time - _enterTime;

    public virtual void Enter() { }

    public virtual void StateUpdate() { }

    public virtual void StateFixedUpdate() { }

    public virtual void Exit() { }

    public virtual void Setup(PlayerController playerController) {
        _playerController = playerController;
        _movement = _playerController.Movement;
        _rigidbody = playerController.Rigidbody;
    }

    public virtual void Initialise()
    {
        _enterTime = Time.time;
        IsComplete = false;
    }
}
