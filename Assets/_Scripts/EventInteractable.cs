using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class EventInteractable : MonoBehaviour
{
    public UnityEvent Event;
    public float InteractionCooldown = 0.5f;
    private bool _ready = true;
    public bool OneTimeUse = false;
    private bool _used = false;

    public bool CanInteract()
    {
        if (OneTimeUse && _used) return false;

        return _ready;
    }

    public void Interact()
    {
        if (OneTimeUse && _used) return;
        _used = true;

        _ready = false;
        DOVirtual.DelayedCall(InteractionCooldown, () => _ready = true);

        Event.Invoke();
    }
}
