using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StartGameFloorCollapseTrigger : MonoBehaviour
{
    public PlayerController Player;
    public GameObject FloorGO;
    public List<GameObject> DissolveGOs;
    public GameObject Bedroom;
    public Doorway ExitDoor;
    public Light MainLight;
    public NightmareController NightmareController;
    //public GameObject MainLightGO;

    public AudioClip SpaceAmbience;

    private List<Material> DissolveMaterials = new List<Material>();
    private Collider FloorCollider;
    private GUIManager gui;

    public float DissolveDuration = 3f;
    public float FloorDisableColliderDuration = 2.3f;
    public float DissolveDelay = 0.5f;
    public float DisableRoomOffset = 1f;
    public float OpenDoorDelay = 3f;
    public float OpenDoorDuration = 2f;
    public float NightmareAppearDelay = 1.5f;

    private void Awake()
    {
        FloorCollider = FloorGO.GetComponent<Collider>();
        foreach (GameObject go in DissolveGOs)
        {
            DissolveMaterials.Add(go.GetComponent<Renderer>().material);
        }
    }

    private void Start()
    {
        gui = GUIManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other} triggered meee");
        DissolveFloor();
    }

    public void DissolveFloor()
    {
        foreach (Material mat in DissolveMaterials)
        {

            if (mat.HasFloat("_Dissolve"))
            {
                //dissolve
                DOTween.To(() => mat.GetFloat("_Dissolve"), (float val) => mat.SetFloat("_Dissolve", val), 0.85f, DissolveDelay + DissolveDuration).SetEase(Ease.InQuad);
            }
        }
        DOVirtual.DelayedCall(DissolveDelay + FloorDisableColliderDuration, () =>
        {
            FloorCollider.enabled = false;
            Player.Movement.SetToInAttractorZone();
            DOVirtual.DelayedCall(DisableRoomOffset, () => Bedroom.SetActive(false));
            AudioManager.Instance.PlayAmbient(SpaceAmbience, 0.4f);
        });

        DOVirtual.DelayedCall(DissolveDelay + FloorDisableColliderDuration + NightmareAppearDelay, () =>
        {
            NightmareController.StartNightmare();
            Player.Movement.IsFirstLaunchableInteraction = true;
        });

        DOVirtual.DelayedCall(DissolveDelay + FloorDisableColliderDuration + NightmareAppearDelay + NightmareController.NightmareAppearDuration + 5f, () =>
        {
            ExitDoor.OpenDoor(OpenDoorDuration);
            AudioSFXPlayer.Instance.PlayDoorOpenClip();

            //MainLightGO.SetActive(true);
            MainLight.gameObject.SetActive(true);
            MainLight.intensity = 0f;
            MainLight.DOIntensity(2f, OpenDoorDuration).SetEase(Ease.InOutQuad);
            DOVirtual.DelayedCall(2.5f, () => VocalizeLevelExitHint());
        });
    }
    public void VocalizeLevelExitHint()
    {
        gui.WriteText("... the light!", 2f);
    }
}
