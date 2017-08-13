#include "BaseNoise.cginc"

fixed perlin_noise(fixed3 p)
{
	fixed3 pi = floor(p);
	fixed3 pf = p - pi;

	fixed3 t = pf * pf * pf * (6 * pf * pf - 15 * pf + 10);

	fixed3 p1 = fixed3(0, 0, 0);
	fixed3 p2 = fixed3(1, 0, 0);
	fixed3 p3 = fixed3(0, 1, 0);
	fixed3 p4 = fixed3(1, 1, 0);
	fixed3 p5 = fixed3(0, 0, 1);
	fixed3 p6 = fixed3(1, 0, 1);
	fixed3 p7 = fixed3(0, 1, 1);
	fixed3 p8 = fixed3(1, 1, 1);

	fixed g1 = dot(hash33(pi + p1), pf - p1);
	fixed g2 = dot(hash33(pi + p2), pf - p2);
	fixed g3 = dot(hash33(pi + p3), pf - p3);
	fixed g4 = dot(hash33(pi + p4), pf - p4);

	fixed g5 = dot(hash33(pi + p5), pf - p5);
	fixed g6 = dot(hash33(pi + p6), pf - p6);
	fixed g7 = dot(hash33(pi + p7), pf - p7);
	fixed g8 = dot(hash33(pi + p8), pf - p8);

	fixed x1 = lerp(g1, g2, t.x);
	fixed x2 = lerp(g3, g4, t.x);

	fixed x3 = lerp(g5, g6, t.x);
	fixed x4 = lerp(g7, g8, t.x);

	fixed3 y1 = lerp(x1, x2, t.y);
	fixed3 y2 = lerp(x3, x4, t.y);

	return lerp(y1, y2, t.z) + 0.5;
}

fixed perlin_noise_sum(fixed3 p, int fractal)
{
	fixed result = 0;
	fixed amplitude = 1;
	fixed maxValue = amplitude;

	result += perlin_noise(p);

	if(fractal > 1)
	{
		for(int it = 1; it < fractal; it++)
		{
			p *= _NoiseLacunarity;
			amplitude *= _NoisePersistence;
			maxValue += amplitude;
			result += amplitude * perlin_noise(p);
		}
	}

	return result / maxValue;
}

fixed perlin_noise_sum_abs(fixed3 p, int fractal)
{
	fixed result = 0;
	fixed amplitude = 1;
	fixed maxValue = amplitude;

	result += abs(perlin_noise(p));

	if(fractal > 1)
	{
		for(int it = 1; it < fractal; it++)
		{
			p *= _NoiseLacunarity;
			amplitude *= _NoisePersistence;
			maxValue += amplitude;
			result += amplitude * abs(perlin_noise(p));
		}
	}

	return result / maxValue;
}

fixed perlin_noise_sum_abs_sin(fixed3 p, int fractal)
{
	fixed result = perlin_noise_sum_abs(p, fractal);

	return sin(result + p.x);
}

fixed4 frag_perlin_noise_sum (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = perlin_noise_sum(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}

fixed4 frag_perlin_noise_sum_abs (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = perlin_noise_sum_abs(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}

fixed4 frag_perlin_noise_sum_abs_sin (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = perlin_noise_sum_abs_sin(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}