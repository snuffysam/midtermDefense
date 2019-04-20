Shader "Custom Toon Lit" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "grey" {} 
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp

sampler2D _Ramp;
float4 _Color;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif

	half d = (dot (s.Normal, lightDir)*0.5 + 0.5) * atten;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	half4 col = _Color;
	if (d < 0.2){
		half r = col[0];
		half g = col[1];
		half b = col[2];
		if (r > g){
			ramp = half3((r*2.0)/3.0, g/3.0, b/3.0);
		} else {
			ramp = half3(r/3.0, g/3.0, (b*2.0)/3.0);
		}
	} else if (d < 0.6){
		half r = col[0];
		half g = col[1];
		half b = col[2];
		if (r > g){
			ramp = half3((r*1.0)/1.2, (g*2.0)/3.0, (b*2.0)/3.0);
		} else {
			ramp = half3((r*2.0)/3.0, (g*2.0)/3.0, (b*1.0)/1.2);
		}
	} else {
		ramp = col.rgb;
	}
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
	c.rgb = ramp * _LightColor0.rgb;
	c.a = 0;
	return c;
}


sampler2D _MainTex;
//float4 _Color;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
};

void surf (Input IN, inout SurfaceOutput o) {
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb * 0;
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
}
