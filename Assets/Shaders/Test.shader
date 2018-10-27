// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/TestShader" {     
     Properties{
         _MainTex("Texture", 2D) = "white"
     }
    
     SubShader {
         Tags
         {
             "Queue" = "Transparent"
         }
         Pass {

             Blend SrcAlpha OneMinusSrcAlpha

             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
 
             #include "UnityCG.cginc"
             
             struct appdata_t {
                 float4 vertex: POSITION;
                 float3 uv : TEXCOORD0;
             };
 
             struct v2f {
                 float4 vertex: SV_POSITION;
                 float3 uv : TEXCOORD0;
             };
             
             float4 _MainTex_ST;

             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.uv = v.uv;
                 return o;
             }
 
            sampler2D _MainTex;

             fixed4 frag (v2f i) : SV_Target
             {
                 float4 color = tex2D(_MainTex, i.uv);
                 return color;
             }
             ENDCG 
         }
     }    
 }