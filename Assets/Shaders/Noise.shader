Shader "Hidden/Noise"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseFrequency ("Noise Frequency", Float) = 1
		_NoiseSpeed ("Noise Speed", Float) = 1
		_NoiseOctave ("Noise Octave", Range(1, 8)) = 1
		_NoiseLacunarity ("Noise Lacunarity", Float) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// Pass 0: Value Noise Sum
		Pass
		{
			CGPROGRAM

			#include "ValueNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_value_noise_sum

			ENDCG
		}

		// Pass 1: Value Noise Sum Abs
		Pass
		{
			CGPROGRAM

			#include "ValueNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_value_noise_sum_abs

			ENDCG
		}

		// Pass 2: Value Noise Sum Abs Sin
		Pass
		{
			CGPROGRAM

			#include "ValueNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_value_noise_sum_abs_sin

			ENDCG
		}

		// Pass 3: Perlin Noise Sum
		Pass
		{
			CGPROGRAM

			#include "PerlinNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_perlin_noise_sum

			ENDCG
		}

		// Pass 4: Perlin Noise Sum Abs
		Pass
		{
			CGPROGRAM

			#include "PerlinNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_perlin_noise_sum_abs

			ENDCG
		}

		// Pass 5: Perlin Noise Sum Abs sin
		Pass
		{
			CGPROGRAM

			#include "PerlinNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_perlin_noise_sum_abs_sin

			ENDCG
		}

		// Pass 6: Simplex Noise Sum
		Pass
		{
			CGPROGRAM

			#include "SimplexNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_simplex_noise_sum

			ENDCG
		}

		// Pass 7: Simplex Noise Sum Abs
		Pass
		{
			CGPROGRAM

			#include "SimplexNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_simplex_noise_sum_abs

			ENDCG
		}

		// Pass 8: Simplex Noise Sum Abs
		Pass
		{
			CGPROGRAM

			#include "SimplexNoise.cginc"

			#pragma vertex vert
			#pragma fragment frag_simplex_noise_sum_abs_sin

			ENDCG
		}
	}
}
