Shader "FogEffect/MaskObject"
{
    SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
			"RenderPipeline" = "UniversalPipeline"
		}

		Pass
		{
			ZTest LEqual
			ZWrite On

			Blend SrcColor OneMinusSrcColor

			Tags
			{
				"LightMode" = "UniversalForward"
			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			sampler2D _CameraDepthTexture;

            struct appdata
            {
                float4 positionOS : Position;
            };

            struct v2f
            {
				float depth : DEPTH;
            };

            v2f vert (appdata v, out float4 positionCS : SV_Position)
            {
                v2f o;
                positionCS = TransformObjectToHClip(v.positionOS.xyz);				
				o.depth = -mul(UNITY_MATRIX_MV, v.positionOS).z * _ProjectionParams.w;
                return o;
            }
			
            float4 frag (v2f i, float4 positionSS : VPOS) : SV_Target
            {
				float2 screenUV = positionSS.xy / _ScreenParams.xy;
				float screenDepth = Linear01Depth(tex2D(_CameraDepthTexture, screenUV).r, _ZBufferParams);

				return step(i.depth - 0.0001f, screenDepth);
            }
            ENDHLSL
        }
    }
}
