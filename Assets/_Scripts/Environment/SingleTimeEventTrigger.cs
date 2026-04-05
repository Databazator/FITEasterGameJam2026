using UnityEngine;
using UnityEngine.Events;

public class SingleTimeEventTrigger : MonoBehaviour
{
    bool triggered = false;
    public UnityEvent Event;

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            triggered = true;
            Event.Invoke();
        }
    }
}
