using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Ambient Sources")]
    public AudioSource ambientSourceA;
    public AudioSource ambientSourceB;

    private AudioSource currentAmbient;
    private AudioSource nextAmbient;

    [Header("Other Sources")]
    public AudioSource musicSource;
    public AudioSource dangerSource;

    [Header("Settings")]
    public float dangerFadeSpeed = 2f;
    public float ambientFadeSpeed = 0.5f;
    public float DangerVolumeMutiplier = 0.5f;

    private float targetDangerVolume = 0f;
    private float nextAmbientVolumeTarget = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            

            currentAmbient = ambientSourceA;
            nextAmbient = ambientSourceB;

            currentAmbient.volume = 1f;
            nextAmbient.volume = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Danger fade
        if (dangerSource != null)
        {
            dangerSource.volume = Mathf.Lerp(
                dangerSource.volume,
                targetDangerVolume* DangerVolumeMutiplier,
                Time.deltaTime * dangerFadeSpeed
            );
        }

        // Ambient crossfade
        if (nextAmbient.isPlaying)
        {
            nextAmbient.volume = Mathf.Lerp(
                nextAmbient.volume,
                nextAmbientVolumeTarget,
                Time.deltaTime * ambientFadeSpeed
            );

            currentAmbient.volume = Mathf.Lerp(
                currentAmbient.volume,
                0f,
                Time.deltaTime * ambientFadeSpeed
            );

            // When fully faded, swap
            if (nextAmbient.volume > 0.99f)
            {
                currentAmbient.Stop();

                // Swap references
                var temp = currentAmbient;
                currentAmbient = nextAmbient;
                nextAmbient = temp;
            }
        }
    }

    // ------------------------
    // AMBIENT (CROSSFADE)
    // ------------------------
    public void PlayAmbient(AudioClip clip, float volume = 1f)
    {
        if (currentAmbient.clip == clip) return;

        nextAmbient.clip = clip;
        nextAmbient.volume = 0f;
        nextAmbient.Play();

        nextAmbientVolumeTarget = volume;
    }

    // ------------------------
    // MUSIC
    // ------------------------
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    // ------------------------
    // DANGER MUSIC
    // ------------------------
    public void SetDanger(bool active)
    {
        if (active)
        {
            if (!dangerSource.isPlaying)
                dangerSource.Play();

            targetDangerVolume = 1f;
        }
        else
        {
            targetDangerVolume = 0f;
        }
    }

    public void SetDangerClip(AudioClip clip)
    {
        dangerSource.clip = clip;
    }

    // ------------------------
    // SFX
    // ------------------------
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
