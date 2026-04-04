using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class StartGameFloorCollapseTrigger : MonoBehaviour
{
    public PlayerController Player;
    public GameObject FloorGO;
    public GameObject Bedroom;
    public Doorway ExitDoor;
    public Light MainLight;
    //public GameObject MainLightGO;

    private Material FloorMaterial;
    private Collider FloorCollider;

    public float DissolveDuration = 3f;
    public float FloorDisableColliderDuration = 2.3f;
    public float DissolveDelay = 1f;
    public float DisableRoomOffset = 1f;
    public float OpenDoorDelay = 3f;
    public float OpenDoorDuration = 2f;

    private void Awake()
    {
        FloorCollider = FloorGO.GetComponent<Collider>();
        FloorMaterial = FloorGO.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other} triggered meee");
        DissolveFloor();
    }

    void DissolveFloor()
    {
        if(FloorMaterial.HasFloat("_Dissolve"))
        {
            //dissolve
            DOTween.To(() => FloorMaterial.GetFloat("_Dissolve"), (float val) => FloorMaterial.SetFloat("_Dissolve", val), 0.85f, DissolveDelay + DissolveDuration).SetEase(Ease.InQuad);
        }
        DOVirtual.DelayedCall(DissolveDelay + FloorDisableColliderDuration, () =>
        {
            FloorCollider.enabled = false;
            Player.Movement.IsInGravityAttractionZone = true;
            DOVirtual.DelayedCall(DisableRoomOffset, () => Bedroom.SetActive(false));
        });

        DOVirtual.DelayedCall(DissolveDelay + FloorDisableColliderDuration + OpenDoorDelay, () =>
        {
            ExitDoor.OpenDoor(OpenDoorDuration);

            //MainLightGO.SetActive(true);
            MainLight.gameObject.SetActive(true);
            MainLight.intensity = 0f;
            MainLight.DOIntensity(2f, OpenDoorDuration).SetEase(Ease.InOutQuad);
        });

    }
}
