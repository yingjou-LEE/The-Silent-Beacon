using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DesaturationFeature : ScriptableRendererFeature
{
    //面板设置的参数
    [System.Serializable]
    public class CustomSettings
    {
        
        public Material material;
        public RenderPassEvent passEvent;
        [Range(0,1)]
        public float desaturationFactor;
        public Texture2D noiseTexture;
        [Range(0.01f,10)]
        public float noiseScale = 1;
       
    }
    
    public CustomSettings settings = new CustomSettings();
    private DesaturationPass desaturationPass;
    
    public override void Create()
    {
        desaturationPass = new DesaturationPass();
        
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(settings.material == null)
        {
            Debug.LogWarning("Material is null");
            return;
        }
        desaturationPass.Setup(settings.material,settings,renderingData);
        renderer.EnqueuePass(desaturationPass);
    }

    protected override void Dispose(bool disposing)
    {
        desaturationPass.Dispose();
    }
}

public class DesaturationPass : ScriptableRenderPass
{
    public Material passMaterial;
    public RTHandle ColorRTHandle;
    public DesaturationFeature.CustomSettings settings;

    public void Setup(Material material, DesaturationFeature.CustomSettings passSettings, RenderingData renderingData)
    {
        passMaterial = material;
        renderPassEvent = passSettings.passEvent;
        settings = passSettings;
        RenderTextureDescriptor colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;

        colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
        RenderingUtils.ReAllocateIfNeeded(ref ColorRTHandle, colorCopyDescriptor, name:"ColorRTHandle");
        
    }

    

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if(passMaterial == null)
        {
            Debug.LogWarning("Material is null");
            return;
        }

        if (renderingData.cameraData.isPreviewCamera)
            return;
        CommandBuffer cmd = CommandBufferPool.Get("DesaturationPass");
        CameraData cameraData = renderingData.cameraData;

        RTHandle sourceRTHandle = null;

        sourceRTHandle = cameraData.renderer.cameraColorTargetHandle;
        
        Blitter.BlitCameraTexture(cmd,sourceRTHandle,ColorRTHandle);
        cmd.SetGlobalTexture("_CameraColor",ColorRTHandle);
        cmd.SetGlobalFloat("_DesaturationFactor",settings.desaturationFactor);
        cmd.SetGlobalTexture("_NoiseTexture",settings.noiseTexture);
        cmd.SetGlobalFloat("_NoiseScale",settings.noiseScale);
        
        CoreUtils.SetRenderTarget(cmd,cameraData.renderer.cameraColorTargetHandle);
        cmd.Blit(ColorRTHandle,sourceRTHandle,passMaterial);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        
    }
    
    public void Dispose()
    {
        ColorRTHandle?.Release();
    }
}
