
using UnityEngine.Rendering.Universal;

namespace SweetCandy.PostProcess
{
    internal sealed class EdgeDetectionRenderPassFeature : ScriptableRendererFeature
    {
        private EdgeDetectionRenderPass _edgeDetectionRenderPass;
    
        public override void Create()
        {
            _edgeDetectionRenderPass = new EdgeDetectionRenderPass(RenderPassEvent.BeforeRenderingPostProcessing);
        }
    
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            _edgeDetectionRenderPass.SetUpRenderTarget(renderer.cameraColorTarget);
            renderer.EnqueuePass(_edgeDetectionRenderPass);
        }
    }
}
