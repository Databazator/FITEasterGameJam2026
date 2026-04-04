using UnityEngine;
using UnityEngine.Events;

public class EndlessHallwayTeleport : MonoBehaviour
{
    public Transform FromSegment;
    public Transform ToSegment;
    public PlayerController Player;
    public UnityEvent OnTriggerTeleport;
    public bool triggerEnabled = true;

    public void OnTriggerEnter(Collider other)
    {
        if(triggerEnabled)
            TeleportPlayer();
    }

    public void TeleportPlayer()
    {
        // Convert player position into local space of current segment
        Vector3 localPos = FromSegment.InverseTransformPoint(Player.transform.position);

        // Convert that local position into world space of target segment
        Vector3 newWorldPos = ToSegment.TransformPoint(localPos);

        // Handle rotation
        Quaternion localRot = Quaternion.Inverse(FromSegment.rotation) * Player.transform.rotation;
        Quaternion newWorldRot = ToSegment.rotation * localRot;

        // Apply
        Player.TeleportTo(newWorldPos);
        Player.Rigidbody.rotation = newWorldRot;

        OnTriggerTeleport.Invoke();
    }
}
