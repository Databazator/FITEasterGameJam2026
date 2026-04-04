using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CameraEffects : MonoBehaviour
{
    private Camera _camera;

    public ParticleSystem SpeedlinesParticleSys;
    private Material _speedlinesMaterial;

    private float _startFOV;
    public float StartFOV => _startFOV;

    private Tweener _fovTweener;
    private Tweener _shakeTweener;
    private Tweener _speedlinesTweener;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _startFOV = _camera.fieldOfView;

        if (!SpeedlinesParticleSys) Debug.LogWarning("SpeedlinesSystem not set");
        else
        {
            var renderer = SpeedlinesParticleSys.GetComponent<ParticleSystemRenderer>();
            _speedlinesMaterial = renderer.material;
            Color transparent = _speedlinesMaterial.color;
            transparent.a = 0f;
            _speedlinesMaterial.color = transparent;
        }
    }

    public void SetFOV(float target, float duration)
    {
        if(_fovTweener != null && _fovTweener.IsActive())
        {
            _fovTweener.Kill();
            _fovTweener = null;
        }

        _fovTweener = _camera.DOFieldOfView(target, duration).SetEase(Ease.InOutQuad);
    }

    public void UnsetFOV(float duration)
    {
        SetFOV(_startFOV, duration);
    }

    public void ShakeCamera(float strength, float duration)
    {
        if(_shakeTweener != null && _shakeTweener.IsActive())
        {
            _shakeTweener.Kill();
            _shakeTweener = null;
        }
        _shakeTweener = _camera.DOShakeRotation(duration, strength);
    }

    public void ShowSpeedlines(float duration)
    {
        SetSpeedlinesOpacity(1f, duration);
    }

    public void FadeSpeedlines(float duration)
    {
        SetSpeedlinesOpacity(0f, duration);
    }

    void SetSpeedlinesOpacity(float value, float duration)
    {
        if (_speedlinesTweener != null && _speedlinesTweener.IsActive())
        {
            _speedlinesTweener.Kill();
            _speedlinesTweener = null;
        }

        if(_speedlinesMaterial != null)
        {
            Color targetColor = _speedlinesMaterial.color;
            targetColor.a = value;
            _speedlinesMaterial.DOColor(targetColor, duration).SetEase(Ease.InOutQuad);
        }
    }
}
