﻿Shader "SupGames/Mobile/Fxaa"
{
	Properties{
		_MainTex("-", 2D) = "white" {}
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform half _Threshold;
	uniform half _Sharpness;
	half4 _MainTex_TexelSize;

	#ifdef UNITY_COLORSPACE_GAMMA
	#define lum half3(0.22h, 0.707h, 0.071h)
	#else 
	#define lum half3(0.0396819152h, 0.45802179h, 0.00609653955h)
	#endif

	struct appdata {
		half4 vertex : POSITION;
		half2 uv : TEXCOORD0;
	};

	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half4 uv1 : TEXCOORD1;
		half4 uv2 : TEXCOORD2;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		half2 offset = (_MainTex_TexelSize.xy) * 0.5h;
		o.uv1 = half4(v.uv - offset, v.uv + offset);
		o.uv2 = half4(offset, offset * 4.0h);
		return o;
	}

	half4 frag(v2f i) : SV_Target
	{
		half3 col = tex2D(_MainTex, i.uv).rgb;
		half gr = dot(col, lum);
		half gtl = dot(tex2D(_MainTex, i.uv1.xy).rgb, lum);
		half gbl = dot(tex2D(_MainTex, i.uv1.xw).rgb, lum);
		half gtr = dot(tex2D(_MainTex, i.uv1.zy).rgb, lum) + 0.0026041667h;
		half gbr = dot(tex2D(_MainTex, i.uv1.zw).rgb, lum);

		half gmax = max(max(gtr, gbr), max(gtl, gbl));
		half gmin = min(min(gtr, gbr), min(gtl, gbl));

		if (max(gmax, gr) - min(gmin, gr) < max(0.0h, gmax * _Threshold))
			return half4(col, 1.0h);

		half diff1 = gbl - gtr;
		half diff2 = gbr - gtl;

		half2 mltp = normalize(half2(diff1 + diff2, diff1 - diff2));
		half dvd = min(abs(mltp.x), abs(mltp.y)) * _Sharpness;

		half3 tmp1 = tex2D(_MainTex, i.uv - mltp * i.uv2.xy).rgb;
		half3 tmp2 = tex2D(_MainTex, i.uv + mltp * i.uv2.xy).rgb;

		mltp = clamp(mltp.xy / dvd, -2.0h, 2.0h);

		half3 tmp3 = tex2D(_MainTex, i.uv - mltp * i.uv2.zw).rgb;
		half3 tmp4 = tex2D(_MainTex, i.uv + mltp * i.uv2.zw).rgb;

		half3 col1 = tmp1 + tmp2;
		half3 col2 = ((tmp3 + tmp4) * 0.25h) + (col1 * 0.25h);

		if (dot(col1, lum) < gmin || dot(col2, lum) > gmax)
			return half4(col1 * 0.5h, 1.0h);
		else
			return half4(col2, 1.0h);
	}
	ENDCG
	
	SubShader{
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }      
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
	}
	FallBack Off
}