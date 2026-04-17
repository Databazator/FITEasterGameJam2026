
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXPlayer : MonoBehaviour
{
    public static AudioSFXPlayer Instance;

    public AudioClip SwitchClip;
    public AudioClip OpenDoorClip;
    public List<AudioClip> DoorHandleClips;
    public AudioClip GravityChange;
    AudioManager audioManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }
    public void PlaySwitchSFX()
    {
        audioManager.PlaySFX(SwitchClip, 0.7f);
    }

    public void PlayDoorOpenClip()
    {
        audioManager.PlaySFX(OpenDoorClip, 0.8f);
    }

    public void PlayDoorOpenClip(Vector3 pos)
    {
        audioManager.PlaySFX(OpenDoorClip, pos, 0.8f);
    }

    public void PlayDoorHandleClip()
    {
        AudioClip rndClip = DoorHandleClips[Random.Range(0, DoorHandleClips.Count)];
        audioManager.PlaySFX(rndClip, 0.8f);
    }

    public void PlayGravityChange(float volume = 1f)
    {
        audioManager.PlaySFX(GravityChange, volume);
    }
}
