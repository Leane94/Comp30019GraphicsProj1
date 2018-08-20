//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{

	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			struct vertIn
			{
				float4 vertex : POSITION;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
			    // increase frequency, decrease amplitude, increase move speed
				v.vertex.y += sin(v.vertex.x * 5.0f + _Time.y * 2.0f) * 0.05f;

				vertOut o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				return float4(0.0f, 0.4f, 1.0f, 0.8f);
			}
			ENDCG
		}
	}
}