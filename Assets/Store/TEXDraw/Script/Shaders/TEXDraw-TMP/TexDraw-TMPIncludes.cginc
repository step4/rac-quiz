
#include "UnityCG.cginc"

// Common APPDATA for most TEXDraw needs
struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float4 tangent : TANGENT;
	half4 color : COLOR;
};

// Common V2F for most TEXDraw needs
struct v2f
{
	half2 uv : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float3 uv2 : TEXCOORD2;
	float4 vertex : SV_POSITION;
	half4 color : COLOR;
};

// Common Vertex program for most TEXDraw needs
v2f vert (appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	o.uv1 = v.uv1;
	half2 tanXY = v.tangent.xy;
	// XY is raw data (compatibility), but Z now contains
	// translated index from tangent XY (make it cheap).
	o.uv2 = half3(tanXY, determineIndex(tanXY));
	o.color = v.color;
	return o;
}

fixed4 mix (fixed4 vert, fixed4 tex)
{
   /*
	*	The reason of why using max:
	* 	Font textures is alpha-only, means it's RGB will be black
	* 	With colors from col, the output color would be the same as i.color 
	*	But this comes problem for sprites: it's RGB value will be overwritten by col.
	*	So, every use of Non-alpha-only sprites must have set the i.color down to black,
	*	which automatically handled by Charbox.cs
	*/
	return fixed4(max(vert, tex).rgb, vert.a * tex.a);
}
#ifdef TEX_4_1

sampler2D _Font0, _Font1, _Font2, _Font3, _Font4, _Font5, _Font6, _Font7;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 0)			alpha = tex2D(_Font0, uv);
	else if(index == 1)		alpha = tex2D(_Font1, uv);
	else if(index == 2)		alpha = tex2D(_Font2, uv);
	else if(index == 3)		alpha = tex2D(_Font3, uv);
	else if(index == 4)		alpha = tex2D(_Font4, uv);
	else if(index == 5)		alpha = tex2D(_Font5, uv);
	else if(index == 6)		alpha = tex2D(_Font6, uv);
	else if(index == 7)		alpha = tex2D(_Font7, uv);
	else if(index == 31)	alpha = fixed4(0, 0, 0, 1);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}
#endif
#ifdef TEX_4_2
sampler2D _Font8, _Font9, _FontA, _FontB, _FontC, _FontD, _FontE, _FontF;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 8)			alpha = tex2D(_Font8, uv);
	else if(index == 9)		alpha = tex2D(_Font9, uv);
	else if(index == 10)	alpha = tex2D(_FontA, uv);
	else if(index == 11)	alpha = tex2D(_FontB, uv);
	else if(index == 12)	alpha = tex2D(_FontC, uv);
	else if(index == 13)	alpha = tex2D(_FontD, uv);
	else if(index == 14)	alpha = tex2D(_FontE, uv);
	else if(index == 15)	alpha = tex2D(_FontF, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}
#endif

#ifdef TEX_4_3
sampler2D _Font10, _Font11, _Font12, _Font13, _Font14, _Font15, _Font16, _Font17;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 16)			alpha = tex2D(_Font10, uv);
	else if(index == 17)	alpha = tex2D(_Font11, uv);
	else if(index == 18)	alpha = tex2D(_Font12, uv);
	else if(index == 19)	alpha = tex2D(_Font13, uv);
	else if(index == 20)	alpha = tex2D(_Font14, uv);
	else if(index == 21)	alpha = tex2D(_Font15, uv);
	else if(index == 22)	alpha = tex2D(_Font16, uv);
	else if(index == 23)	alpha = tex2D(_Font17, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}
#endif

#ifdef TEX_4_4
sampler2D _Font18, _Font19, _Font1A, _Font1B, _Font1C, _Font1D, _Font1E;


fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 24)			alpha = tex2D(_Font18, uv);
	else if(index == 25)	alpha = tex2D(_Font19, uv);
	else if(index == 26)	alpha = tex2D(_Font1A, uv);
	else if(index == 27)	alpha = tex2D(_Font1B, uv);
	else if(index == 28)	alpha = tex2D(_Font1C, uv);
	else if(index == 29)	alpha = tex2D(_Font1D, uv);
	else if(index == 30)	alpha = tex2D(_Font1E, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}
#endif
#ifdef TEX_5_1

sampler2D _Font0, _Font1, _Font2, _Font3, _Font4, _Font5;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 0)			alpha = tex2D(_Font0, uv);
	else if(index == 1)		alpha = tex2D(_Font1, uv);
	else if(index == 2)		alpha = tex2D(_Font2, uv);
	else if(index == 3)		alpha = tex2D(_Font3, uv);
	else if(index == 4)		alpha = tex2D(_Font4, uv);
	else if(index == 5)		alpha = tex2D(_Font5, uv);
	else if(index == 31)	alpha = fixed4(0, 0, 0, 1);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}

#endif
#ifdef TEX_5_2
sampler2D _Font6, _Font7, _Font8, _Font9, _FontA, _FontB;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 6)			alpha = tex2D(_Font6, uv);
	else if(index == 7)		alpha = tex2D(_Font7, uv);
	else if(index == 8)		alpha = tex2D(_Font8, uv);
	else if(index == 9)		alpha = tex2D(_Font9, uv);
	else if(index == 10)	alpha = tex2D(_FontA, uv);
	else if(index == 11)	alpha = tex2D(_FontB, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}

#endif

#ifdef TEX_5_3

sampler2D _FontC, _FontD, _FontE, _FontF, _Font10, _Font11;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 12)			alpha = tex2D(_FontC, uv);
	else if(index == 13)	alpha = tex2D(_FontD, uv);
	else if(index == 14)	alpha = tex2D(_FontE, uv);
	else if(index == 15)	alpha = tex2D(_FontF, uv);
	else if(index == 16)	alpha = tex2D(_Font10, uv);
	else if(index == 17)	alpha = tex2D(_Font11, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}

#endif
#ifdef TEX_5_4
sampler2D _Font12, _Font13, _Font14, _Font15, _Font16, _Font17;

fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 18)			alpha = tex2D(_Font12, uv);
	else if(index == 19)	alpha = tex2D(_Font13, uv);
	else if(index == 20)	alpha = tex2D(_Font14, uv);
	else if(index == 21)	alpha = tex2D(_Font15, uv);
	else if(index == 22)	alpha = tex2D(_Font16, uv);
	else if(index == 23)	alpha = tex2D(_Font17, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}

#endif

#ifdef TEX_5_5
sampler2D _Font18, _Font19, _Font1A, _Font1B, _Font1C, _Font1D, _Font1E;


fixed4 getTexPoint(half2 uv, half index)
{
	fixed4 alpha;
	if(index == 24)			alpha = tex2D(_Font18, uv);
	else if(index == 25)	alpha = tex2D(_Font19, uv);
	else if(index == 26)	alpha = tex2D(_Font1A, uv);
	else if(index == 27)	alpha = tex2D(_Font1B, uv);
	else if(index == 28)	alpha = tex2D(_Font1C, uv);
	else if(index == 29)	alpha = tex2D(_Font1D, uv);
	else if(index == 30)	alpha = tex2D(_Font1E, uv);
	else					alpha = fixed4(0, 0, 0, 0);
	return alpha;
}

#endif
// ------------------- MODIFIED TMPRO.cginc snippet for solving compatibility issues within TEXDraw ----------------


float4 GetHeightField(float2 uv, float3 delta, half index)
{
	// Read "height field"
  float4 h = {getTexPoint(uv - delta.xz, index).a,
				getTexPoint(uv + delta.xz, index).a,
				getTexPoint(uv - delta.zy, index).a,
				getTexPoint(uv + delta.zy, index).a};

	//return GetSurfaceNormal(h, bias);
	return h;
}

// ----------------------FOR STANDARD SDF Shader -------------------
#ifdef SDF_STANDARD
fixed4 PixShaderSDFInternal(pixel_t input)
{
		float4 heightF = half4(0,0,0,0);
		half UVAlpha = 0;
		half fontIndex = input.indexcoords.x;
#if BEVEL_ON
		float3 dxy = float3(0.5 / _TextureWidth, 0.5 / _TextureHeight, 0);
		heightF = GetHeightField (input.texcoords.xy, dxy, fontIndex);
#endif
#if UNDERLAY_ON || UNDERLAY_INNER
		UVAlpha = getTexPoint(input.texcoord2.xy,fontIndex).a;
#endif
		fixed4 c = getTexPoint(input.texcoords,fontIndex);
		return PixShaderSubset(input, c, heightF, UVAlpha);
}
#endif

// ----------------------FOR Mobile SDF Shader -------------------
#ifdef SDF_MOBILE
fixed4 PixShaderSDFMobileInternal(pixel_t input)
{
		half fontIndex = input.indexcoords.x;
		fixed4 c = getTexPoint(input.texcoord0,fontIndex);
#if UNDERLAY_ON || UNDERLAY_INNER
		fixed4 c2 = getTexPoint(input.texcoord1,fontIndex);
#else
		fixed4 c2 = fixed4(0,0,0,0);
#endif
		return PixShaderSubset(input, c, c2);
}
#endif