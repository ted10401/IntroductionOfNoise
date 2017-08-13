#include "BaseNoise.cginc"

fixed value_noise(fixed3 p)
{
	fixed3 pi = floor(p);
	fixed3 pf = p - pi;

	fixed3 t = pf * pf * pf * (6 * pf * pf - 15 * pf + 10);

	fixed3 vec = fixed3(110, 241, 171);
	fixed n = dot(pi, vec);

	fixed g1 = hash11(n + dot(vec, fixed3(0, 0, 0)));
	fixed g2 = hash11(n + dot(vec, fixed3(1, 0, 0)));
	fixed g3 = hash11(n + dot(vec, fixed3(0, 1, 0)));
	fixed g4 = hash11(n + dot(vec, fixed3(1, 1, 0)));

	fixed g5 = hash11(n + dot(vec, fixed3(0, 0, 1)));
	fixed g6 = hash11(n + dot(vec, fixed3(1, 0, 1)));
	fixed g7 = hash11(n + dot(vec, fixed3(0, 1, 1)));
	fixed g8 = hash11(n + dot(vec, fixed3(1, 1, 1)));

	fixed x1 = lerp(g1, g2, t.x);
	fixed x2 = lerp(g3, g4, t.x);

	fixed x3 = lerp(g5, g6, t.x);
	fixed x4 = lerp(g7, g8, t.x);

	fixed3 y1 = lerp(x1, x2, t.y);
	fixed3 y2 = lerp(x3, x4, t.y);

	return lerp(y1, y2, t.z);
}

fixed value_noise_sum(fixed3 p, int fractal)
{
	fixed result = 0;
	fixed amplitude = 1;
	fixed maxValue = amplitude;

	result += value_noise(p);

	if(fractal > 1)
	{
		for(int it = 1; it < fractal; it++)
		{
			p *= _NoiseLacunarity;
			amplitude *= _NoisePersistence;
			maxValue += amplitude;
			result += amplitude * value_noise(p);
		}
	}

	return result / maxValue;
}

fixed value_noise_sum_abs(fixed3 p, int fractal)
{
	fixed result = 0;
	fixed amplitude = 1;
	fixed maxValue = amplitude;

	result += abs(value_noise(p));

	if(fractal > 1)
	{
		for(int it = 1; it < fractal; it++)
		{
			p *= _NoiseLacunarity;
			amplitude *= _NoisePersistence;
			maxValue += amplitude;
			result += amplitude * abs(value_noise(p));
		}
	}

	return result / maxValue;
}

fixed value_noise_sum_abs_sin(fixed3 p, int fractal)
{
	fixed result = value_noise_sum_abs(p, fractal);

	return sin(result + p.x);
}

fixed4 frag_value_noise_sum (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = value_noise_sum(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}

fixed4 frag_value_noise_sum_abs (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = value_noise_sum_abs(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}

fixed4 frag_value_noise_sum_abs_sin (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = value_noise_sum_abs_sin(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}