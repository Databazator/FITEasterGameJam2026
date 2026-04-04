using DG.Tweening;
using UnityEngine;

public class NightmareController : MonoBehaviour
{
    public GameObject NightmareMesh;
    public Light NightmareLight;
    private float _nightmareIntensity;
    private float _nightmareMeshSize;

    public float NightmareAppearDuration = 3f;

    public Vector3 NightmareSpeed;
    private bool _nightmareRunning = false;

    void Start()
    {
        _nightmareIntensity = NightmareLight.intensity;
        NightmareLight.intensity = 0;
        _nightmareMeshSize = NightmareMesh.transform.localScale.x;
        NightmareMesh.transform.localScale = Vector3.zero;
    }

    public void StartNightmare()
    {
        NightmareMesh.transform.DOScale(Vector3.one * _nightmareMeshSize, NightmareAppearDuration).SetEase(Ease.InCubic);
        NightmareLight.DOIntensity(_nightmareIntensity, NightmareAppearDuration).SetEase(Ease.InCubic);
        _nightmareRunning = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(_nightmareRunning)
            this.transform.position += NightmareSpeed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawLine(transform.position, transform.position + NightmareSpeed);
    }
}
