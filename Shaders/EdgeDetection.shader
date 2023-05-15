Shader "Custom/EdgeDetection"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _EdgeColor("EdgeColor",Color)=(0,0,0,1)
        _Threshold("Threshold",float)=0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        ZTest Always Cull Off ZWrite Off

        Pass
        {
            HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            ENDHLSL

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct a2v
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv[9] : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _Threshold;
            half4 _EdgeColor;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.positionOS.xyz);
                half2 uv = v.texcoord;
                o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, 1);
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, 1);
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
                o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
                o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
                o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, -1);
                o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, -1);
                return o;
            }

            half luminance(half3 RGBColor)
            {
                return 0.2125 * RGBColor.r + 0.7154 * RGBColor.g + 0.0721 * RGBColor.b;
            }

            half sobel(v2f i)
            {
                const half sobelX[9] =
                {
                    -1, -2, -1,
                    0, 0, 0,
                    1, 2, 1
                };
                const half sobelY[9] =
                {
                    -1, 0, 1,
                    -2, 0, 2,
                    -1, 0, 1
                };
                half Gx = 0, Gy = 0;
                for (int it = 0; it < 9; ++it)
                {
                    half texColor = luminance(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[it]));
                    Gx += sobelX[it] * texColor;
                    Gy += sobelY[it] * texColor;
                }
                //the edge is at the place where the value is greater
                return abs(Gx) + abs(Gy);
            }

            half4 frag(v2f i) : SV_Target
            {
                half3 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[4]);
                half3 finalColor = lerp(texColor, _EdgeColor.rgb, saturate(sobel(i) - _Threshold));
                return half4(finalColor, 1);
            }
            ENDHLSL
        }
    }FallBack Off
}