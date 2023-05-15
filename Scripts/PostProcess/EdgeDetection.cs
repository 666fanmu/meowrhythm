
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SweetCandy.PostProcess
{
    [Serializable,
     VolumeComponentMenuForRenderPipeline("Addition-Post-processing/EdgeDetection", typeof(UniversalRenderPipeline))]
    public sealed class EdgeDetection : VolumeComponent, IPostProcessComponent
    {
        public ColorParameter EdgeColor = new ColorParameter(Color.black);
        public FloatParameter Threshold = new FloatParameter(1);

        public bool IsActive() => true;

        public bool IsTileCompatible() => false;
    }
}