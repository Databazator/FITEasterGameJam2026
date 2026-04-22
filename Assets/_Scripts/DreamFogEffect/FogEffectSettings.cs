using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("DreamFogEffect")]
public sealed class FogEffectSettings : VolumeComponent, IPostProcessComponent
{
    public LayerMaskParameter ExcludeObjectMask = new LayerMaskParameter(0);

    public FloatParameter FogDistance = new FloatParameter(100.0f);

    public ColorParameter FogColor = new ColorParameter(new Color(58f/256f, 64f/256f, 103f/255f));

    public bool IsActive()
    {
        return active;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
    
}
