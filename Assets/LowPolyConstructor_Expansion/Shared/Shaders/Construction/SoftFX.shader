Shader "LowPolyConstructorExpansion/SoftFX"
{
 Properties { _BaseMap("Mask",2D)="white"{} _BaseColor("Color",Color)=(1,1,1,1) _AlphaScale("Alpha scale",Float)=1 _EmissionStrength("Brightness",Float)=1 _UseTextureColor("Texture color",Float)=0 }
 SubShader {
  Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
  Pass {
   Tags { "LightMode"="SRPDefaultUnlit" }
   Blend SrcAlpha OneMinusSrcAlpha
   ZWrite Off
   Cull Off
   HLSLPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
   TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
   CBUFFER_START(UnityPerMaterial)
   float4 _BaseMap_ST; half4 _BaseColor; half _AlphaScale,_EmissionStrength,_UseTextureColor;
   CBUFFER_END
   struct A { float4 positionOS:POSITION; float2 uv:TEXCOORD0; };
   struct V { float4 positionCS:SV_POSITION; float2 uv:TEXCOORD0; };
   V vert(A v) { V o;o.positionCS=TransformObjectToHClip(v.positionOS.xyz);o.uv=TRANSFORM_TEX(v.uv,_BaseMap);return o; }
   half4 frag(V i):SV_Target { half4 t=SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,i.uv); return half4(_BaseColor.rgb*lerp(half3(1,1,1),t.rgb,_UseTextureColor)*_EmissionStrength,t.a*saturate(_BaseColor.a*_AlphaScale)); }
   ENDHLSL
  }
 }
}
