using UnityEngine;

public class ChangeGravityOnceTrigger : MonoBehaviour
{
    public Vector3 NewGravity;
    public PlayerController Player;
    private bool _active = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Player)
        {
            Debug.LogWarning("Player ref not set! Finding...");
            Player = FindAnyObjectByType<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_active)
        {
            _active = false;
            Debug.Log($"{other.gameObject} entered, switching gravity");

            Player.Movement.SetGravity(NewGravity, true);
        }
    }
}
