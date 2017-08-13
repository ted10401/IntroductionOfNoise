#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

struct v2f
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
};

v2f vert (appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

sampler2D _MainTex;
fixed _NoiseFrequency;
fixed _NoiseSpeed;
fixed _NoiseOctave;
fixed _NoiseLacunarity;
fixed _NoisePersistence;

fixed hash11(fixed n)
{
	return frac(sin(n) * 43758.5453123);
}

fixed3 hash33(fixed3 p)
{
	fixed3 mod = fixed3(0.1031, 0.11369, 0.13787);

	p = frac(p * mod);
    p += dot(p, p.yxz + 19.19);

    return -1.0 + 2.0 * frac(fixed3((p.x + p.y) * p.z, (p.x + p.z) * p.y, (p.y + p.z) * p.x));
}