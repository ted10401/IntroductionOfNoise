using UnityEngine;
using System.Collections.Generic;

public delegate float NoiseMethodDelegate (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency);
public delegate float InterpolateMethodDelegate (float x, float y, float t);

public enum NoiseMethodType
{
    Value,
    Perlin,
    Simplex
}

public enum DimensionType
{
    OneDimension,
    TwoDimension,
    ThreeDimension
}

public enum InterpolateMethodType
{
    Point,
    Linear,
    Fade1,
    Fade2
}

public class Noise
{
    public static float GetValue(Vector3 point, NoiseMethodType noiseMethodType, DimensionType dimension, InterpolateMethodType interpolate, float frequency, int octave, float lacunarity, float persistence)
    {
        float value = 0;

        switch (noiseMethodType)
        {
            case NoiseMethodType.Value:
                value = FractalSum(point, m_valueMethods[(int)dimension], m_interpolateMethods[(int)interpolate], frequency, octave, lacunarity, persistence);
                break;
            case NoiseMethodType.Perlin:
                value = FractalSum(point, m_perlinMethods[(int)dimension], m_interpolateMethods[(int)interpolate], frequency, octave, lacunarity, persistence);
                break;
            case NoiseMethodType.Simplex:
                value = FractalSum(point, m_simplexMethods[(int)dimension], m_interpolateMethods[(int)interpolate], frequency, octave, lacunarity, persistence);
                break;
        }

        return value;
    }


    private static float FractalSum(Vector3 point, NoiseMethodDelegate noiseMethod, InterpolateMethodDelegate interpolateMethod, float frequency, int octave, float lacunarity, float persistence)
    {
        float sum = noiseMethod(point, interpolateMethod, frequency);
        float amplitude = 1f;
        float maxValue = 1f;

        for (int i = 1; i < octave; i++)
        {
            frequency *= lacunarity;
            amplitude *= persistence;
            maxValue += amplitude;
            sum += noiseMethod(point, interpolateMethod, frequency) * amplitude;
        }

        return sum / maxValue;
    }


    private static NoiseMethodDelegate[] m_valueMethods =
        {
            Value1D,
            Value2D,
            Value3D
        };
    

    private static NoiseMethodDelegate[] m_perlinMethods =
        {
            Perlin1D,
            Perlin2D,
            Perlin3D
        };

    private static NoiseMethodDelegate[] m_simplexMethods =
        {
            Simplex1D,
            Simplex2D,
            Simplex3D
        };

    private static InterpolateMethodDelegate[] m_interpolateMethods =
        {
            InterpolatePoint,
            InterpolateLinear,
            InterpolateFade1,
            InterpolateFade2
        };
    

    private static int[] m_hash = {
        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
        57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
        74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
        60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
        65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
        52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
        81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
        57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
        74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
        60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
        65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
        52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
        81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
    };

    private const int HASH_LENGTH = 255;


    public static void GenerateHash()
    {
        List<int> hash1 = new List<int>();
        List<int> hash2 = new List<int>();

        for (int i = 0; i < 256; i++)
        {
            hash1.Add(i);
        }

        int value = 0;
        int random = 0;
        while (hash1.Count > 0)
        {
            random = Random.Range(0, hash1.Count);
            value = hash1[random];
            hash1.RemoveAt(random);
            hash2.Add(value);
        }

        hash2.AddRange(hash2);

        m_hash = hash2.ToArray();
    }


    private static float InterpolatePoint(float x, float y, float t)
    {
        return x;
    }


    private static float InterpolateLinear(float x, float y, float t)
    {
        return Mathf.Lerp(x, y, t);
    }


    private static float InterpolateFade1(float x, float y, float t)
    {
        t = t * t * (3 - 2 * t);

        return Mathf.Lerp(x, y, t);
    }


    private static float InterpolateFade2(float x, float y, float t)
    {
        t = t * t * t * (6 * t * t - 15 * t + 10);

        return Mathf.Lerp(x, y, t);
    }


    private static float Value1D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        int i0 = Mathf.FloorToInt(point.x);

        float t = point.x - i0;

        i0 &= HASH_LENGTH;

        int i1 = i0 + 1;

        int h0 = m_hash[i0];
        int h1 = m_hash[i1];

        float sample = interpolateMethod(h0, h1, t);

        return sample * (2f / HASH_LENGTH) - 1f;
    }


    private static float Value2D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);

        float tx = point.x - ix0;
        float ty = point.y - iy0;

        ix0 &= HASH_LENGTH;
        iy0 &= HASH_LENGTH;

        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;

        int h0 = m_hash[ix0];
        int h1 = m_hash[ix1];

        int h00 = m_hash[h0 + iy0];
        int h10 = m_hash[h1 + iy0];
        int h01 = m_hash[h0 + iy1];
        int h11 = m_hash[h1 + iy1];

        float xLerp1 = interpolateMethod(h00, h10, tx);
        float xLerp2 = interpolateMethod(h01, h11, tx);

        float sample = interpolateMethod(xLerp1, xLerp2, ty);

        return sample * (2f / HASH_LENGTH) - 1f;
    }


    private static float Value3D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);
        int iz0 = Mathf.FloorToInt(point.z);

        float tx = point.x - ix0;
        float ty = point.y - iy0;
        float tz = point.z - iz0;

        ix0 &= HASH_LENGTH;
        iy0 &= HASH_LENGTH;
        iz0 &= HASH_LENGTH;

        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;
        int iz1 = iz0 + 1;

        int h0 = m_hash[ix0];
        int h1 = m_hash[ix1];

        int h00 = m_hash[h0 + iy0];
        int h10 = m_hash[h1 + iy0];
        int h01 = m_hash[h0 + iy1];
        int h11 = m_hash[h1 + iy1];

        int h000 = m_hash[h00 + iz0];
        int h100 = m_hash[h10 + iz0];
        int h010 = m_hash[h01 + iz0];
        int h110 = m_hash[h11 + iz0];
        int h001 = m_hash[h00 + iz1];
        int h101 = m_hash[h10 + iz1];
        int h011 = m_hash[h01 + iz1];
        int h111 = m_hash[h11 + iz1];

        float xLerp1 = interpolateMethod(h000, h100, tx);
        float xLerp2 = interpolateMethod(h010, h110, tx);
        float xLerp3 = interpolateMethod(h001, h101, tx);
        float xLerp4 = interpolateMethod(h011, h111, tx);

        float yLerp1 = interpolateMethod(xLerp1, xLerp2, ty);
        float yLerp2 = interpolateMethod(xLerp3, xLerp4, ty);

        float sample = interpolateMethod(yLerp1, yLerp2, tz);

        return sample * (2f / HASH_LENGTH) - 1f;
    }


    private static float[] m_gradients1D = {
        1f, -1f
    };

    private const int GRADIENT_MASK_1D = 1;

    private static float Perlin1D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        int i0 = Mathf.FloorToInt(point.x);

        float t0 = point.x - i0;
        float t1 = t0 - 1f;
        i0 &= HASH_LENGTH;
        int i1 = i0 + 1;

        float g0 = m_gradients1D[m_hash[i0] & GRADIENT_MASK_1D];
        float g1 = m_gradients1D[m_hash[i1] & GRADIENT_MASK_1D];

        float v0 = g0 * t0;
        float v1 = g1 * t1;

        float sample = interpolateMethod(v0, v1, t0);

        return sample * 2f;
    }


    private static Vector2[] m_gradients2D = {
        new Vector2( 1f, 0f),
        new Vector2(-1f, 0f),
        new Vector2( 0f, 1f),
        new Vector2( 0f,-1f),
        new Vector2( 1f, 1f).normalized,
        new Vector2(-1f, 1f).normalized,
        new Vector2( 1f,-1f).normalized,
        new Vector2(-1f,-1f).normalized
    };

    private const int GRADIENT_MASK_2D = 7;
    private static float m_sqrt2 = Mathf.Sqrt(2f);

    private static float Dot(Vector3 gradient, float x, float y)
    {
        return gradient.x * x + gradient.y * y;
    }

    private static float Perlin2D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);

        float tx0 = point.x - ix0;
        float ty0 = point.y - iy0;
        float tx1 = tx0 - 1;
        float ty1 = ty0 - 1;

        ix0 &= HASH_LENGTH;
        iy0 &= HASH_LENGTH;

        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;

        int h0 = m_hash[ix0];
        int h1 = m_hash[ix1];

        Vector2 g00 = m_gradients2D[m_hash[h0 + iy0] & GRADIENT_MASK_2D];
        Vector2 g10 = m_gradients2D[m_hash[h1 + iy0] & GRADIENT_MASK_2D];
        Vector2 g01 = m_gradients2D[m_hash[h0 + iy1] & GRADIENT_MASK_2D];
        Vector2 g11 = m_gradients2D[m_hash[h1 + iy1] & GRADIENT_MASK_2D];

        float v00 = Dot(g00, tx0, ty0);
        float v10 = Dot(g10, tx1, ty0);
        float v01 = Dot(g01, tx0, ty1);
        float v11 = Dot(g11, tx1, ty1);

        float xLerp1 = interpolateMethod(v00, v10, tx0);
        float xLerp2 = interpolateMethod(v01, v11, tx0);

        float sample = interpolateMethod(xLerp1, xLerp2, ty0);

        return sample * m_sqrt2;
    }


    private static Vector3[] m_gradients3D = {
        new Vector3( 1f, 1f, 0f),
        new Vector3(-1f, 1f, 0f),
        new Vector3( 1f,-1f, 0f),
        new Vector3(-1f,-1f, 0f),
        new Vector3( 1f, 0f, 1f),
        new Vector3(-1f, 0f, 1f),
        new Vector3( 1f, 0f,-1f),
        new Vector3(-1f, 0f,-1f),
        new Vector3( 0f, 1f, 1f),
        new Vector3( 0f,-1f, 1f),
        new Vector3( 0f, 1f,-1f),
        new Vector3( 0f,-1f,-1f),

        new Vector3( 1f, 1f, 0f),
        new Vector3(-1f, 1f, 0f),
        new Vector3( 0f,-1f, 1f),
        new Vector3( 0f,-1f,-1f)
    };

    private const int GRADIENT_MASK_3D = 15;

    private static float Dot(Vector3 gradient, float x, float y, float z)
    {
        return gradient.x * x + gradient.y * y + gradient.z * z;
    }

    private static float Perlin3D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);
        int iz0 = Mathf.FloorToInt(point.z);

        float tx0 = point.x - ix0;
        float ty0 = point.y - iy0;
        float tz0 = point.z - iz0;
        float tx1 = tx0 - 1;
        float ty1 = ty0 - 1;
        float tz1 = tz0 - 1;

        ix0 &= HASH_LENGTH;
        iy0 &= HASH_LENGTH;
        iz0 &= HASH_LENGTH;
        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;
        int iz1 = iz0 + 1;

        int h0 = m_hash[ix0];
        int h1 = m_hash[ix1];

        int h00 = m_hash[h0 + iy0];
        int h10 = m_hash[h1 + iy0];
        int h01 = m_hash[h0 + iy1];
        int h11 = m_hash[h1 + iy1];

        Vector3 g000 = m_gradients3D[m_hash[h00 + iz0] & GRADIENT_MASK_3D];
        Vector3 g100 = m_gradients3D[m_hash[h10 + iz0] & GRADIENT_MASK_3D];
        Vector3 g010 = m_gradients3D[m_hash[h01 + iz0] & GRADIENT_MASK_3D];
        Vector3 g110 = m_gradients3D[m_hash[h11 + iz0] & GRADIENT_MASK_3D];
        Vector3 g001 = m_gradients3D[m_hash[h00 + iz1] & GRADIENT_MASK_3D];
        Vector3 g101 = m_gradients3D[m_hash[h10 + iz1] & GRADIENT_MASK_3D];
        Vector3 g011 = m_gradients3D[m_hash[h01 + iz1] & GRADIENT_MASK_3D];
        Vector3 g111 = m_gradients3D[m_hash[h11 + iz1] & GRADIENT_MASK_3D];

        float v000 = Dot(g000, tx0, ty0, tz0);
        float v100 = Dot(g100, tx1, ty0, tz0);
        float v010 = Dot(g010, tx0, ty1, tz0);
        float v110 = Dot(g110, tx1, ty1, tz0);
        float v001 = Dot(g001, tx0, ty0, tz1);
        float v101 = Dot(g101, tx1, ty0, tz1);
        float v011 = Dot(g011, tx0, ty1, tz1);
        float v111 = Dot(g111, tx1, ty1, tz1);

        float xLerp1 = interpolateMethod(v000, v100, tx0);
        float xLerp2 = interpolateMethod(v010, v110, tx0);
        float xLerp3 = interpolateMethod(v001, v101, tx0);
        float xLerp4 = interpolateMethod(v011, v111, tx0);

        float yLerp1 = interpolateMethod(xLerp1, xLerp2, ty0);
        float yLerp2 = interpolateMethod(xLerp3, xLerp4, ty0);

        float sample = interpolateMethod(yLerp1, yLerp2, tz0);

        return sample;
    }


    private static float Simplex1D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;
        int ix = Mathf.FloorToInt(point.x);

        float value = Simplex1DFunction(point, ix);
        value += Simplex1DFunction(point, ix + 1);

        return value * (2f / HASH_LENGTH) - 1f;
    }


    private static float Simplex1DFunction(Vector3 point, int ix)
    {
        float x = point.x - ix;
        float x2 = x * x;
        float f = 1 - x2;
        float f3 = f * f * f;

        int hx = ix & HASH_LENGTH;

        float hash = m_hash[hx];

        return f3 * hash;
    }


    private static float SQUARES_TO_TRIANGLES = 0.2113248654052f; //(3f - Mathf.Sqrt(3f)) / 6f
    private static float TRIANGLES_TO_SQUARES = 0.3660254037844f; //(Mathf.Sqrt(3f) - 1f) / 2f

    private static float Simplex2D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        float skew = (point.x + point.y) * TRIANGLES_TO_SQUARES;
        float sx = point.x + skew;
        float sy = point.y + skew;

        int ix = Mathf.FloorToInt(sx);
        int iy = Mathf.FloorToInt(sy);

        float value = Simplex2DFunction(point, ix, iy);
        value += Simplex2DFunction(point, ix + 1, iy + 1);

        if (sx - ix >= sy - iy)
        {
            value += Simplex2DFunction(point, ix + 1, iy);
        }
        else
        {
            value += Simplex2DFunction(point, ix, iy + 1);
        }

        return value * (8f * 2f / HASH_LENGTH) - 1f;
    }


    private static float Simplex2DFunction(Vector3 point, int ix, int iy)
    {
        float unskew = (ix + iy) * SQUARES_TO_TRIANGLES;
        float x = point.x - ix + unskew;
        float y = point.y - iy + unskew;
        float x2 = x * x;
        float y2 = y * y;
        float f = 0.5f - x2 - y2;

        if (f > 0)
        {
            float f3 = f * f * f;

            int hx = ix & HASH_LENGTH;
            int hy = iy & HASH_LENGTH;

            float hash = m_hash[m_hash[hx] + hy];

            return f3 * hash;
        }

        return 0;
    }


    private static float TETRAHEDRA_TO_CUBE = 1f / 3f;
    private static float CUBE_TO_TETRAHEDRA = 1f / 6f;

    private static float Simplex3D (Vector3 point, InterpolateMethodDelegate interpolateMethod, float frequency)
    {
        point *= frequency;

        float skew = (point.x + point.y + point.z) * TETRAHEDRA_TO_CUBE;
        float sx = point.x + skew;
        float sy = point.y + skew;
        float sz = point.z + skew;
        int ix = Mathf.FloorToInt(sx);
        int iy = Mathf.FloorToInt(sy);
        int iz = Mathf.FloorToInt(sz);

        float value = Simplex3DFunction(point, ix, iy, iz);
        value += Simplex3DFunction(point, ix + 1, iy + 1, iz + 1);

        float x = sx - ix;
        float y = sy - iy;
        float z = sz - iz;
        if (x >= y)
        {
            if (x >= z)
            {
                value += Simplex3DFunction(point, ix + 1, iy, iz);

                if (y >= z)
                {
                    value += Simplex3DFunction(point, ix + 1, iy + 1, iz);
                }
                else
                {
                    value += Simplex3DFunction(point, ix + 1, iy, iz + 1);
                }
            }
            else
            {
                value += Simplex3DFunction(point, ix, iy, iz + 1);
                value += Simplex3DFunction(point, ix + 1, iy, iz + 1);
            }
        }
        else
        {
            if (y >= z)
            {
                value += Simplex3DFunction(point, ix, iy + 1, iz);

                if (x >= z)
                {
                    value += Simplex3DFunction(point, ix + 1, iy + 1, iz);
                }
                else
                {
                    value += Simplex3DFunction(point, ix, iy + 1, iz + 1);
                }
            }
            else
            {
                value += Simplex3DFunction(point, ix, iy, iz + 1);
                value += Simplex3DFunction(point, ix, iy + 1, iz + 1);
            }
        }

        return value * (8f * 2f / HASH_LENGTH) - 1f;
    }


    private static float Simplex3DFunction(Vector3 point, int ix, int iy, int iz)
    {
        float unskew = (ix + iy + iz) * CUBE_TO_TETRAHEDRA;

        float x = point.x - ix + unskew;
        float y = point.y - iy + unskew;
        float z = point.z - iz + unskew;

        float f = 0.5f - x * x - y * y - z * z;

        if (f > 0)
        {
            float f3 = f * f * f;

            int hx = ix & HASH_LENGTH;
            int hy = iy & HASH_LENGTH;
            int hz = iz & HASH_LENGTH;

            float hash = m_hash[m_hash[m_hash[hx] + hy] + hz];

            return f3 * hash;
        }

        return 0;
    }
}
