// Copyright (C) 2014 - 2016 Stephan Schaem - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// Simplified SDF shader:
// - No Shading Option (bevel / bump / env map)
// - No Glow Option
// - Softness is applied on both side of the outline

// Derivation of original Distance Field with some modification for TEXDraw compatibility

Shader "TEXDraw/TextMeshPro/Distance Field (Mobile)" {

Properties {
    	[MiniThumbTexture] _Font0("Font 0", 2D) = "white" {}
		[MiniThumbTexture] _Font1("Font 1", 2D) = "white" {}
		[MiniThumbTexture] _Font2("Font 2", 2D) = "white" {}
		[MiniThumbTexture] _Font3("Font 3", 2D) = "white" {}
		[MiniThumbTexture] _Font4("Font 4", 2D) = "white" {}
		[MiniThumbTexture] _Font5("Font 5", 2D) = "white" {}
		[MiniThumbTexture] _Font6("Font 6", 2D) = "white" {}
		[MiniThumbTexture] _Font7("Font 7", 2D) = "white" {}
		[Space]
		[MiniThumbTexture] _Font8("Font 8", 2D) = "white" {}
		[MiniThumbTexture] _Font9("Font 9", 2D) = "white" {}
		[MiniThumbTexture] _FontA("Font A", 2D) = "white" {}
		[MiniThumbTexture] _FontB("Font B", 2D) = "white" {}
		[MiniThumbTexture] _FontC("Font C", 2D) = "white" {}
		[MiniThumbTexture] _FontD("Font D", 2D) = "white" {}
		[MiniThumbTexture] _FontE("Font E", 2D) = "white" {}
		[MiniThumbTexture] _FontF("Font F", 2D) = "white" {}
		[Space]
		[MiniThumbTexture] _Font10("Font 10", 2D) = "white" {}
		[MiniThumbTexture] _Font11("Font 11", 2D) = "white" {}
		[MiniThumbTexture] _Font12("Font 12", 2D) = "white" {}
		[MiniThumbTexture] _Font13("Font 13", 2D) = "white" {}
		[MiniThumbTexture] _Font14("Font 14", 2D) = "white" {}
		[MiniThumbTexture] _Font15("Font 15", 2D) = "white" {}
		[MiniThumbTexture] _Font16("Font 16", 2D) = "white" {}
		[MiniThumbTexture] _Font17("Font 17", 2D) = "white" {}
		[Space]
		[MiniThumbTexture] _Font18("Font 18", 2D) = "white" {}
		[MiniThumbTexture] _Font19("Font 19", 2D) = "white" {}
		[MiniThumbTexture] _Font1A("Font 1A", 2D) = "white" {}
		[MiniThumbTexture] _Font1B("Font 1B", 2D) = "white" {}
		[MiniThumbTexture] _Font1C("Font 1C", 2D) = "white" {}
		[MiniThumbTexture] _Font1D("Font 1D", 2D) = "white" {}
		[MiniThumbTexture] _Font1E("Font 1E", 2D) = "white" {}
		[Space] 
	_FaceColor			("Face Color", Color) = (1,1,1,1)
	_FaceDilate			("Face Dilate", Range(-1,1)) = 0

	_OutlineColor		("Outline Color", Color) = (0,0,0,1)
	_OutlineWidth		("Outline Thickness", Range(0,1)) = 0
	_OutlineSoftness	("Outline Softness", Range(0,1)) = 0

	_UnderlayColor		("Border Color", Color) = (0,0,0,.5)
	_UnderlayOffsetX 	("Border OffsetX", Range(-1,1)) = 0
	_UnderlayOffsetY 	("Border OffsetY", Range(-1,1)) = 0
	_UnderlayDilate		("Border Dilate", Range(-1,1)) = 0
	_UnderlaySoftness 	("Border Softness", Range(0,1)) = 0

	_WeightNormal		("Weight Normal", float) = 0
	_WeightBold			("Weight Bold", float) = .5

	_ShaderFlags		("Flags", float) = 0
	_ScaleRatioA		("Scale RatioA", float) = 1
	_ScaleRatioB		("Scale RatioB", float) = 1
	_ScaleRatioC		("Scale RatioC", float) = 1

	_MainTex			("Font Atlas", 2D) = "white" {}
	_TextureWidth		("Texture Width", float) = 512
	_TextureHeight		("Texture Height", float) = 512
	_GradientScale		("Gradient Scale", float) = 5
	_ScaleX				("Scale X", float) = 1
	_ScaleY				("Scale Y", float) = 1
	_PerspectiveFilter	("Perspective Correction", Range(0, 1)) = 0.875

	_VertexOffsetX		("Vertex OffsetX", float) = 0
	_VertexOffsetY		("Vertex OffsetY", float) = 0

	_ClipRect			("Clip Rect", vector) = (-10000, -10000, 10000, 10000)
	_MaskSoftnessX		("Mask SoftnessX", float) = 0
	_MaskSoftnessY		("Mask SoftnessY", float) = 0
	
	_StencilComp		("Stencil Comparison", Float) = 8
	_Stencil			("Stencil ID", Float) = 0
	_StencilOp			("Stencil Operation", Float) = 0
	_StencilWriteMask	("Stencil Write Mask", Float) = 255
	_StencilReadMask	("Stencil Read Mask", Float) = 255
	
	_ColorMask			("Color Mask", Float) = 15
}

SubShader {
	Tags 
	{
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
		"TexMaterialType"="Standard"
	}


	Stencil
	{
		Ref [_Stencil]
		Comp [_StencilComp]
		Pass [_StencilOp] 
		ReadMask [_StencilReadMask]
		WriteMask [_StencilWriteMask]
	}

	Cull [_CullMode]
	ZWrite Off
	Lighting Off
	Fog { Mode Off }
	ZTest [unity_GUIZTestMode]
	Blend One OneMinusSrcAlpha
	ColorMask [_ColorMask]

		CGINCLUDE
		#pragma shader_feature __ OUTLINE_ON
		#pragma shader_feature __ UNDERLAY_ON UNDERLAY_INNER
		#define SDF_MOBILE
		#include "UnityCG.cginc"
		#include "UnityUI.cginc"
		#include "TMPro_Properties.cginc"

		struct vertex_t {
			float4	vertex			: POSITION;
			float3	normal			: NORMAL;
			fixed4	color			: COLOR;
			float2	texcoord0		: TEXCOORD0;
			float2	texcoord1		: TEXCOORD1;
			float2	texcoord2		: TEXCOORD2;
		};

		struct pixel_t {
			float4	vertex			: SV_POSITION;
			fixed4	faceColor		: COLOR;
			fixed4	outlineColor	: COLOR1;
			float4	texcoord0		: TEXCOORD0;			// Texture UV, Mask UV
			half3	indexcoords		: TEXCOORD1;			// Texture UV, Mask UV
			half4	param			: TEXCOORD2;			// Scale(x), BiasIn(y), BiasOut(z), Bias(w)
			half4	mask			: TEXCOORD3;			// Position in clip space(xy), Softness(zw)
		#if (UNDERLAY_ON | UNDERLAY_INNER)
			float4	texcoord1		: TEXCOORD4;			// Texture UV, alpha, reserved
			half2	underlayParam	: TEXCOORD5;			// Scale(x), Bias(y)
		#endif
		};


		half determineIndex(half2 uv1)
		{
			half x, y;
			x = floor(uv1.x*8+0.5h);
			y = floor(uv1.y*4+0.5h);
			return (y * 8) + x;
		}

		pixel_t VertShader(vertex_t input)
		{
			float bold = step(input.texcoord1.y, 0);

			float4 vert = input.vertex;
			vert.x += _VertexOffsetX;
			vert.y += _VertexOffsetY;
			float4 vPosition = UnityObjectToClipPos(vert);

			float2 pixelSize = vPosition.w;
			pixelSize /= float2(_ScaleX, _ScaleY) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
			
			float scale = rsqrt(dot(pixelSize, pixelSize));
			scale *= abs(input.texcoord1.y) * _GradientScale * 1.5;
			if(UNITY_MATRIX_P[3][3] == 0) scale = lerp(abs(scale) * (1 - _PerspectiveFilter), scale, abs(dot(UnityObjectToWorldNormal(input.normal.xyz), normalize(WorldSpaceViewDir(vert)))));

			float weight = lerp(_WeightNormal, _WeightBold, bold) / _GradientScale;
			weight += _FaceDilate * _ScaleRatioA * 0.5;

			float layerScale = scale;

			scale /= 1 + (_OutlineSoftness * _ScaleRatioA * scale);
			float bias = (0.5 - weight) * scale - 0.5;
			float outline = _OutlineWidth * _ScaleRatioA * 0.5 * scale;

			float opacity = input.color.a;
		#if (UNDERLAY_ON | UNDERLAY_INNER)
				opacity = 1.0;
		#endif

			fixed4 faceColor = fixed4(input.color.rgb, opacity) * _FaceColor;
			faceColor.rgb *= faceColor.a;

			fixed4 outlineColor = _OutlineColor;
			outlineColor.a *= opacity;
			outlineColor.rgb *= outlineColor.a;
			outlineColor = lerp(faceColor, outlineColor, sqrt(min(1.0, (outline * 2))));

		#if (UNDERLAY_ON | UNDERLAY_INNER)

			layerScale /= 1 + ((_UnderlaySoftness * _ScaleRatioC) * layerScale);
			float layerBias = (.5 - weight) * layerScale - .5 - ((_UnderlayDilate * _ScaleRatioC) * .5 * layerScale);

			float x = -(_UnderlayOffsetX * _ScaleRatioC) * _GradientScale / _TextureWidth;
			float y = -(_UnderlayOffsetY * _ScaleRatioC) * _GradientScale / _TextureHeight;
			float2 layerOffset = float2(x, y);
		#endif

			// Generate UV for the Masking Texture
			float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
			float2 maskUV = (vert.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);

			// Structure for pixel shader
			pixel_t output = {
				vPosition,
				faceColor,
				outlineColor,
				float4(input.texcoord0.x, input.texcoord0.y, maskUV.x, maskUV.y),
                float3(input.texcoord1.x,0,0),
				half4(scale, bias - outline, bias + outline, bias),
				half4(vert.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_MaskSoftnessX, _MaskSoftnessY) + pixelSize.xy)),
			#if (UNDERLAY_ON | UNDERLAY_INNER)
				float4(input.texcoord0 + layerOffset, input.color.a, 0),
				half2(layerScale, layerBias),
			#endif
			};

			return output;
		}


		// PIXEL SHADER
		fixed4 PixShaderSubset(pixel_t input, fixed4 color, fixed4 color2) 
		{
			half d = color.a * input.param.x;
			half4 c = input.faceColor * saturate(d - input.param.w);

		#ifdef OUTLINE_ON
			c = lerp(input.outlineColor, input.faceColor, saturate(d - input.param.z));
			c *= saturate(d - input.param.y);
		#endif

		#if UNDERLAY_ON
			d = color2.a * input.underlayParam.x;
			c += float4(_UnderlayColor.rgb * _UnderlayColor.a, _UnderlayColor.a) * saturate(d - input.underlayParam.y) * (1 - c.a);
		#endif

		#if UNDERLAY_INNER
			half sd = saturate(d - input.param.z);
			d = color2.a * input.underlayParam.x;
			c += float4(_UnderlayColor.rgb * _UnderlayColor.a, _UnderlayColor.a) * (1 - saturate(d - input.underlayParam.y)) * sd * (1 - c.a);
		#endif

		#if UNITY_VERSION < 530
			// Unity 5.2 2D Rect Mask Support
			if (_UseClipRect)
			{
				half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
				c *= m.x * m.y;
			}
		#else
			// Alternative implementation to UnityGet2DClipping with support for softness.
			half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
			c *= m.x * m.y;
		#endif

		#if (UNDERLAY_ON | UNDERLAY_INNER)
			c *= input.texcoord1.z;
		#endif

			return c;
		}
		ENDCG
        		
        Pass
        {
            Name "SecondaryPass"
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment PixShader
            #define TEX_4_2
            #include "TexDraw-TMPIncludes.cginc"
                        
            fixed4 PixShader(pixel_t input) : SV_Target
            { return PixShaderSDFMobileInternal(input); }

            ENDCG
        }

        Pass
        {
            Name "ThirdPass"
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment PixShader
            #define TEX_4_3
            #include "TexDraw-TMPIncludes.cginc"
                        
            fixed4 PixShader(pixel_t input) : SV_Target
            { return PixShaderSDFMobileInternal(input); }

            ENDCG
        }
        
        Pass
        {
            Name "FourthPass"
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment PixShader
            #define TEX_4_4
            #include "TexDraw-TMPIncludes.cginc"
                        
            fixed4 PixShader(pixel_t input) : SV_Target
            { return PixShaderSDFMobileInternal(input); }

            ENDCG
        }
        
        Pass
        {
            Name "FirstPass"
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment PixShader
            #define TEX_4_1
            #include "TexDraw-TMPIncludes.cginc"
                        
            fixed4 PixShader(pixel_t input) : SV_Target
            { return PixShaderSDFMobileInternal(input); }

            ENDCG
        }

	}
    CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}
