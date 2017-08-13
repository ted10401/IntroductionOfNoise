#include "BaseNoise.cginc"

fixed simplex_noise(fixed3 p)
{
	fixed tetrahedraToCube = 0.333333333;
	fixed cubeToTetrahedra = 0.166666667;

	fixed skew = (p.x + p.y + p.z) * tetrahedraToCube;
	fixed3 i = floor(p + skew);
	fixed unskew = (i.x + i.y + i.z) * cubeToTetrahedra;
	fixed3 d0 = p - i + unskew;

	fixed3 e = step(fixed3(0, 0, 0), d0 - d0.yzx);
	fixed3 i1 = e * (1.0 - e.zxy);
	fixed3 i2 = 1.0 - e.zxy * (1.0 - e);

	fixed3 d1 = d0 - (i1 - 1.0 * cubeToTetrahedra);
	fixed3 d2 = d0 - (i2 - 2.0 * cubeToTetrahedra);
	fixed3 d3 = d0 - (1.0 - 3.0 * cubeToTetrahedra);

	fixed4 h = max(0.5 - fixed4(dot(d0, d0), dot(d1, d1), dot(d2, d2), dot(d3, d3)), 0.0);
	fixed4 n = h * h * h * h * fixed4(dot(d0, hash33(i)), dot(d1, hash33(i + i1)), dot(d2, hash33(i + i2)), dot(d3, hash33(i + 1.0)));

	return dot(fixed4(31.316, 31.316, 31.316, 31.316), n);
}

fixed simplex_noise_sum(fixed3 p, int fractal)
{
	fixed result = 0;
	fixed amplitude = 1;
	fixed maxValue = amplitude;

	result += simplex_noise(p);

	if(fractal > 1)
	{
		for(int it = 1; it < fractal; it++)
		{
			p *= _NoiseLacunarity;
			amplitude *= _NoisePersistence;
			maxValue += amplitude;
			result += amplitude * simplex_noise(p);
		}
	}

	return result / maxValue;
}

fixed simplex_noise_sum_abs(fixed3 p, int fractal)
{
	fixed result = 0;
	fixed amplitude = 1;
	fixed maxValue = amplitude;

	result += abs(simplex_noise(p));

	if(fractal > 1)
	{
		for(int it = 1; it < fractal; it++)
		{
			p *= _NoiseLacunarity;
			amplitude *= _NoisePersistence;
			maxValue += amplitude;
			result += amplitude * abs(simplex_noise(p));
		}
	}

	return result / maxValue;
}

fixed simplex_noise_sum_abs_sin(fixed3 p, int fractal)
{
	fixed result = simplex_noise_sum_abs(p, fractal);

	return sin(result + p.x);
}

fixed4 frag_simplex_noise_sum (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = simplex_noise_sum(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}

fixed4 frag_simplex_noise_sum_abs (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = simplex_noise_sum_abs(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}

fixed4 frag_simplex_noise_sum_abs_sin (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed3 noise;

	noise = simplex_noise_sum_abs_sin(fixed3(i.uv * _NoiseFrequency, _Time.y * _NoiseSpeed), _NoiseOctave);

	col.rgb *= noise;

	return col;
}