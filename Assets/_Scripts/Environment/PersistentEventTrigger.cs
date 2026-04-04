using UnityEngine;
using UnityEngine.Events;

public class PersistentEventTrigger : MonoBehaviour
{
    public UnityEvent Event;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Event.Invoke();
        }
    }
}
