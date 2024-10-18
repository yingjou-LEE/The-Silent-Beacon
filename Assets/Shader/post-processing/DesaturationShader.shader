Shader "Hidden/DesaturationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)));
                o.uv = v.uv;
                return o;
            }

            Texture2D _MainTex;
            Texture2D _NoiseTexture;
            Texture2D _CameraColor;
            TEXTURE2D_X_FLOAT(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);
            
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_CameraColor);
            float _DesaturationFactor;
            float _NoiseScale;

            void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
            {
                float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
                Out =  luma.xxx + Saturation.xxx * (In - luma.xxx);
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float rawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv).r;
                float sceneZ = (unity_OrthoParams.w == 0) ? LinearEyeDepth(rawDepth, _ZBufferParams) : LinearDepthToEyeDepth(rawDepth);
                float depth = LinearEyeDepth(rawDepth, _ZBufferParams);
                
                float4 cameraColor = SAMPLE_TEXTURE2D(_CameraColor, sampler_CameraColor, i.uv);
                float4 noise = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_CameraColor, i.uv * _NoiseScale * (1 + saturate(1 - saturate(depth/30))));
                float3 res = 0;
                Unity_Saturation_float(cameraColor.rgb, _DesaturationFactor, res);
                res = lerp(res, res * noise.r ,0.4);
				return float4(res,0);
            }
            ENDHLSL
        }
    }
}
