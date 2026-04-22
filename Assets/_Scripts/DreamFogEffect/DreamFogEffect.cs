using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DreamFogEffect : ScriptableRendererFeature
{
    DreamFogRenderPass pass;

    public override void Create()
    {
        pass = new DreamFogRenderPass();
        name = "DreamFogEffect";
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        FogEffectSettings settings = VolumeManager.instance.stack.GetComponent<FogEffectSettings>();

        if (settings != null && settings.active && settings.IsActive())
        {
            pass.ConfigureInput(ScriptableRenderPassInput.Depth);
            renderer.EnqueuePass(pass);
        }
    }
    protected override void Dispose(bool disposing)
    {
        pass.Dispose();
        base.Dispose(disposing);
    }
}

public class DreamFogRenderPass: ScriptableRenderPass
{
    Material _material;
    Material _maskMaterial;


    private RTHandle tempTexHandle;
    private RTHandle maskedObjectsHandle;

    public DreamFogRenderPass()
    {
        profilingSampler = new ProfilingSampler("DreamFog");
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    private void CreateMaterials()
    {
        var shader = Shader.Find("Shader Graphs/DepthFog");

        if (shader == null)
        {
            Debug.LogError("Cannot find shader: \"Shader Graphs/DepthFog\".");
            return;
        }

        _material = new Material(shader);

        shader = Shader.Find("FogEffect/MaskObject");

        if (shader == null)
        {
            Debug.LogError("Cannot find shader: \"FogEffect/MaskObject\".");
            return;
        }

        _maskMaterial = new Material(shader);
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        ResetTarget();

        var descriptor = cameraTextureDescriptor;

        descriptor.msaaSamples = 1;
        descriptor.depthBufferBits = (int)DepthBits.None;
        RenderingUtils.ReAllocateIfNeeded(ref tempTexHandle, descriptor);

        descriptor.colorFormat = RenderTextureFormat.R8;
        RenderingUtils.ReAllocateIfNeeded(ref maskedObjectsHandle, descriptor);

        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isPreviewCamera)
        {
            return;
        }

        if (_material == null || _maskMaterial == null)
        {
            CreateMaterials();
        }

        CommandBuffer cmd = CommandBufferPool.Get();

        // Set Recall effect properties.
        var settings = VolumeManager.instance.stack.GetComponent<FogEffectSettings>();
        _material.SetFloat("_FogDistance", settings.FogDistance.value);
        _material.SetColor("_FogColor", settings.FogColor.value);

        RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

        // Perform the Blit operations for the Recall effect.
        using (new ProfilingScope(cmd, profilingSampler))
        {
            CoreUtils.SetRenderTarget(cmd, maskedObjectsHandle);
            CoreUtils.ClearRenderTarget(cmd, ClearFlag.All, Color.black);

            var camera = renderingData.cameraData.camera;
            

            var cullingResults = renderingData.cullResults;

            var sortingSettings = new SortingSettings(camera);

            FilteringSettings filteringSettings =
                new FilteringSettings(RenderQueueRange.all, settings.ExcludeObjectMask.value);

            ShaderTagId shaderTagId = new ShaderTagId("UniversalForward");

            DrawingSettings drawingSettingsLit = new DrawingSettings(shaderTagId, sortingSettings)
            {
                overrideMaterial = _maskMaterial
            };

            RendererListParams rendererParams = new RendererListParams(cullingResults, drawingSettingsLit, filteringSettings);
            RendererList rendererList = context.CreateRendererList(ref rendererParams);

            cmd.DrawRendererList(rendererList);

            shaderTagId = new ShaderTagId("SRPDefaultUnlit");

            DrawingSettings drawingSettingsUnlit = new DrawingSettings(shaderTagId, sortingSettings)
            {
                overrideMaterial = _maskMaterial
            };

            rendererParams = new RendererListParams(cullingResults, drawingSettingsUnlit, filteringSettings);
            rendererList = context.CreateRendererList(ref rendererParams);

            cmd.DrawRendererList(rendererList);

            _material.SetTexture("_MaskedObjects", maskedObjectsHandle);
            _material.SetFloat("_FarClipPlane", camera.farClipPlane);

            Blitter.BlitCameraTexture(cmd, cameraTargetHandle, tempTexHandle);
            Blitter.BlitCameraTexture(cmd, tempTexHandle, cameraTargetHandle, _material, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    {
        tempTexHandle?.Release();
        maskedObjectsHandle?.Release();
    }
}
