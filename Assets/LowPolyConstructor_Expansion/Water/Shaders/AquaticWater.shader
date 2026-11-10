Shader "LowPolyConstructor/AquaticWater" {
 Properties { _BaseColor("Water Color",Color)=(.05,.3,.38,.80) _WaveHeight("Wave Height",Range(0,.3))=.045 _WaveScale("Wave Scale",Range(.02,1))=.16 _WaveSpeed("Wave Speed",Range(0,3))=.55 _FoamColor("Foam Color",Color)=(.8,.9,.87,1) _FoamStrength("Foam",Range(0,1))=.1 }
 SubShader { Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
 Pass { Name "Water" Tags { "LightMode"="UniversalForward" } Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off
 HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #pragma multi_compile_instancing
 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 CBUFFER_START(UnityPerMaterial)
 half4 _BaseColor;float _WaveHeight;float _WaveScale;float _WaveSpeed;half4 _FoamColor;half _FoamStrength;
 CBUFFER_END
 struct Attributes {float4 positionOS:POSITION;UNITY_VERTEX_INPUT_INSTANCE_ID};
 struct Varyings {float4 positionCS:SV_POSITION;float3 world:TEXCOORD0;};
 float hash21(float2 p){return frac(sin(dot(p,float2(127.1,311.7)))*43758.5453);}
 float noise2(float2 p){float2 f=frac(p),i=floor(p);f=f*f*(3.-2.*f);return lerp(lerp(hash21(i),hash21(i+float2(1,0)),f.x),lerp(hash21(i+float2(0,1)),hash21(i+float2(1,1)),f.x),f.y);}
 Varyings vert(Attributes i){Varyings o;UNITY_SETUP_INSTANCE_ID(i);float3 w=TransformObjectToWorld(i.positionOS.xyz);float t=_Time.y*_WaveSpeed;float a=sin((w.x+w.z*.37)*_WaveScale+t);float b=sin((w.z-w.x*.23)*(_WaveScale*1.6)-t*.7);w.y+=_WaveHeight*(a+.35*b);o.positionCS=TransformWorldToHClip(w);o.world=w;return o;}
 half4 frag(Varyings i):SV_Target {float2 p=i.world.xz;float t=_Time.y*_WaveSpeed;float pa=(p.x+p.y*.37)*_WaveScale+t;float pb=(p.y-p.x*.23)*(_WaveScale*1.6)-t*.7;float a=sin(pa);float2 grad=_WaveHeight*_WaveScale*(cos(pa)*float2(1,.37)+.56*cos(pb)*float2(-.23,1));float3 normal=normalize(float3(-grad.x*8,1,-grad.y*8));float3 view=normalize(_WorldSpaceCameraPos-i.world);Light light=GetMainLight();float fresnel=pow(1-saturate(abs(dot(normal,view))),3);float spec=pow(saturate(dot(normal,normalize(view+light.direction))),28);float waveLight=saturate(dot(normal,light.direction));float n=noise2(p*.32+float2(t*.04,-t*.05));float foam=smoothstep(.91,.995,a)*smoothstep(.70,.88,n)*_FoamStrength*.34;half3 col=_BaseColor.rgb*(.73+.12*n+.045*a+.42*waveLight)+half3(.18,.25,.24)*fresnel*.22+light.color*spec*.32;return half4(lerp(col,_FoamColor.rgb,foam),_BaseColor.a);}
 ENDHLSL
 } } }
