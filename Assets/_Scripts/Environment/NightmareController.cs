using DG.Tweening;
using UnityEngine;

public class NightmareController : MonoBehaviour
{
    public PlayerController Player;
    public GameObject NightmareMesh;
    public Light NightmareLight;
    private float _nightmareIntensity;
    public float NightmareMeshSize;

    public float NightmareLateralSpeed;

    public float NightmareAppearDuration = 4f;

    public Vector2 NoisePlayerDistances;
    public Vector2 DistanceNoiseVolumeRanges;

    public Vector3 NightmareSpeed;
    private bool _nightmareRunning = false;

    public AudioSource NoiseSource;

    void Start()
    {
        if(!Player) Player = FindAnyObjectByType<PlayerController>();
        _nightmareIntensity = NightmareLight.intensity;
        NightmareLight.intensity = 0;        
        NightmareMesh.transform.localScale = Vector3.zero;
        NoiseSource.Play();
        NoiseSource.volume = 0f;
    }

    public void StartNightmare()
    {
        NightmareMesh.transform.DOScale(Vector3.one * NightmareMeshSize, NightmareAppearDuration).SetEase(Ease.InCubic);
        NightmareLight.DOIntensity(_nightmareIntensity, NightmareAppearDuration).SetEase(Ease.InCubic);
        _nightmareRunning = true;

        //noise crackles
        Sequence noise = DOTween.Sequence();

        float volume = 0.5f;
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

            // update film grain
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawLine(transform.position, transform.position + NightmareSpeed);
    }
}
