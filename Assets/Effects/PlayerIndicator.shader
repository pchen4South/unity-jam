Shader "GroundEffects/PlayerIndicator"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;
			float _SinAngle;
			float _CosAngle;

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
				float2x2 rotation = float2x2(
					_CosAngle, _SinAngle,
					-_SinAngle, _CosAngle
				);

				float2 uv = v.uv - float2(.5, .5);
				float2 uvRotated = mul(rotation, uv);
				float2 uvFinal = uvRotated + float2(.5, .5);

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = uvFinal;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 tx = tex2D(_MainTex, i.uv);
				fixed4 c = _Color;

				c.a = tx.a;
				return c;
			}
			ENDCG
		}
	}
}