using UnityEngine;

public class NightmareHitboxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.LogWarning("Player triggered nightmare hitbox");
        }
    }
}
