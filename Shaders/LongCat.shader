Shader "Custom/LongCat"
{
    Properties
    {
        _BaseColor("Base Color",Color)=(1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                half4 vertexColor: COLOR;
                half4 normal:NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                half4 vertexColor:TEXCOORD0;
                float3 worldNormal:TEXCOORD1;
                float3 worldPos:TEXCOORD2;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.vertexColor = IN.vertexColor;
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normal.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings i) : SV_Target
            {
                Light light = GetMainLight();
                half3 worldLightDir = normalize(TransformObjectToWorldDir(light.direction));
                //half3 worldViewDir = normalize(GetWorldSpaceViewDir(i.worldPos));
                half3 worldNormal = normalize(i.worldNormal);

                half3 albedo = i.vertexColor.rgb * _BaseColor.rgb;
                half3 halfLambert = light.color * albedo * (max(0, dot(worldLightDir, worldNormal)) * 0.5 + 0.5);

                return half4(halfLambert, 1);
            }
            ENDHLSL
        }
    }
}