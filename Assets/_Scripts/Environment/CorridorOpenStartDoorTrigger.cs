using DG.Tweening;
using UnityEngine;

public class CorridorOpenStartDoorTrigger : MonoBehaviour
{
    public Doorway StartDoor;
    public GameObject DoorGlowGO;
    public float GlowFadeDuration;
    public float DoorOpenDuration = 2f;
    private Material GlowMaterial;

    private void Start()
    {
        GlowMaterial = DoorGlowGO.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collided with {other.gameObject}");

        StartDoor.OpenDoor(DoorOpenDuration);
        GlowMaterial.DOColor(new Color(0f, 0f, 0f, 0f), GlowFadeDuration).SetEase(Ease.InQuad);

    }
}


