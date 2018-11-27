//RealToon V5.0.1
//MJQStudioWorks
//2018

Shader "RealToon/Version 5/Tessellation/Refraction" {
    Properties {

		[Enum(Off,2,On,0)] _DoubleSided("Double Sided", int) = 2

        _RefractionIntensity ("Refraction Intensity", Range(0, 1)) = 1

        _MainColor ("Main Color", Color) = (0.7843137,0.7843137,0.7843137,1)
        [MaterialToggle] _MainColorAffectTexture ("Main Color Affect Texture", Float ) = 0

        _TextureIntesnity ("Texture Intesnity", Range(0, 1)) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _ReduceTextureQuality ("Reduce Texture Quality", Float ) = 0
        [MaterialToggle] _TexturePatternStyle ("Texture Pattern Style", Float ) = 0

        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalMapIntensity ("Normal Map Intensity", Float ) = 0

        _Saturation ("Saturation", Range(0, 2)) = 1

        _SelfLitIntensity ("Intensity", Range(0, 1)) = 0
        _SelfLitColor ("Color", Color) = (1,1,1,1)
        _SelfLitPower ("Power", Float ) = 2
        [MaterialToggle] _SelfLitHighContrast ("High Contrast", Float ) = 1
        _MaskSelfLit ("Mask Self Lit", 2D) = "white" {}

        _GlossIntensity ("Intensity", Range(0, 1)) = 1
        _Glossiness ("Glossiness", Range(0, 1)) = 0.6
        _GlossSoftness ("Softness", Range(0, 1)) = 0
        _GlossColor ("Color", Color) = (1,1,1,1)
        _GlossColorPower ("Color Power", Float ) = 10
        _MaskGloss ("Mask Gloss", 2D) = "white" {}

        _GlossTexture ("Gloss Texture", 2D) = "black" {}
        _GlossTextureSoftness ("Softness", Float ) = 0
        _GlossTextureRotate ("Rotate", Float ) = 0
        [MaterialToggle] _GlossTextureFollowObjectRotation ("Follow Object Rotation", Float ) = 0
        _GlossTextureFollowLight ("Follow Light", Range(0, 1)) = 0

        _OverallShadowColor ("Overall Shadow Color", Color) = (0,0,0,1)
        _OverallShadowColorPower ("Overall Shadow Color Power", Float ) = 1

        [MaterialToggle] _SelfShadowShadowTAtViewDirection ("Self Shadow & ShadowT At View Direction", Float ) = 0

        _HighlightColor ("Highlight Color", Color) = (1,1,1,1)
        _HighlightColorPower ("Highlight Color Power", Float ) = 1

        _SelfShadowIntensity ("Self Shadow Intensity", Range(0, 1)) = 1
        _SelfShadowThreshold ("Threshold", Range(0, 1)) = 0.85
        [MaterialToggle] _VertexColorGreenControlSelfShadowThreshold ("Vertex Color Green Control Self Shadow Threshold", Float ) = 0
        _SelfShadowHardness ("Hardness", Range(0, 1)) = 1
        _SelfShadowColor ("Color", Color) = (1,1,1,1)
        _SelfShadowColorPower ("Color Power", Float ) = 1
        [MaterialToggle] _SelfShadowAffectedByLightShadowStrength ("Self Shadow Affected By Light Shadow Strength", Float ) = 0

        _SmoothObjectNormal ("Smooth Object Normal", Range(0, 1)) = 0
        [MaterialToggle] _VertexColorBlueControlSmoothObjectNormal ("Vertex Color Blue Control Smooth Object Normal", Float ) = 0
        _XYZPosition ("XYZ Position", Vector) = (0,0,0,0)
        _XYZHardness ("XYZ Hardness", Float ) = 14
        [MaterialToggle] _ShowNormal ("Show Normal", Float ) = 0

        _ShadowColorTexture ("Shadow Color Texture", 2D) = "white" {}
        _ShadowColorTexturePower ("Power", Float ) = 0

        _ShadowTIntensity ("ShadowT Intensity", Range(0, 1)) = 1
        _ShadowT ("ShadowT", 2D) = "white" {}
        _ShadowTLightThreshold ("Light Threshold", Float ) = 0.5
        _ShadowTShadowThreshold ("Shadow Threshold", Float ) = 0
        _ShadowTColor ("Color", Color) = (1,1,1,1)
        _ShadowTColorPower ("Color Power", Float ) = 1
        _ShadowTHardness ("Hardness", Range(0, 1)) = 1

		[Toggle(N_F_STIS_ON)] _N_F_STIS ("Show In Shadow", Float ) = 0

		[Toggle(N_F_STIAL_ON)] _N_F_STIAL ("Show In Ambient Light", Float ) = 0
        _ShowInAmbientLightIntensity ("Show In Ambient Light Intensity", Range(0, 1)) = 1
        _ShowInAmbientLightShadowThreshold ("Show In Ambient Light & Shadow Threshold", Float ) = 0.4

        [MaterialToggle] _LightFalloffAffectShadowT ("Light Falloff Affect ShadowT", Float ) = 0

        _PTexture ("PTexture", 2D) = "white" {}
        _PTexturePower ("Power", Float ) = 1

        [MaterialToggle] _GIFlatShade ("GI Flat Shade", Float ) = 0
        _GIShadeThreshold ("GI Shade Threshold", Range(0, 1)) = 0

        [MaterialToggle] _LightAffectShadow ("Light Affect Shadow", Float ) = 0
        _LightIntensity ("Light Intensity", Float ) = -1

        _EnvironmentalLightingIntensity ("Environmental Lighting Intensity", Float ) = 1

		_PointSpotlightIntensity ("Point and Spot Light Intensity", Float ) = 0.3
		_LightFalloffSoftness ("Light Falloff Softness", Range(0, 1)) = 1

        _CustomLightDirectionIntensity ("Intensity", Range(0, 1)) = 0
        _CustomLightDirection ("Custom Light Direction", Vector) = (0,0,10,0)
        [MaterialToggle] _CustomLightDirectionFollowObjectRotation ("Follow Object Rotation", Float ) = 0

        _ReflectionIntensity ("Intensity", Range(0, 1)) = 0
        _ReflectionRoughtness ("Roughtness", Float ) = 0
        _MaskReflection ("Mask Reflection", 2D) = "white" {}
        [MaterialToggle] _UseFReflection ("Use FReflection", Float ) = 0
        _FReflection ("FReflection", 2D) = "black" {}

        _RimLightUnfill ("Unfill", Float ) = 1.5
        _RimLightColor ("Color", Color) = (1,1,1,1)
        _RimLightColorPower ("Color Power", Float ) = 10
        _RimLightSoftness ("Softness", Range(0, 1)) = 1
        [MaterialToggle] _RimLightInLight ("Rim Light In Light", Float ) = 1
        [MaterialToggle] _LightAffectRimLightColor ("Light Affect Rim Light Color", Float ) = 0

        _Depth ("Depth", Float ) = 0.2
        _DepthEdgeHardness ("Edge Hardness", Range(0, 1)) = 0.1
        _DepthColor ("Color", Color) = (0.5,0.5,0.5,1)
        _DepthColorPower ("Color Power", Float ) = 1.8

		_TessellationSmoothness ("Smoothness", Range(0, 1)) = 0.5
        _TessellationTransition ("Tessellation Transition", Range(0, 1)) = 0.8
        _TessellationNear ("Tessellation Near", Float ) = 1
        _TessellationFar ("Tessellation Far", Float ) = 1

		_RefVal ("ID", int ) = 0
        [Enum(None,8,A,0,B,2)] _Oper("Set 1", int) = 8
        [Enum(None,8,A,6,B,7)] _Compa("Set 2", int) = 8

		[Toggle(N_F_CA_ON)] _N_F_CA ("Color Adjustment", Float ) = 0
		[Toggle(N_F_SL_ON)] _N_F_SL ("Self Lit", Float ) = 0
		[Toggle(N_F_GLO_ON)] _N_F_GLO ("Gloss", Float ) = 0
		[Toggle(N_F_GLOT_ON)] _N_F_GLOT ("Gloss Texture", Float ) = 0
		[Toggle(N_F_SS_ON)] _N_F_SS ("Self Shadow", Float ) = 0
		[Toggle(N_F_SON_ON)] _N_F_SON ("Smooth Object Normal", Float ) = 0
		[Toggle(N_F_SCT_ON)] _N_F_SCT ("Shadow Color Texture", Float ) = 0
		[Toggle(N_F_ST_ON)] _N_F_ST ("ShadowT", Float ) = 0
		[Toggle(N_F_PT_ON)] _N_F_PT ("PTexture", Float ) = 0
		[Toggle(N_F_CLD_ON)] _N_F_CLD ("Custom Light Direction", Float ) = 0
		[Toggle(N_F_R_ON)] _N_F_R ("Reflection", Float ) = 0
		[Toggle(N_F_FR_ON)] _N_F_FR ("FReflection", Float ) = 0
		[Toggle(N_F_RL_ON)] _N_F_RL ("Rim Light", Float ) = 0
		[Toggle(N_F_D_ON)] _N_F_D ("Depth", Float ) = 0
		[Enum(On,1,Off,0)] _ZWrite("ZWrite", int) = 1

    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull [_DoubleSided]
            ZWrite [_ZWrite]
            
			Stencil {
            	Ref[_RefVal]
            	Comp [_Compa]
            	Pass [_Oper]
            	Fail [_Oper]
            }
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

			#pragma multi_compile_instancing

            #pragma only_renderers d3d11 glcore gles3 metal xboxone ps4 wiiu switch
            #pragma target 4.6

			#pragma shader_feature N_F_CA_ON
			#pragma shader_feature N_F_SL_ON
			#pragma shader_feature N_F_GLO_ON
			#pragma shader_feature N_F_GLOT_ON
			#pragma shader_feature N_F_SS_ON
			#pragma shader_feature N_F_SCT_ON
			#pragma shader_feature N_F_ST_ON
			#pragma shader_feature N_F_STIS_ON
			#pragma shader_feature N_F_STIAL_ON 
			#pragma shader_feature N_F_SON_ON
			#pragma shader_feature N_F_PT_ON
			#pragma shader_feature N_F_CLD_ON
			#pragma shader_feature N_F_R_ON
			#pragma shader_feature N_F_FR_ON
			#pragma shader_feature N_F_RL_ON
			#pragma shader_feature N_F_D_ON

            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform half _SelfShadowHardness;
            uniform half _SelfShadowThreshold;
            uniform half4 _HighlightColor;
            uniform half4 _SelfShadowColor;
            uniform half _SelfShadowColorPower;
            uniform half _HighlightColorPower;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform half4 _MainColor;
            uniform sampler2D _ShadowColorTexture; uniform float4 _ShadowColorTexture_ST;
            uniform half _ShadowColorTexturePower;
            uniform sampler2D _ShadowT; uniform float4 _ShadowT_ST;
            uniform half _ShadowTShadowThreshold;
            uniform fixed _LightFalloffAffectShadowT;
            uniform half _ShadowTLightThreshold;
            uniform half _ShadowTHardness;
            uniform half4 _ShadowTColor;
            uniform half _ShadowTColorPower;
            uniform half4 _OverallShadowColor;
            uniform half _OverallShadowColorPower;
            uniform half _Glossiness;
            uniform half _GlossSoftness;
            uniform half4 _GlossColor;
            uniform half _GlossColorPower;
            uniform sampler2D _GlossTexture; uniform float4 _GlossTexture_ST;
            uniform half _GlossTextureFollowLight;
            uniform fixed _GlossTextureFollowObjectRotation;
            uniform sampler2D _MaskGloss; uniform float4 _MaskGloss_ST;
            uniform sampler2D _FReflection; uniform float4 _FReflection_ST;
            uniform half _ReflectionIntensity;
            uniform sampler2D _MaskReflection; uniform float4 _MaskReflection_ST;
            uniform half _GlossTextureSoftness;
            uniform half4 _SelfLitColor;
            uniform fixed _SelfLitHighContrast;
            uniform half _SelfLitPower;
            uniform half _SelfLitIntensity;
            uniform sampler2D _MaskSelfLit; uniform float4 _MaskSelfLit_ST;
            uniform sampler2D _PTexture; uniform float4 _PTexture_ST;
            uniform half _PTexturePower;
            half3 AL_GI( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            uniform fixed _RimLightInLight;
            uniform half _RimLightUnfill;
            uniform half4 _RimLightColor;
            uniform half _RimLightColorPower;
            uniform half _RimLightSoftness;
            half N_F_SS( half A , half B ){
            #ifdef N_F_SS_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SCT( half3 A ){
            #ifdef N_F_SCT_ON
            
            return A;
            
            #else
            
            return 1;
            
            #endif
            }
            
            half N_F_PT( half A ){
            #ifdef N_F_PT_ON
            
            return A;
            
            #else
            
            return 1;
            
            #endif
            }
            
            half3 N_F_ST( half3 A , half3 B ){
            #ifdef N_F_ST_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_GLO( half3 A , half3 B ){
            #ifdef N_F_GLO_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_GLOT( half A , half B ){
            #ifdef N_F_GLOT_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_R( half3 A , half3 B ){
            #ifdef N_F_R_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SL( half3 A , half3 B ){
            #ifdef N_F_SL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_RL( half3 A , half3 B ){
            #ifdef N_F_RL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SL_1( half3 A , half3 B ){
            #ifdef N_F_SL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_RL_1( half3 A , half3 B ){
            #ifdef N_F_RL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            uniform fixed _SelfShadowShadowTAtViewDirection;
            uniform half _XYZHardness;
            uniform float4 _XYZPosition;
            uniform half _SmoothObjectNormal;
            uniform fixed _ShowNormal;
            half3 N_F_SON( half3 A , half3 B ){
            #ifdef N_F_SON_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SON_1( half3 A ){
            #ifdef N_F_SON_ON
            
            return A;
            
            #else
            
            return 1;
            
            #endif
            }
            
            uniform half4 _CustomLightDirection;
            uniform fixed _CustomLightDirectionFollowObjectRotation;
            uniform half _CustomLightDirectionIntensity;
            half3 N_F_CLD( half3 A , half3 B ){
            #ifdef N_F_CLD_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            uniform fixed _VertexColorGreenControlSelfShadowSize;
            uniform fixed _VertexColorBlueControlSmoothObjectNormal;
            uniform fixed _LightAffectRimLightColor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform half _NormalMapIntensity;
            uniform half _ReduceTextureQuality;
            uniform fixed _UseFReflection;
            float3 Ref( half3 VR , half Mip ){
            float4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, VR, Mip);
            return DecodeHDR (skyData, unity_SpecCube0_HDR);
            }
            
            uniform half _ReflectionRoughtness;
            uniform fixed _GIFlatShade;
            uniform half _GIShadeThreshold;
            uniform half _GlossTextureRotate;
            uniform half _LightIntensity;
            uniform fixed _LightAffectShadow;
            uniform fixed _TexturePatternStyle;
            uniform half _TextureIntesnity;
            uniform half _RefractionIntensity;
            uniform fixed _MainColorAffectTexture;
            uniform half _Depth;
            uniform half _DepthEdgeHardness;
            uniform half4 _DepthColor;
            uniform half _DepthColorPower;
            half3 N_F_D( half3 A , half3 B ){
            #ifdef N_F_D_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_FR( half3 A , half3 B ){
            #ifdef N_F_FR_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_STIAL( half3 A ){
            #ifdef N_F_ST_ON
            
            	#ifdef N_F_STIAL_ON
            
            	return A;
            
            	#else
            
            	return 1;
            
            	#endif
            
            #else
            
            return 1;
            
            #endif
            }
            
            uniform half _ShowInAmbientLightIntensity;
            half3 N_F_STIS( half3 A ){
            #ifdef N_F_ST_ON
            
            	#ifdef N_F_STIS_ON
            
            	return A;
            
            	#else
            
            	return 1;
            
            	#endif
            
            #else
            
            return 1;
            
            #endif
            }
            
            uniform half _ShadowTIntensity;
            uniform half _ShowInAmbientLightShadowThreshold;
            uniform half _EnvironmentalLightingIntensity;
            half3 N_F_CA( half3 A , half3 B ){
            #ifdef N_F_CA_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            uniform half _Saturation;
            uniform half _SelfShadowIntensity;
            uniform fixed _SelfShadowAffectedByLightShadowStrength;
            fixed C_SS(){
            return _LightShadowData.x;
            }
            
            uniform half _GlossIntensity;
            uniform half _TessellationNear;
            uniform half _TessellationFar;
            uniform half _TessellationTransition;

			uniform float _TessellationSmoothness;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD5;
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID (v);
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float4 vertexColor : COLOR;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.vertexColor = v.vertexColor;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return lerp(_TessellationNear,_TessellationFar,saturate((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)*(1.0 - _TessellationTransition))));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;

					float3 pp[3];
					for (int i = 0; i < 3; ++i)
					pp[i] = v.vertex.xyz - vi[i].normal * (dot(v.vertex.xyz, vi[i].normal) - dot(vi[i].vertex.xyz, vi[i].normal));
					v.vertex.xyz = _TessellationSmoothness * (pp[0] * bary.x + pp[1] * bary.y + pp[2] * bary.z) + (1.0f - _TessellationSmoothness) * v.vertex.xyz;

                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.vertexColor = vi[0].vertexColor*bary.x + vi[1].vertexColor*bary.y + vi[2].vertexColor*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                half3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = lerp(half3(0,0,1),_NormalMap_var.rgb,_NormalMapIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = 1;
                half3 node_7067 = (tex2D( _GrabTexture, lerp(sceneUVs.rg,reflect(float3(sceneUVs.rg,0.0),normalDirection).rg,lerp(0.0,(-0.2),_RefractionIntensity))).rgb*_MainColor.rgb);
                half2 node_9146 = (float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg*distance(objPos.rgb,_WorldSpaceCameraPos));
                half2 _TexturePatternStyle_var = lerp( i.uv0, node_9146, _TexturePatternStyle );
                half4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(_TexturePatternStyle_var, _MainTex),0.0,_ReduceTextureQuality));
                half3 node_3924 = lerp(node_7067,lerp(node_7067,lerp( _MainTex_var.rgb, (_MainTex_var.rgb*_MainColor.rgb), _MainColorAffectTexture ),_MainTex_var.a),_TextureIntesnity);
                float node_3622 = 0.0;
				float node_4432 = 1.0;
                float3 node_76 = mul( unity_WorldToObject, float4((i.posWorld.rgb-objPos.rgb),0) ).xyz.rgb.rgb;
                half3 node_5206 = lerp(normalDirection,mul( unity_ObjectToWorld, float4(float3((_XYZPosition.r+(_XYZHardness*node_76.r)),(_XYZPosition.g+(_XYZHardness*node_76.g)),(_XYZPosition.b+(_XYZHardness*node_76.b))),0) ).xyz.rgb,lerp( _SmoothObjectNormal, (_SmoothObjectNormal*(1.0 - i.vertexColor.b)), _VertexColorBlueControlSmoothObjectNormal ));
                half3 node_3134 = N_F_SON( node_5206 , normalDirection );
                float node_2183 = 0.2;
                float node_8383 = 0.3;
                half3 node_1243 = (AL_GI( lerp(float3(node_3622,node_3622,node_3622),float3(node_4432,node_4432,node_4432),lerp( (node_3134*_GIShadeThreshold), float3(smoothstep( float2(node_2183,node_2183), float2(node_8383,node_8383), (node_3134.rb*_GIShadeThreshold) ),0.0), _GIFlatShade )) )*_EnvironmentalLightingIntensity);
                half4 _MaskSelfLit_var = tex2D(_MaskSelfLit,TRANSFORM_TEX(i.uv0, _MaskSelfLit));
                float node_932 = 1.0;
                half node_4736 = 1.0;
                half4 _ShadowColorTexture_var = tex2D(_ShadowColorTexture,TRANSFORM_TEX(i.uv0, _ShadowColorTexture));
                half3 node_9600 = N_F_SCT( lerp(_ShadowColorTexture_var.rgb,(_ShadowColorTexture_var.rgb*_ShadowColorTexture_var.rgb),_ShadowColorTexturePower) );
                half4 _PTexture_var = tex2D(_PTexture,TRANSFORM_TEX(node_9146, _PTexture));
                half node_8910 = N_F_PT( lerp((1.0 - _PTexturePower),1.0,_PTexture_var.r) );
                half3 node_7102 = (_OverallShadowColor.rgb*_OverallShadowColorPower);
                half3 node_338 = ((_ShadowTColor.rgb*_ShadowTColorPower)*node_9600*node_8910*node_7102);
                float node_7419 = 1.0;
                half node_6045 = lerp(0.0,0.22,_ShadowTHardness);
                half node_3187 = 0.22;
                half4 _ShadowT_var = tex2D(_ShadowT,TRANSFORM_TEX(i.uv0, _ShadowT));
                half3 node_8749 = lerp(float3(node_4736,node_4736,node_4736),lerp(node_338,float3(node_7419,node_7419,node_7419),smoothstep( node_6045, node_3187, (_ShowInAmbientLightShadowThreshold*_ShadowT_var.r) )),_ShadowTIntensity);
                float node_208 = 1.0;
                float4 node_6613_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_6613_p = lerp(float4(float4(_LightColor0.rgb,0.0).zy, node_6613_k.wz), float4(float4(_LightColor0.rgb,0.0).yz, node_6613_k.xy), step(float4(_LightColor0.rgb,0.0).z, float4(_LightColor0.rgb,0.0).y));
                float4 node_6613_q = lerp(float4(node_6613_p.xyw, float4(_LightColor0.rgb,0.0).x), float4(float4(_LightColor0.rgb,0.0).x, node_6613_p.yzx), step(node_6613_p.x, float4(_LightColor0.rgb,0.0).x));
                float node_6613_d = node_6613_q.x - min(node_6613_q.w, node_6613_q.y);
                float node_6613_e = 1.0e-10;
                half3 node_6613 = float3(abs(node_6613_q.z + (node_6613_q.w - node_6613_q.y) / (6.0 * node_6613_d + node_6613_e)), node_6613_d / (node_6613_q.x + node_6613_e), node_6613_q.x);;
                half node_938 = saturate(node_6613.b);
                half3 node_1860 = ((_SelfShadowColor.rgb*_SelfShadowColorPower)*node_7102*node_9600*node_8910);
                half3 node_6694 = (_LightIntensity+_LightColor0.rgb);
                half3 node_2779 = (_HighlightColor.rgb*_HighlightColorPower);
                float node_9345 = 0.0;
                float node_5992_ang = _GlossTextureRotate;
                float node_5992_spd = 1.0;
                float node_5992_cos = cos(node_5992_spd*node_5992_ang);
                float node_5992_sin = sin(node_5992_spd*node_5992_ang);
                float2 node_5992_piv = float2(0.5,0.5);
                half3 node_2832 = reflect(lerp(viewDirection,halfDirection,_GlossTextureFollowLight),normalDirection);
                half2 node_9280 = lerp( node_2832, mul( unity_WorldToObject, float4(node_2832,0) ).xyz.rgb, _GlossTextureFollowObjectRotation ).rg;
                half2 node_5992 = (mul(float2((-1*node_9280.r),node_9280.g)-node_5992_piv,float2x2( node_5992_cos, -node_5992_sin, node_5992_sin, node_5992_cos))+node_5992_piv);
                half2 node_8759 = (node_5992*0.5+0.5);
                half4 _GlossTexture_var = tex2Dlod(_GlossTexture,float4(TRANSFORM_TEX(node_8759, _GlossTexture),0.0,_GlossTextureSoftness));
                half4 _MaskGloss_var = tex2D(_MaskGloss,TRANSFORM_TEX(i.uv0, _MaskGloss));
                half3 node_2687 = N_F_GLO( lerp(node_2779,lerp(node_2779,(_GlossColor.rgb*_GlossColorPower),lerp(float3(node_9345,node_9345,node_9345),N_F_GLOT( _GlossTexture_var.r , smoothstep( 0.1, lerp(0.1,1.0,_GlossSoftness), pow(max(0,dot(halfDirection,normalDirection)),exp2(lerp(-2,15,_Glossiness))) ) ),_GlossIntensity)),_MaskGloss_var.r) , node_2779 );
                float node_4353 = 0.0;
                float node_3687 = 0.0;
                half3 node_4858 = lerp(float3(node_4353,node_4353,node_4353),(lerp( _RimLightColor.rgb, lerp(float3(node_3687,node_3687,node_3687),_RimLightColor.rgb,_LightColor0.rgb), _LightAffectRimLightColor )*_RimLightColorPower),smoothstep( 1.7, lerp(1.7,0.29,_RimLightSoftness), pow(1.0-max(0,dot(normalDirection, viewDirection)),(1.0 - _RimLightUnfill)) ));
                half3 node_2500 = (N_F_RL_1( lerp(node_2687,(node_2687+node_4858),_RimLightInLight) , node_2687 )*N_F_SON_1( lerp( 1.0, node_5206, _ShowNormal ) )*_LightColor0.rgb);
                half node_573 = 0.5*dot(lerp( N_F_CLD( lerp(lightDirection,lerp( _CustomLightDirection.rgb, mul( unity_ObjectToWorld, float4(_CustomLightDirection.rgb,0) ).xyz.rgb, _CustomLightDirectionFollowObjectRotation ),_CustomLightDirectionIntensity) , lightDirection ), viewDirection, _SelfShadowShadowTAtViewDirection ),node_3134)+0.5;
                half node_4111 = 1.0;
                half node_9545 = smoothstep( lerp(0.3,1.0,_SelfShadowHardness), 1.0, (node_573*lerp(7,lerp( _SelfShadowThreshold, (_SelfShadowThreshold*(1.0 - i.vertexColor.g)), _VertexColorGreenControlSelfShadowSize ),_SelfShadowThreshold)) );
                half3 node_2388 = (node_3924*lerp((lerp(node_1860,(node_1860+node_6694),_LightAffectShadow)*node_938*N_F_STIS( node_8749 )),N_F_ST( lerp(((node_338+node_6694)*node_938),node_2500,lerp(node_4736,smoothstep( node_6045, node_3187, ((_ShadowT_var.r*(1.0 - _ShadowTShadowThreshold))*(lerp( node_573, (attenuation*node_573), _LightFalloffAffectShadowT )*_ShadowTLightThreshold)) ),_ShadowTIntensity)) , node_2500 ),N_F_SS( lerp(node_4111,(lerp( node_9545, lerp(node_9545,node_4111,C_SS()), _SelfShadowAffectedByLightShadowStrength )*attenuation),_SelfShadowIntensity) , lerp(node_4111,attenuation,_SelfShadowIntensity) )));
                float3 node_9694 = Ref( viewReflectDirection , _ReflectionRoughtness );
                half2 node_8431 = reflect(viewDirection,normalDirection).rg;
                half2 node_4207 = (float2(node_8431.r,(-1*node_8431.g))*0.5+0.5);
                half4 _FReflection_var = tex2Dlod(_FReflection,float4(TRANSFORM_TEX(node_4207, _FReflection),0.0,_ReflectionRoughtness));
                half4 _MaskReflection_var = tex2D(_MaskReflection,TRANSFORM_TEX(i.uv0, _MaskReflection));
                half3 node_2704 = N_F_R( lerp(node_2388,lerp(node_2388,N_F_FR( lerp( node_9694, _FReflection_var.rgb, _UseFReflection ) , node_9694 ),_MaskReflection_var.r),_ReflectionIntensity) , node_2388 );
                half3 node_7395 = N_F_SL_1( lerp(node_2704,lerp(node_2704,node_3924,_MaskSelfLit_var.r),_SelfLitIntensity) , node_2704 );
                half3 node_1225 = N_F_RL( lerp((node_7395+node_4858),node_7395,_RimLightInLight) , node_7395 );
                float node_7145 = 0.0;
                half3 node_9768 = ((node_3924*N_F_SL( lerp(node_1243,lerp(node_1243,((_SelfLitColor.rgb*node_3924*lerp( 1.0, node_3924, _SelfLitHighContrast ))*_SelfLitPower),_MaskSelfLit_var.r),_SelfLitIntensity) , node_1243 )*N_F_STIAL( lerp(lerp(float3(node_932,node_932,node_932),node_8749,_ShowInAmbientLightIntensity),float3(node_208,node_208,node_208),clamp((node_938*8.0),0,1)) ))+N_F_D( (node_1225+lerp((_DepthColor.rgb*_DepthColorPower),float3(node_7145,node_7145,node_7145),smoothstep( clamp(_DepthEdgeHardness,_DepthEdgeHardness,(1.0-0.001)), 1.0, saturate((sceneZ-partZ)/_Depth) ))) , node_1225 ));
                float3 finalColor = N_F_CA( lerp(node_9768,dot(node_9768,float3(0.3,0.59,0.11)),(1.0 - _Saturation)) , node_9768 );
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
			Cull [_DoubleSided]
            BlendOp Max
            ZWrite [_ZWrite]
            
			Stencil {
            	Ref[_RefVal]
            	Comp [_Compa]
            	Pass [_Oper]
            	Fail [_Oper]
            }
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog

			#pragma multi_compile_instancing

            #pragma only_renderers d3d11 glcore gles3 metal xboxone ps4 wiiu switch
            #pragma target 4.6

			#pragma shader_feature N_F_CA_ON
			#pragma shader_feature N_F_SL_ON
			#pragma shader_feature N_F_GLO_ON
			#pragma shader_feature N_F_GLOT_ON
			#pragma shader_feature N_F_SS_ON
			#pragma shader_feature N_F_SCT_ON
			#pragma shader_feature N_F_ST_ON
			#pragma shader_feature N_F_STIS_ON
			#pragma shader_feature N_F_STIAL_ON 
			#pragma shader_feature N_F_SON_ON
			#pragma shader_feature N_F_PT_ON
			#pragma shader_feature N_F_CLD_ON
			#pragma shader_feature N_F_R_ON
			#pragma shader_feature N_F_FR_ON
			#pragma shader_feature N_F_RL_ON
			#pragma shader_feature N_F_D_ON

            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform half _SelfShadowHardness;
            uniform half _SelfShadowThreshold;
            uniform half4 _HighlightColor;
            uniform half4 _SelfShadowColor;
            uniform half _SelfShadowColorPower;
            uniform half _HighlightColorPower;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform half4 _MainColor;
            uniform sampler2D _ShadowColorTexture; uniform float4 _ShadowColorTexture_ST;
            uniform half _ShadowColorTexturePower;
            uniform sampler2D _ShadowT; uniform float4 _ShadowT_ST;
            uniform half _ShadowTShadowThreshold;
            uniform fixed _LightFalloffAffectShadowT;
            uniform half _ShadowTLightThreshold;
            uniform half _ShadowTHardness;
            uniform half4 _ShadowTColor;
            uniform half _ShadowTColorPower;
            uniform half4 _OverallShadowColor;
            uniform half _OverallShadowColorPower;
            uniform half _Glossiness;
            uniform half _GlossSoftness;
            uniform half4 _GlossColor;
            uniform half _GlossColorPower;
            uniform sampler2D _GlossTexture; uniform float4 _GlossTexture_ST;
            uniform half _GlossTextureFollowLight;
            uniform fixed _GlossTextureFollowObjectRotation;
            uniform sampler2D _MaskGloss; uniform float4 _MaskGloss_ST;
            uniform sampler2D _FReflection; uniform float4 _FReflection_ST;
            uniform half _ReflectionIntensity;
            uniform sampler2D _MaskReflection; uniform float4 _MaskReflection_ST;
            uniform half _GlossTextureSoftness;
            uniform half4 _SelfLitColor;
            uniform fixed _SelfLitHighContrast;
            uniform half _SelfLitPower;
            uniform half _SelfLitIntensity;
            uniform sampler2D _MaskSelfLit; uniform float4 _MaskSelfLit_ST;
            uniform sampler2D _PTexture; uniform float4 _PTexture_ST;
            uniform half _PTexturePower;
            half3 AL_GI( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            uniform fixed _RimLightInLight;
            uniform half _RimLightUnfill;
            uniform half4 _RimLightColor;
            uniform half _RimLightColorPower;
            uniform half _RimLightSoftness;
            half N_F_SS( half A , half B ){
            #ifdef N_F_SS_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SCT( half3 A ){
            #ifdef N_F_SCT_ON
            
            return A;
            
            #else
            
            return 1;
            
            #endif
            }
            
            half N_F_PT( half A ){
            #ifdef N_F_PT_ON
            
            return A;
            
            #else
            
            return 1;
            
            #endif
            }
            
            half3 N_F_ST( half3 A , half3 B ){
            #ifdef N_F_ST_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_GLO( half3 A , half3 B ){
            #ifdef N_F_GLO_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_GLOT( half A , half B ){
            #ifdef N_F_GLOT_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_R( half3 A , half3 B ){
            #ifdef N_F_R_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SL( half3 A , half3 B ){
            #ifdef N_F_SL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_RL( half3 A , half3 B ){
            #ifdef N_F_RL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SL_1( half3 A , half3 B ){
            #ifdef N_F_SL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_RL_1( half3 A , half3 B ){
            #ifdef N_F_RL_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            uniform fixed _SelfShadowShadowTAtViewDirection;
            uniform half _XYZHardness;
            uniform float4 _XYZPosition;
            uniform half _SmoothObjectNormal;
            uniform fixed _ShowNormal;
            half3 N_F_SON( half3 A , half3 B ){
            #ifdef N_F_SON_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_SON_1( half3 A ){
            #ifdef N_F_SON_ON
            
            return A;
            
            #else
            
            return 1;
            
            #endif
            }
            
            uniform half4 _CustomLightDirection;
            uniform fixed _CustomLightDirectionFollowObjectRotation;
            uniform half _CustomLightDirectionIntensity;
            half3 N_F_CLD( half3 A , half3 B ){
            #ifdef N_F_CLD_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            uniform fixed _VertexColorGreenControlSelfShadowSize;
            uniform fixed _VertexColorBlueControlSmoothObjectNormal;
            uniform fixed _LightAffectRimLightColor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform half _NormalMapIntensity;
            uniform half _ReduceTextureQuality;
            uniform fixed _UseFReflection;
            float3 Ref( half3 VR , half Mip ){
            float4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, VR, Mip);
            return DecodeHDR (skyData, unity_SpecCube0_HDR);
            }
            
            uniform half _ReflectionRoughtness;
            uniform fixed _GIFlatShade;
            uniform half _GIShadeThreshold;
            uniform half _GlossTextureRotate;
            uniform half _LightIntensity;
            uniform fixed _LightAffectShadow;
            uniform fixed _TexturePatternStyle;
            uniform half _TextureIntesnity;
            uniform half _RefractionIntensity;
            uniform fixed _MainColorAffectTexture;
            uniform half _Depth;
            uniform half _DepthEdgeHardness;
            uniform half4 _DepthColor;
            uniform half _DepthColorPower;
            half3 N_F_D( half3 A , half3 B ){
            #ifdef N_F_D_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_FR( half3 A , half3 B ){
            #ifdef N_F_FR_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            half3 N_F_STIAL( half3 A ){
            #ifdef N_F_ST_ON
            
            	#ifdef N_F_STIAL_ON
            
            	return A;
            
            	#else
            
            	return 1;
            
            	#endif
            
            #else
            
            return 1;
            
            #endif
            }
            
            uniform half _ShowInAmbientLightIntensity;
            half3 N_F_STIS( half3 A ){
            #ifdef N_F_ST_ON
            
            	#ifdef N_F_STIS_ON
            
            	return A;
            
            	#else
            
            	return 1;
            
            	#endif
            
            #else
            
            return 1;
            
            #endif
            }
            
            uniform half _ShadowTIntensity;
            uniform half _ShowInAmbientLightShadowThreshold;
            uniform half _EnvironmentalLightingIntensity;
            half3 N_F_CA( half3 A , half3 B ){
            #ifdef N_F_CA_ON
            
            return A;
            
            #else
            
            return B;
            
            #endif
            }
            
            uniform half _Saturation;
            uniform half _SelfShadowIntensity;
            uniform fixed _SelfShadowAffectedByLightShadowStrength;
            fixed C_SS(){
            return _LightShadowData.x;
            }
            
            uniform half _GlossIntensity;
            uniform half _TessellationNear;
            uniform half _TessellationFar;
            uniform half _TessellationTransition;

			uniform float _TessellationSmoothness;
			uniform half _PointSpotlightIntensity;
			uniform half _LightFalloffSoftness;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;

				UNITY_SETUP_INSTANCE_ID (v);

                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float4 vertexColor : COLOR;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.vertexColor = v.vertexColor;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return lerp(_TessellationNear,_TessellationFar,saturate((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)*(1.0 - _TessellationTransition))));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;

					float3 pp[3];
					for (int i = 0; i < 3; ++i)
					pp[i] = v.vertex.xyz - vi[i].normal * (dot(v.vertex.xyz, vi[i].normal) - dot(vi[i].vertex.xyz, vi[i].normal));
					v.vertex.xyz = _TessellationSmoothness * (pp[0] * bary.x + pp[1] * bary.y + pp[2] * bary.z) + (1.0f - _TessellationSmoothness) * v.vertex.xyz;

                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.vertexColor = vi[0].vertexColor*bary.x + vi[1].vertexColor*bary.y + vi[2].vertexColor*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                half3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = lerp(half3(0,0,1),_NormalMap_var.rgb,_NormalMapIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);

                fixed lightfo = 0;
				#ifdef POINT
				unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(i.posWorld.xyz, 1)).xyz; 
				lightfo = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).r;
				#else
				lightfo;
				#endif
				#ifdef POINT_COOKIE
				#if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
				#define DLCOO(input, worldPos) unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1)).xyz
				#else
				#define DLCOO(input, worldPos) unityShadowCoord3 lightCoord = i._LightCoord
				#endif
				DLCOO(i, i.posWorld.xyz);
				lightfo = tex2D(_LightTextureB0, dot(lightCoord, lightCoord).rr).r * texCUBE(_LightTexture0, lightCoord).w;
				#else
				lightfo;
				#endif
				#ifdef DIRECTIONAL
				lightfo = UNITY_SHADOW_ATTENUATION(i, i.posWorld.xyz);
				#endif
				#ifdef DIRECTIONAL_COOKIE
				#if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
				#define DLCOO(input, worldPos) unityShadowCoord2 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1)).xy
				#else
				#define DLCOO(input, worldPos) unityShadowCoord2 lightCoord = input._LightCoord
				#endif
				DLCOO(i, i.posWorld.xyz);
				lightfo = tex2D(_LightTexture0, lightCoord).w;
				#endif
				#ifdef SPOT
				#if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
				#define DLCOO(input, worldPos) unityShadowCoord4 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1))
				#else
				#define DLCOO(input, worldPos) unityShadowCoord4 lightCoord = input._LightCoord
				#endif
				DLCOO(i, i.posWorld.xyz);
				lightfo = (lightCoord.z > 0) * tex2D(_LightTexture0, lightCoord.xy / lightCoord.w + 0.5).w * tex2D(_LightTextureB0, dot(lightCoord, lightCoord).xx).r;
				#else
				lightfo;
				#endif
				fixed attenuation = 1; 
				fixed lightfos = smoothstep(0, _LightFalloffSoftness ,lightfo);

                half3 node_7067 = (tex2D( _GrabTexture, lerp(sceneUVs.rg,reflect(float3(sceneUVs.rg,0.0),normalDirection).rg,lerp(0.0,(-0.2),_RefractionIntensity))).rgb*_MainColor.rgb);
                half2 node_9146 = (float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg*distance(objPos.rgb,_WorldSpaceCameraPos));
                half2 _TexturePatternStyle_var = lerp( i.uv0, node_9146, _TexturePatternStyle );
                half4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(_TexturePatternStyle_var, _MainTex),0.0,_ReduceTextureQuality));
                half3 node_3924 = lerp(node_7067,lerp(node_7067,lerp( _MainTex_var.rgb, (_MainTex_var.rgb*_MainColor.rgb), _MainColorAffectTexture ),_MainTex_var.a),_TextureIntesnity);
                float node_3622 = 0.0;
				float node_4432 = 1.0;
                float3 node_76 = mul( unity_WorldToObject, float4((i.posWorld.rgb-objPos.rgb),0) ).xyz.rgb.rgb;
                half3 node_5206 = lerp(normalDirection,mul( unity_ObjectToWorld, float4(float3((_XYZPosition.r+(_XYZHardness*node_76.r)),(_XYZPosition.g+(_XYZHardness*node_76.g)),(_XYZPosition.b+(_XYZHardness*node_76.b))),0) ).xyz.rgb,lerp( _SmoothObjectNormal, (_SmoothObjectNormal*(1.0 - i.vertexColor.b)), _VertexColorBlueControlSmoothObjectNormal ));
                half3 node_3134 = N_F_SON( node_5206 , normalDirection );
                float node_2183 = 0.2;
                float node_8383 = 0.3;
                half3 node_1243 = (AL_GI( lerp(float3(node_3622,node_3622,node_3622),float3(node_4432,node_4432,node_4432),lerp( (node_3134*_GIShadeThreshold), float3(smoothstep( float2(node_2183,node_2183), float2(node_8383,node_8383), (node_3134.rb*_GIShadeThreshold) ),0.0), _GIFlatShade )) )*_EnvironmentalLightingIntensity);
                half4 _MaskSelfLit_var = tex2D(_MaskSelfLit,TRANSFORM_TEX(i.uv0, _MaskSelfLit));
                float node_932 = 1.0;
                half node_4736 = 1.0;
                half4 _ShadowColorTexture_var = tex2D(_ShadowColorTexture,TRANSFORM_TEX(i.uv0, _ShadowColorTexture));
                half3 node_9600 = N_F_SCT( lerp(_ShadowColorTexture_var.rgb,(_ShadowColorTexture_var.rgb*_ShadowColorTexture_var.rgb),_ShadowColorTexturePower) );
                half4 _PTexture_var = tex2D(_PTexture,TRANSFORM_TEX(node_9146, _PTexture));
                half node_8910 = N_F_PT( lerp((1.0 - _PTexturePower),1.0,_PTexture_var.r) );
                half3 node_7102 = (_OverallShadowColor.rgb*_OverallShadowColorPower);
                half3 node_338 = ((_ShadowTColor.rgb*_ShadowTColorPower)*node_9600*node_8910*node_7102);
                float node_7419 = 1.0;
                half node_6045 = lerp(0.0,0.22,_ShadowTHardness);
                half node_3187 = 0.22;
                half4 _ShadowT_var = tex2D(_ShadowT,TRANSFORM_TEX(i.uv0, _ShadowT));
                half3 node_8749 = lerp(float3(node_4736,node_4736,node_4736),lerp(node_338,float3(node_7419,node_7419,node_7419),smoothstep( node_6045, node_3187, (_ShowInAmbientLightShadowThreshold*_ShadowT_var.r) )),_ShadowTIntensity);
                float node_208 = 1.0;
                float4 node_6613_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_6613_p = lerp(float4(float4(_LightColor0.rgb,0.0).zy, node_6613_k.wz), float4(float4(_LightColor0.rgb,0.0).yz, node_6613_k.xy), step(float4(_LightColor0.rgb,0.0).z, float4(_LightColor0.rgb,0.0).y));
                float4 node_6613_q = lerp(float4(node_6613_p.xyw, float4(_LightColor0.rgb,0.0).x), float4(float4(_LightColor0.rgb,0.0).x, node_6613_p.yzx), step(node_6613_p.x, float4(_LightColor0.rgb,0.0).x));
                float node_6613_d = node_6613_q.x - min(node_6613_q.w, node_6613_q.y);
                float node_6613_e = 1.0e-10;
                half3 node_6613 = float3(abs(node_6613_q.z + (node_6613_q.w - node_6613_q.y) / (6.0 * node_6613_d + node_6613_e)), node_6613_d / (node_6613_q.x + node_6613_e), node_6613_q.x);;
                half node_938 = saturate(node_6613.b);
                half3 node_1860 = ((_SelfShadowColor.rgb*_SelfShadowColorPower)*node_7102*node_9600*node_8910);
                half3 node_6694 = (_LightIntensity+_LightColor0.rgb);
                half3 node_2779 = (_HighlightColor.rgb*_HighlightColorPower+_PointSpotlightIntensity);
                float node_9345 = 0.0;
                float node_5992_ang = _GlossTextureRotate;
                float node_5992_spd = 1.0;
                float node_5992_cos = cos(node_5992_spd*node_5992_ang);
                float node_5992_sin = sin(node_5992_spd*node_5992_ang);
                float2 node_5992_piv = float2(0.5,0.5);
                half3 node_2832 = reflect(lerp(viewDirection,halfDirection,_GlossTextureFollowLight),normalDirection);
                half2 node_9280 = lerp( node_2832, mul( unity_WorldToObject, float4(node_2832,0) ).xyz.rgb, _GlossTextureFollowObjectRotation ).rg;
                half2 node_5992 = (mul(float2((-1*node_9280.r),node_9280.g)-node_5992_piv,float2x2( node_5992_cos, -node_5992_sin, node_5992_sin, node_5992_cos))+node_5992_piv);
                half2 node_8759 = (node_5992*0.5+0.5);
                half4 _GlossTexture_var = tex2Dlod(_GlossTexture,float4(TRANSFORM_TEX(node_8759, _GlossTexture),0.0,_GlossTextureSoftness));
                half4 _MaskGloss_var = tex2D(_MaskGloss,TRANSFORM_TEX(i.uv0, _MaskGloss));
                half3 node_2687 = N_F_GLO( lerp(node_2779,lerp(node_2779,(_GlossColor.rgb*_GlossColorPower),lerp(float3(node_9345,node_9345,node_9345),N_F_GLOT( _GlossTexture_var.r , smoothstep( 0.1, lerp(0.1,1.0,_GlossSoftness), pow(max(0,dot(halfDirection,normalDirection)),exp2(lerp(-2,15,_Glossiness))) ) ),_GlossIntensity)),_MaskGloss_var.r) , node_2779 );
                float node_4353 = 0.0;
                float node_3687 = 0.0;
                half3 node_4858 = lerp(float3(node_4353,node_4353,node_4353),(lerp( _RimLightColor.rgb, lerp(float3(node_3687,node_3687,node_3687),_RimLightColor.rgb,_LightColor0.rgb), _LightAffectRimLightColor )*_RimLightColorPower),smoothstep( 1.7, lerp(1.7,0.29,_RimLightSoftness), pow(1.0-max(0,dot(normalDirection, viewDirection)),(1.0 - _RimLightUnfill)) ));
                half3 node_2500 = (N_F_RL_1( lerp(node_2687,(node_2687+node_4858),_RimLightInLight) , node_2687 )*N_F_SON_1( lerp( 1.0, node_5206, _ShowNormal ) )*_LightColor0.rgb);
                half node_573 = 0.5*dot(lerp( N_F_CLD( lerp(lightDirection,lerp( _CustomLightDirection.rgb, mul( unity_ObjectToWorld, float4(_CustomLightDirection.rgb,0) ).xyz.rgb, _CustomLightDirectionFollowObjectRotation ),_CustomLightDirectionIntensity) , lightDirection ), viewDirection, _SelfShadowShadowTAtViewDirection ),node_3134)+0.5;
                half node_4111 = 1.0;
                half node_9545 = smoothstep( lerp(0.3,1.0,_SelfShadowHardness), 1.0, (node_573*lerp(7,lerp( _SelfShadowThreshold, (_SelfShadowThreshold*(1.0 - i.vertexColor.g)), _VertexColorGreenControlSelfShadowSize ),_SelfShadowThreshold)) );
                half3 node_2388 = (node_3924*lerp((lerp(node_1860,(node_1860+node_6694),_LightAffectShadow)*node_938*N_F_STIS( node_8749 )),N_F_ST( lerp(((node_338+node_6694)*node_938),node_2500,lerp(node_4736,smoothstep( node_6045, node_3187, ((_ShadowT_var.r*(1.0 - _ShadowTShadowThreshold))*(lerp( node_573, (lightfos*node_573), _LightFalloffAffectShadowT )*_ShadowTLightThreshold)) ),_ShadowTIntensity)) , node_2500 ),N_F_SS( lerp(node_4111,(lerp( node_9545, lerp(node_9545,node_4111,C_SS()), _SelfShadowAffectedByLightShadowStrength )*attenuation),_SelfShadowIntensity) , lerp(node_4111,attenuation,_SelfShadowIntensity) ))*lightfos);
                float3 node_9694 = Ref( viewReflectDirection , _ReflectionRoughtness );
                half2 node_8431 = reflect(viewDirection,normalDirection).rg;
                half2 node_4207 = (float2(node_8431.r,(-1*node_8431.g))*0.5+0.5);
                half4 _FReflection_var = tex2Dlod(_FReflection,float4(TRANSFORM_TEX(node_4207, _FReflection),0.0,_ReflectionRoughtness));
                half4 _MaskReflection_var = tex2D(_MaskReflection,TRANSFORM_TEX(i.uv0, _MaskReflection));
                half3 node_2704 = N_F_R( lerp(node_2388,lerp(node_2388,N_F_FR( lerp( node_9694, _FReflection_var.rgb, _UseFReflection ) , node_9694 ),_MaskReflection_var.r)*lightfos,_ReflectionIntensity) , node_2388 );
                half3 node_7395 = N_F_SL_1( lerp(node_2704,lerp(node_2704,node_3924,_MaskSelfLit_var.r),_SelfLitIntensity) , node_2704 );
                half3 node_1225 = N_F_RL( lerp((node_7395+node_4858)*lightfos,node_7395,_RimLightInLight) , node_7395 );
                float node_7145 = 0.0;
                half3 node_9768 = ((node_3924*N_F_SL( lerp(node_1243,lerp(node_1243,((_SelfLitColor.rgb*node_3924*lerp( 1.0, node_3924, _SelfLitHighContrast ))*_SelfLitPower),_MaskSelfLit_var.r),_SelfLitIntensity) , node_1243 )*N_F_STIAL( lerp(lerp(float3(node_932,node_932,node_932),node_8749,_ShowInAmbientLightIntensity),float3(node_208,node_208,node_208),clamp((node_938*8.0),0,1)) ))+N_F_D( (node_1225+lerp((_DepthColor.rgb*_DepthColorPower),float3(node_7145,node_7145,node_7145),smoothstep( clamp(_DepthEdgeHardness,_DepthEdgeHardness,(1.0-0.001)), 1.0, saturate((sceneZ-partZ)/_Depth) ))) , node_1225 ));
                float3 finalColor = N_F_CA( lerp(node_9768,dot(node_9768,float3(0.3,0.59,0.11)),(1.0 - _Saturation)) , node_9768 );
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
		CustomEditor "RealToonShaderGUI"
}