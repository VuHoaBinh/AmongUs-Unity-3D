// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX_Kandol_Pack/Flipbook_Effects"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[Header(ParticlesSystem)]_Opacity_RGB("Opacity_RGB", Range( 0 , 1)) = 0
		_Opacity_Alpha("Opacity_Alpha", Range( 0 , 1)) = 1
		[Header(FLIPBOOK UV ANIM)]_Fllipbook_Emissive("Fllipbook_Emissive", Range( 0 , 10)) = 1
		_Fllipbook_Opacity("Fllipbook_Opacity", Range( 0 , 1)) = 1
		[IntRange]_Fllipbook_Columns("Fllipbook_Columns", Range( 1 , 8)) = 0
		[IntRange]_Fllipbook_Rows("Fllipbook_Rows", Range( 1 , 8)) = 0
		[IntRange]_Fllipbook_speed("Fllipbook_speed", Range( 0 , 20)) = 1
		[Header(MASK_SELECTS)][KeywordEnum(Gradetion,Sphere)] _MASK_SELECTS("MASK_SELECTS", Float) = 0
		[Header(MASK_Gradetion_Sphere)]_Mask_Gradetion_PawerA("Mask_Gradetion_PawerA", Range( 0 , 5)) = 3.95
		_Mask_Gradetion_PawerB("Mask_Gradetion_PawerB", Range( 0.1 , 0.5)) = 3.95
		_Mask_SphereSize("Mask_Sphere Size", Range( 0 , 0.5)) = 0.2
		_Mask_SphereCenter("Mask_Sphere Center ", Range( 0 , 0.09)) = 0
		[NoScaleOffset][Header(NOISE)]_Noise("Noise", 2D) = "black" {}
		_Gradetion_NoisexyTilingyzOffset("Gradetion_Noise: xyTiling / yz Offset", Vector) = (1,1,0,0)
		_Gradetion_Noise_Speed("Gradetion_Noise_Speed", Range( 0 , 5)) = 1
		_Gradetion_Noise_Opacity("Gradetion_Noise_Opacity", Range( 0 , 1)) = 3.12906
		_Sphere_NoiseRotate_Speed("Sphere_NoiseRotate_Speed", Range( 0.1 , 0.5)) = 0.5
		_Sphere_Noise_Opacity("Sphere_Noise_Opacity", Range( 0 , 0.1)) = 3.12906
		[Header(Twister)][IntRange]_Sphere_Twister_Size("Sphere_Twister_Size", Range( 0 , 10)) = 1.457462
		_Sphere_Twister_Speed("Sphere_Twister_Speed", Range( 0 , 0.3)) = 0
		_Sphere_Twister_Opacity("Sphere_Twister_Opacity", Range( 0 , 0.2)) = 0

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend One One
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR
				#pragma shader_feature_local _MASK_SELECTS_GRADETION _MASK_SELECTS_SPHERE


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform sampler2D _Noise;
				uniform float _Gradetion_Noise_Speed;
				uniform float4 _Gradetion_NoisexyTilingyzOffset;
				uniform float _Gradetion_Noise_Opacity;
				uniform float _Mask_SphereCenter;
				uniform float _Mask_SphereSize;
				uniform float _Sphere_Twister_Speed;
				uniform float _Sphere_Twister_Size;
				uniform float _Sphere_Twister_Opacity;
				uniform float _Sphere_NoiseRotate_Speed;
				uniform float _Sphere_Noise_Opacity;
				uniform float _Fllipbook_Columns;
				uniform float _Fllipbook_Rows;
				uniform float _Fllipbook_speed;
				uniform float _Fllipbook_Opacity;
				uniform float _Mask_Gradetion_PawerB;
				uniform float _Mask_Gradetion_PawerA;
				uniform float _Fllipbook_Emissive;
				uniform float _Opacity_RGB;
				uniform float _Opacity_Alpha;
				float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
				float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
				float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
				float snoise( float2 v )
				{
					const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
					float2 i = floor( v + dot( v, C.yy ) );
					float2 x0 = v - i + dot( i, C.xx );
					float2 i1;
					i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
					float4 x12 = x0.xyxy + C.xxzz;
					x12.xy -= i1;
					i = mod2D289( i );
					float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
					float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
					m = m * m;
					m = m * m;
					float3 x = 2.0 * frac( p * C.www ) - 1.0;
					float3 h = abs( x ) - 0.5;
					float3 ox = floor( x + 0.5 );
					float3 a0 = x - ox;
					m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
					float3 g;
					g.x = a0.x * x0.x + h.x * x0.y;
					g.yz = a0.yz * x12.xz + h.yz * x12.yw;
					return 130.0 * dot( m, g );
				}
				


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float mulTime150 = _Time.y * _Gradetion_Noise_Speed;
					float2 appendResult146 = (float2(_Gradetion_NoisexyTilingyzOffset.x , _Gradetion_NoisexyTilingyzOffset.y));
					float2 appendResult340 = (float2(_Gradetion_NoisexyTilingyzOffset.z , _Gradetion_NoisexyTilingyzOffset.w));
					float2 texCoord144 = i.texcoord.xy * appendResult146 + appendResult340;
					float2 panner148 = ( mulTime150 * float2( 0.01,-0.1 ) + texCoord144);
					float2 texCoord339 = i.texcoord.xy * ( appendResult146 * float2( 0.5,0.5 ) ) + appendResult340;
					float2 panner337 = ( mulTime150 * float2( -0.01,-0.025 ) + texCoord339);
					float NoiseUP538 = ( ( tex2D( _Noise, panner148 ).r + tex2D( _Noise, panner337 ).r ) * _Gradetion_Noise_Opacity );
					float smoothstepResult461 = smoothstep( 0.1 , _Mask_SphereCenter , pow( length( ( ( i.texcoord.xy - float2( 0.5,0.5 ) ) * _Mask_SphereSize ) ) , 1.0 ));
					float Mask_Sphere462 = smoothstepResult461;
					float2 texCoord467 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 CenteredUV15_g35 = ( texCoord467 - float2( 0.5,0.5 ) );
					float2 break17_g35 = CenteredUV15_g35;
					float2 appendResult23_g35 = (float2(( length( CenteredUV15_g35 ) * 0.3 * 2.0 ) , ( atan2( break17_g35.x , break17_g35.y ) * ( 1.0 / 6.28318548202515 ) * _Sphere_Twister_Speed )));
					float mulTime465 = _Time.y * 0.3;
					float simplePerlin2D474 = snoise( ( appendResult23_g35 + -( _Sphere_Twister_Speed * mulTime465 ) )*_Sphere_Twister_Size );
					simplePerlin2D474 = simplePerlin2D474*0.5 + 0.5;
					float SphereMaskTwister473 = ( ( Mask_Sphere462 * simplePerlin2D474 ) * _Sphere_Twister_Opacity );
					float2 temp_cast_0 = (0.5).xx;
					float mulTime502 = _Time.y * _Sphere_NoiseRotate_Speed;
					float cos499 = cos( mulTime502 );
					float sin499 = sin( mulTime502 );
					float2 rotator499 = mul( i.texcoord.xy - temp_cast_0 , float2x2( cos499 , -sin499 , sin499 , cos499 )) + temp_cast_0;
					float2 temp_cast_1 = (0.5).xx;
					float cos503 = cos( ( mulTime502 * -0.5 ) );
					float sin503 = sin( ( mulTime502 * -0.5 ) );
					float2 rotator503 = mul( i.texcoord.xy - temp_cast_1 , float2x2( cos503 , -sin503 , sin503 , cos503 )) + temp_cast_1;
					float NoiseRotate345 = ( ( tex2D( _Noise, rotator499 ).r + tex2D( _Noise, rotator503 ).r ) * _Sphere_Noise_Opacity );
					#if defined(_MASK_SELECTS_GRADETION)
					float staticSwitch544 = NoiseUP538;
					#elif defined(_MASK_SELECTS_SPHERE)
					float staticSwitch544 = ( SphereMaskTwister473 + NoiseRotate345 );
					#else
					float staticSwitch544 = NoiseUP538;
					#endif
					float2 appendResult444 = (float2(1.0 , 1.0));
					float mulTime45 = _Time.y * 3.0;
					// *** BEGIN Flipbook UV Animation vars ***
					// Total tiles of Flipbook Texture
					float fbtotaltiles34 = _Fllipbook_Columns * _Fllipbook_Rows;
					// Offsets for cols and rows of Flipbook Texture
					float fbcolsoffset34 = 1.0f / _Fllipbook_Columns;
					float fbrowsoffset34 = 1.0f / _Fllipbook_Rows;
					// Speed of animation
					float fbspeed34 = mulTime45 * _Fllipbook_speed;
					// UV Tiling (col and row offset)
					float2 fbtiling34 = float2(fbcolsoffset34, fbrowsoffset34);
					// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
					// Calculate current tile linear index
					float fbcurrenttileindex34 = round( fmod( fbspeed34 + 1.0, fbtotaltiles34) );
					fbcurrenttileindex34 += ( fbcurrenttileindex34 < 0) ? fbtotaltiles34 : 0;
					// Obtain Offset X coordinate from current tile linear index
					float fblinearindextox34 = round ( fmod ( fbcurrenttileindex34, _Fllipbook_Columns ) );
					// Multiply Offset X by coloffset
					float fboffsetx34 = fblinearindextox34 * fbcolsoffset34;
					// Obtain Offset Y coordinate from current tile linear index
					float fblinearindextoy34 = round( fmod( ( fbcurrenttileindex34 - fblinearindextox34 ) / _Fllipbook_Columns, _Fllipbook_Rows ) );
					// Reverse Y to get tiles from Top to Bottom
					fblinearindextoy34 = (int)(_Fllipbook_Rows-1) - fblinearindextoy34;
					// Multiply Offset Y by rowoffset
					float fboffsety34 = fblinearindextoy34 * fbrowsoffset34;
					// UV Offset
					float2 fboffset34 = float2(fboffsetx34, fboffsety34);
					// Flipbook UV
					half2 fbuv34 = ( ( i.texcoord.xy + staticSwitch544 ) * appendResult444 ) * fbtiling34 + fboffset34;
					// *** END Flipbook UV Animation vars ***
					float4 tex2DNode35 = tex2D( _MainTex, fbuv34 );
					float smoothstepResult528 = smoothstep( 0.0 , _Mask_Gradetion_PawerB , pow( ( 1.0 - (i.texcoord.xy).y ) , _Mask_Gradetion_PawerA ));
					float Mask_Gradetion484 = smoothstepResult528;
					#if defined(_MASK_SELECTS_GRADETION)
					float staticSwitch543 = Mask_Gradetion484;
					#elif defined(_MASK_SELECTS_SPHERE)
					float staticSwitch543 = Mask_Sphere462;
					#else
					float staticSwitch543 = Mask_Gradetion484;
					#endif
					float SwitchMask_Selects531 = staticSwitch543;
					float4 appendResult548 = (float4(( ( ( _TintColor * ( ( ( ( tex2DNode35 * tex2DNode35.a ) * _Fllipbook_Opacity ) * SwitchMask_Selects531 ) * _Fllipbook_Emissive ) ) * i.color ) * _Opacity_RGB ).rgb , ( ( i.color.a * SwitchMask_Selects531 ) * _Opacity_Alpha )));
					

					fixed4 col = appendResult548;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
323;119;1398;927;345.5832;319.4316;1.989975;True;True
Node;AmplifyShaderEditor.CommentaryNode;487;-2249.891,1268.624;Inherit;False;1270.241;836.5273;Comment;2;483;453;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;453;-2197.391,1671.794;Inherit;False;1170.241;412.6575;Sphere;9;462;461;460;459;458;457;456;455;454;Sphere;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;454;-2178.013,1715.836;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;456;-2091.307,1876.433;Inherit;False;Property;_Mask_SphereSize;Mask_Sphere Size;10;0;Create;True;1;MASK_Sphere;0;0;False;0;False;0.2;0.2;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;455;-1994.378,1714.692;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;343;-5234.443,998.9627;Inherit;False;2302.534;1258.601;Comment;30;337;342;148;505;500;502;144;335;499;142;150;135;339;336;340;145;503;149;345;141;146;506;501;521;522;523;538;539;541;545;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;463;-4726.505,364.0493;Inherit;False;1774.672;611.6138;Comment;14;362;473;467;472;470;469;474;372;468;464;465;471;475;466;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;457;-1845.275,1727.098;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;500;-5034.03,1379.807;Inherit;False;Property;_Sphere_NoiseRotate_Speed;Sphere_NoiseRotate_Speed;16;0;Create;True;0;0;0;False;0;False;0.5;0.1;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;458;-1708.348,1719.445;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;464;-4703.062,586.1083;Inherit;False;Property;_Sphere_Twister_Speed;Sphere_Twister_Speed;19;0;Create;True;0;0;0;False;0;False;0;0;0;0.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;145;-5130.406,1728.844;Inherit;False;Property;_Gradetion_NoisexyTilingyzOffset;Gradetion_Noise: xyTiling / yz Offset;13;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;465;-4607.059,668.1786;Inherit;False;1;0;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;146;-4848.822,1728.709;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;502;-4762.03,1378.808;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;466;-4415.06,652.1787;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;460;-1579.036,1731.205;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;467;-4655.062,428.1791;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;459;-1729.578,1981.896;Inherit;False;Property;_Mask_SphereCenter;Mask_Sphere Center ;11;0;Create;True;0;0;0;False;0;False;0;0;0;0.09;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;342;-4681.185,1801.707;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;469;-4415.06,428.1791;Inherit;True;Polar Coordinates;-1;;35;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;0.3;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;541;-4786.104,1245.499;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;501;-4535.43,1428.353;Inherit;False;Constant;_Float15;Float 15;32;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-4851.575,1979.91;Inherit;False;Property;_Gradetion_Noise_Speed;Gradetion_Noise_Speed;14;0;Create;True;0;0;0;False;0;False;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;468;-4255.059,652.1787;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;461;-1363.456,1913.935;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;505;-4490.944,1509.159;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;340;-4840.559,1834.08;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;150;-4488.473,1998.534;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;339;-4523.128,1855.013;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;144;-4546.091,1672.516;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;471;-4092.973,517.9006;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;335;-4319.132,1118.719;Inherit;True;Property;_Noise;Noise;12;1;[NoScaleOffset];Create;True;0;0;0;False;1;Header(NOISE);False;f439bf46c90ab6b49bd5efa93c4f8585;f439bf46c90ab6b49bd5efa93c4f8585;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RotatorNode;499;-4273.104,1315.148;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;503;-4257.482,1466.84;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;470;-4376.511,745.4766;Inherit;False;Property;_Sphere_Twister_Size;Sphere_Twister_Size;18;2;[Header];[IntRange];Create;True;1;Twister;0;0;False;0;False;1.457462;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;462;-1212.918,1853.068;Inherit;True;Mask_Sphere;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;337;-4224.46,1815.605;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.01,-0.025;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;475;-3916.519,418.9077;Inherit;True;462;Mask_Sphere;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;474;-3966.387,631.1725;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;135;-4017.583,1242.589;Inherit;True;Property;_Noise000;Noise000;10;0;Create;True;0;0;0;False;0;False;-1;f439bf46c90ab6b49bd5efa93c4f8585;f6f35492f85b46e4eb93927dc72219f9;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;336;-4011.684,1435.232;Inherit;True;Property;_TextureSample1;Texture Sample 1;10;0;Create;True;0;0;0;False;0;False;-1;f439bf46c90ab6b49bd5efa93c4f8585;f6f35492f85b46e4eb93927dc72219f9;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;148;-4234.001,1676.751;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,-0.1;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;522;-4000.987,1850.964;Inherit;True;Property;_TextureSample2;Texture Sample 2;10;0;Create;True;0;0;0;False;0;False;-1;f439bf46c90ab6b49bd5efa93c4f8585;f6f35492f85b46e4eb93927dc72219f9;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;142;-3686.864,1533.565;Inherit;False;Property;_Sphere_Noise_Opacity;Sphere_Noise_Opacity;17;0;Create;True;0;0;0;False;0;False;3.12906;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;472;-3677.338,553.3862;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;521;-4022.137,1644.831;Inherit;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;0;False;0;False;-1;f439bf46c90ab6b49bd5efa93c4f8585;f6f35492f85b46e4eb93927dc72219f9;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;506;-3609.27,1372.094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;372;-3910.408,869.661;Inherit;False;Property;_Sphere_Twister_Opacity;Sphere_Twister_Opacity;20;0;Create;True;0;0;0;False;0;False;0;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;523;-3633.156,1702.196;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;545;-3679.585,1615.287;Inherit;False;Property;_Gradetion_Noise_Opacity;Gradetion_Noise_Opacity;15;0;Create;True;0;0;0;False;0;False;3.12906;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;362;-3538.567,615.3917;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-3386.167,1346.064;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;483;-2199.338,1318.624;Inherit;False;1201.183;336.0878;mask;8;484;441;440;435;437;438;528;529;mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;345;-3164.986,1344.603;Inherit;True;NoiseRotate;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;435;-2149.338,1370.353;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;539;-3386.875,1733.409;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;170;-2886.162,458.6926;Inherit;False;3174.956;770.7452;Comment;29;540;363;536;365;442;439;41;34;443;28;39;35;444;532;445;27;63;482;45;488;52;61;40;29;481;544;546;554;555;Flipbook;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;473;-3266.486,748.1628;Inherit;True;SphereMaskTwister;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;438;-1951.686,1368.624;Inherit;False;False;True;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;363;-2828.377,741.045;Inherit;True;345;NoiseRotate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;536;-2832.851,517.2025;Inherit;True;473;SphereMaskTwister;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;538;-3147.703,1719.262;Inherit;True;NoiseUP;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;540;-2821.018,969.9614;Inherit;True;538;NoiseUP;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;441;-1919.337,1495.908;Inherit;False;Property;_Mask_Gradetion_PawerA;Mask_Gradetion_PawerA;8;1;[Header];Create;True;1;MASK_Gradetion_Sphere;0;0;False;0;False;3.95;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;437;-1760.934,1373.966;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;546;-2564.516,616.1141;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;544;-2282.861,835.0928;Inherit;False;Property;_MASK_SELECTS;MASK_SELECTS;7;0;Create;True;0;0;0;False;1;Header(MASK_SELECTS);False;0;0;0;True;;KeywordEnum;2;Gradetion;Sphere;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;529;-1918.056,1569.09;Inherit;False;Property;_Mask_Gradetion_PawerB;Mask_Gradetion_PawerB;9;0;Create;True;0;0;0;False;0;False;3.95;0.1;0.1;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;443;-2002.147,1002.208;Inherit;False;Constant;_Float7;Float 7;24;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;440;-1613.301,1383.432;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;41;-2192.48,535.7269;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;445;-2006.856,1092.748;Inherit;False;Constant;_Float9;Float 9;24;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;365;-1904.109,623.7501;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;488;-1560.178,1061.904;Inherit;False;Constant;_Float8;Float 8;30;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;444;-1824.312,1046.484;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;528;-1375.056,1521.09;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;482;-1591.044,654.7532;Inherit;False;Property;_Fllipbook_Columns;Fllipbook_Columns;4;1;[IntRange];Create;True;0;0;0;False;0;False;0;4;1;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;481;-1580.482,739.2491;Inherit;False;Property;_Fllipbook_Rows;Fllipbook_Rows;5;1;[IntRange];Create;True;0;0;0;False;0;False;0;4;1;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;530;-953.1083,1516.336;Inherit;False;940.8605;379.0031;Cylinder / Ground;4;531;485;486;543;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;45;-1472.871,975.2073;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1527.286,887.9026;Inherit;False;Constant;_Fllipbook_startFrame;Fllipbook_start Frame;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1580.676,813.7302;Inherit;False;Property;_Fllipbook_speed;Fllipbook_speed;6;1;[IntRange];Create;True;0;0;0;False;0;False;1;10;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;484;-1216.629,1390.758;Inherit;True;Mask_Gradetion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;442;-1596.755,552.2983;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;486;-712.2794,1668.339;Inherit;True;462;Mask_Sphere;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;485;-903.1083,1566.336;Inherit;True;484;Mask_Gradetion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;554;-1233.484,458.6559;Inherit;True;0;0;_MainTex;Shader;True;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;34;-1258.457,666.4656;Inherit;True;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;35;-977.1475,583.9812;Inherit;True;Property;_00;00;6;1;[NoScaleOffset];Create;True;0;0;0;False;1;Header(FLIPBOOK);False;-1;576edb8b3b86da1489d9a2c93e0408d6;e67e1ad3d9f2c144e9ba4a1bd1d88aa5;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;543;-476.2415,1582.613;Inherit;False;Property;_MASK_SELECTS;MASK_SELECTS;9;0;Create;True;0;0;0;False;1;Header(MASK_SELECTS);False;0;0;0;True;;KeywordEnum;2;Gradetion;Sphere;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-938.8551,860.8615;Inherit;False;Property;_Fllipbook_Opacity;Fllipbook_Opacity;3;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-666.564,655.9149;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;531;-245.0962,1573.65;Inherit;True;SwitchMask_Selects;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-514.8416,726.7235;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;532;-707.631,1012.13;Inherit;True;531;SwitchMask_Selects;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-428.6162,1142.841;Inherit;False;Property;_Fllipbook_Emissive;Fllipbook_Emissive;2;1;[Header];Create;True;1;FLIPBOOK UV ANIM;0;0;False;0;False;1;3;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;439;-291.6901,896.6202;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-135.5585,857.8883;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;555;-240.8378,655.8815;Inherit;False;0;0;_TintColor;Shader;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;87.1586,767.5056;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;533;323.0906,1177.737;Inherit;True;531;SwitchMask_Selects;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;429;313.6666,932.7748;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;430;554.6356,776.3711;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;525;596.2179,1100.986;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;594.9755,1255.474;Inherit;False;Property;_Opacity_Alpha;Opacity_Alpha;1;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;480;528.0014,1004.499;Inherit;False;Property;_Opacity_RGB;Opacity_RGB;0;1;[Header];Create;True;1;ParticlesSystem;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;479;916.1075,903.9129;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;432;911.226,1078.779;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;548;1062.052,1001.581;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;549;1241.56,1003.169;Float;False;True;-1;2;ASEMaterialInspector;0;9;FX_Kandol_Pack/Flipbook_Effects;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;4;1;False;407;1;False;405;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;True;2;False;406;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;True;True;2;False;403;True;3;False;404;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;455;0;454;0
WireConnection;457;0;455;0
WireConnection;457;1;456;0
WireConnection;458;0;457;0
WireConnection;146;0;145;1
WireConnection;146;1;145;2
WireConnection;502;0;500;0
WireConnection;466;0;464;0
WireConnection;466;1;465;0
WireConnection;460;0;458;0
WireConnection;342;0;146;0
WireConnection;469;1;467;0
WireConnection;469;4;464;0
WireConnection;468;0;466;0
WireConnection;461;0;460;0
WireConnection;461;2;459;0
WireConnection;505;0;502;0
WireConnection;340;0;145;3
WireConnection;340;1;145;4
WireConnection;150;0;149;0
WireConnection;339;0;342;0
WireConnection;339;1;340;0
WireConnection;144;0;146;0
WireConnection;144;1;340;0
WireConnection;471;0;469;0
WireConnection;471;1;468;0
WireConnection;499;0;541;0
WireConnection;499;1;501;0
WireConnection;499;2;502;0
WireConnection;503;0;541;0
WireConnection;503;1;501;0
WireConnection;503;2;505;0
WireConnection;462;0;461;0
WireConnection;337;0;339;0
WireConnection;337;1;150;0
WireConnection;474;0;471;0
WireConnection;474;1;470;0
WireConnection;135;0;335;0
WireConnection;135;1;499;0
WireConnection;336;0;335;0
WireConnection;336;1;503;0
WireConnection;148;0;144;0
WireConnection;148;1;150;0
WireConnection;522;0;335;0
WireConnection;522;1;337;0
WireConnection;472;0;475;0
WireConnection;472;1;474;0
WireConnection;521;0;335;0
WireConnection;521;1;148;0
WireConnection;506;0;135;1
WireConnection;506;1;336;1
WireConnection;523;0;521;1
WireConnection;523;1;522;1
WireConnection;362;0;472;0
WireConnection;362;1;372;0
WireConnection;141;0;506;0
WireConnection;141;1;142;0
WireConnection;345;0;141;0
WireConnection;539;0;523;0
WireConnection;539;1;545;0
WireConnection;473;0;362;0
WireConnection;438;0;435;0
WireConnection;538;0;539;0
WireConnection;437;0;438;0
WireConnection;546;0;536;0
WireConnection;546;1;363;0
WireConnection;544;1;540;0
WireConnection;544;0;546;0
WireConnection;440;0;437;0
WireConnection;440;1;441;0
WireConnection;365;0;41;0
WireConnection;365;1;544;0
WireConnection;444;0;443;0
WireConnection;444;1;445;0
WireConnection;528;0;440;0
WireConnection;528;2;529;0
WireConnection;45;0;488;0
WireConnection;484;0;528;0
WireConnection;442;0;365;0
WireConnection;442;1;444;0
WireConnection;34;0;442;0
WireConnection;34;1;482;0
WireConnection;34;2;481;0
WireConnection;34;3;39;0
WireConnection;34;4;40;0
WireConnection;34;5;45;0
WireConnection;35;0;554;0
WireConnection;35;1;34;0
WireConnection;543;1;485;0
WireConnection;543;0;486;0
WireConnection;52;0;35;0
WireConnection;52;1;35;4
WireConnection;531;0;543;0
WireConnection;61;0;52;0
WireConnection;61;1;63;0
WireConnection;439;0;61;0
WireConnection;439;1;532;0
WireConnection;27;0;439;0
WireConnection;27;1;28;0
WireConnection;29;0;555;0
WireConnection;29;1;27;0
WireConnection;430;0;29;0
WireConnection;430;1;429;0
WireConnection;525;0;429;4
WireConnection;525;1;533;0
WireConnection;479;0;430;0
WireConnection;479;1;480;0
WireConnection;432;0;525;0
WireConnection;432;1;32;0
WireConnection;548;0;479;0
WireConnection;548;3;432;0
WireConnection;549;0;548;0
ASEEND*/
//CHKSM=68408C82FC0191BF0018D40093833FCD320AF9FB