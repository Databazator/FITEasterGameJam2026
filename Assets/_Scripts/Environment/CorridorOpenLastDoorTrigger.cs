using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CorridorOpenLastDoorTrigger : MonoBehaviour
{
    public Doorway EndDoor;
    public GameObject DoorGlowGO;
    public float GlowFadeDuration;
    public float DoorOpenDuration = 2f;
    private Material GlowMaterial;
    public UnityEvent OnTriggerEvent;

    private void Start()
    {
        GlowMaterial = DoorGlowGO.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collided with {other.gameObject}");
        
        EndDoor.OpenDoor(DoorOpenDuration);
        GlowMaterial.DOColor(new Color(0f, 0f, 0f, 0f), GlowFadeDuration).SetEase(Ease.InQuad);
        OnTriggerEvent.Invoke();
    }
}
