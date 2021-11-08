 
Shader"Custom/ChromaKey" {
 
Properties {
MainTex ("Base (RGB)", 2D) = "white" {}
thresh ("Threshold", Range (0, 16)) = 0.65
slope ("Slope", Range (0, 1)) = 0.63
keyingColor ("KeyColour", Color) = (1,1,1,1)
}
 
SubShader {
Tags {"Queue"="Transparent""IgnoreProjector"="True""RenderType"="Transparent"}
LOD 100
 
Lighting Off
ZWrite Off
AlphaTest Off
Blend SrcAlpha OneMinusSrcAlpha
 
Pass {
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
 
sampler2D MainTex;
float3 keyingColor;
float thresh; //0.8
float slope; //0.2
 
#include "UnityCG.cginc"
 
float4 frag(v2f_img i) : COLOR{
float3 input_color = tex2D(MainTex,i.uv).rgb;
float d = abs(length(abs(keyingColor.rgb - input_color.rgb)));
float edge0 = thresh*(1 - slope);
float alpha = smoothstep(edge0,thresh,d);
return float4(input_color,alpha);
 
 
}
 
ENDCG
}
}
 
FallBack"Unlit/Texture"
}
 