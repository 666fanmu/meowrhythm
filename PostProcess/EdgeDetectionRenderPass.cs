
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SweetCandy.PostProcess
{
    public sealed class EdgeDetectionRenderPass : ScriptableRenderPass
    {
        private static readonly string renderTag = "EdgeDetection";
        private static readonly int MainTex_Id = Shader.PropertyToID("_MainTex");
        private static readonly int Threshold_Id = Shader.PropertyToID("_Threshold");
        private static readonly int EdgeColor_Id = Shader.PropertyToID("_EdgeColor");
        private static readonly int TempTarget_Id = Shader.PropertyToID("_TempTarget");
        private RenderTargetIdentifier _currentTarget;
        private Material _edgeDetectionMaterial;
        private EdgeDetection _edgeDetection;

        public EdgeDetectionRenderPass(RenderPassEvent evt)
        {
            renderPassEvent = evt;
            var shader = Shader.Find("Custom/EdgeDetection");
            if (shader == null)
            {
                Debug.LogError("Shader not found!");
            }

            _edgeDetectionMaterial = CoreUtils.CreateEngineMaterial(shader);
        }

        public void SetUpRenderTarget(in RenderTargetIdentifier renderTargetIdentifier)
        {
            _currentTarget = renderTargetIdentifier;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_edgeDetectionMaterial == null)
            {
                Debug.LogError("Material created failed!");
                return;
            }

            if (!renderingData.cameraData.postProcessEnabled)
                return;
        
            _edgeDetection = VolumeManager.instance.stack.GetComponent<EdgeDetection>();
            if (_edgeDetection == null || !_edgeDetection.IsActive())
                return;
        

            var commandBuffer = CommandBufferPool.Get(renderTag);
            Render(commandBuffer, renderingData);
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        void Render(CommandBuffer commandBuffer, in RenderingData renderingData)
        {
            ref var source = ref _currentTarget;
            var destination = TempTarget_Id;
        
            int width = renderingData.cameraData.camera.scaledPixelWidth;
            int height = renderingData.cameraData.camera.scaledPixelHeight;

            _edgeDetectionMaterial.SetColor(EdgeColor_Id, _edgeDetection.EdgeColor.value);
            _edgeDetectionMaterial.SetFloat(Threshold_Id, _edgeDetection.Threshold.value);

            commandBuffer.SetGlobalTexture(MainTex_Id, source);
            commandBuffer.GetTemporaryRT(destination, width, height,0,FilterMode.Point,RenderTextureFormat.Default);
            commandBuffer.Blit(source,destination);
            commandBuffer.Blit(destination,source,_edgeDetectionMaterial);
        
        }
    }
}