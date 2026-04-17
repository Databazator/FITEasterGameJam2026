using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightmareController : MonoBehaviour
{
    public PlayerController Player;
    public GameObject NightmareMesh;
    public Light NightmareLight;
    private float _nightmareIntensity;
    public float NightmareMeshSize;

    public float NightmareLateralSpeed;

    public float NightmareAppearDuration = 4f;

    public float NoiseVolumeChangeSpeed = 2f;
    public Vector2 NoisePlayerDistances;
    public Vector2 DistanceNoiseVolumeRanges;
    public Vector2 LookingAtNoiseVolumeRanges;

    public Vector3 NightmareSpeed;
    private bool _nightmareRunning = false;

    public AudioSource NoiseSource;

    public Volume globalVolume;
    private FilmGrain filmGrain;
    public Transform playerCamera;

    public float NoiseDistanceVal;
    public float NoiseLookAtVal;

    void Start()
    {
        if(!Player) Player = FindAnyObjectByType<PlayerController>();
        _nightmareIntensity = NightmareLight.intensity;
        NightmareLight.intensity = 0;        
        NightmareMesh.transform.localScale = Vector3.zero;
        NoiseSource.Play();
        NoiseSource.volume = 0f;

        if (globalVolume.profile.TryGet(out filmGrain))
        {
            Debug.Log("Film Grain found!");
            filmGrain.intensity.value = 0f;
        }

        //playerCamera = Player.CameraLook.Cam.transform;
    }

    public void StartNightmare()
    {
        NightmareMesh.transform.DOScale(Vector3.one * NightmareMeshSize, NightmareAppearDuration).SetEase(Ease.InCubic);
        NightmareLight.DOIntensity(_nightmareIntensity, NightmareAppearDuration).SetEase(Ease.InCubic);
        _nightmareRunning = true;

        //noise crackles
        Sequence noise = DOTween.Sequence();

        //float volume = 0.5f;
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0.5f, 0.15f));
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0f, 0.15f)).SetDelay(0.25f);
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0.75f, 0.1f)).SetDelay(0.3f);
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0f, 0.15f)).SetDelay(0.15f);
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0.55f, 0.1f)).SetDelay(0.35f);
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0.025f, 0.4f)).SetDelay(0.5f);
        noise.Append(DOTween.To(() => NoiseSource.volume, (float v) => NoiseSource.volume = v, 0f, 1f)).SetDelay(0.5f);

        // start danger music
        DOVirtual.DelayedCall(NightmareAppearDuration, () => {
            AudioManager.Instance.SetDanger(true);
            GUIManager.Instance.WriteText("what is that", 2f);
            });
    }

    
    void Update()
    {
        if (_nightmareRunning)
        {
            this.transform.position += NightmareSpeed * Time.deltaTime;
            Vector3 targetYZ = Player.transform.position;
            targetYZ.x = this.transform.position.x;
            Vector3 YZmove = (targetYZ - this.transform.position).normalized * NightmareLateralSpeed * Time.deltaTime;
            this.transform.position += YZmove;

            // update light
            NightmareLight.transform.LookAt(Player.transform.position);
            NightmareMesh.transform.LookAt(Player.transform.position);

            // update noise
            // how much are we looking at the nighmare
            float cameraLookFac = Vector3.Dot(NightmareMesh.transform.forward * -1f, playerCamera.forward);
           
            //distance noise
            float distance = Vector3.Distance(this.transform.position, Player.transform.position);
            float distFac = Mathf.Clamp(distance, NoisePlayerDistances.x, NoisePlayerDistances.y) / NoisePlayerDistances.y;
            float noiseDistanceVal = Mathf.Lerp(DistanceNoiseVolumeRanges.x, DistanceNoiseVolumeRanges.y, distFac);
            NoiseDistanceVal = noiseDistanceVal;

            float noiseCamLookFac = Mathf.Clamp(cameraLookFac, 0f, 0.5f) * 2f;
            float noiseCamLookVal = Mathf.Lerp(LookingAtNoiseVolumeRanges.x, LookingAtNoiseVolumeRanges.y, noiseCamLookFac);
            
            NoiseLookAtVal = noiseCamLookVal;

            float currentTarget = noiseDistanceVal > noiseCamLookVal ? noiseDistanceVal : noiseCamLookVal;
            NoiseSource.volume = Mathf.MoveTowards(NoiseSource.volume, currentTarget, NoiseVolumeChangeSpeed * Time.deltaTime);

            float pitch = 1f;
            if (distance <= 30f)
            {
                float fac = distance / 30f;
                pitch = Mathf.Lerp(1.5f, 1f, fac);
            }
            NoiseSource.pitch = pitch;

            // update film grain
            float distGrainVal = Mathf.Lerp(0.7f, 0.0f, distFac);            
            float cameraLookGrainVal = Mathf.Clamp(cameraLookFac, 0f, 0.5f) * 2f;

            filmGrain.intensity.value = cameraLookGrainVal > distGrainVal ? cameraLookGrainVal : distGrainVal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawLine(transform.position, transform.position + NightmareSpeed);
    }
}
