using UnityEditor;
using UnityEngine;
using System.IO;

public class NoiseGeneratorWindow : EditorWindow
{
    private int m_resolution = 256;
    private NoiseMethodType m_noiseMethod = NoiseMethodType.Value;
    private DimensionType m_noiseDimension = DimensionType.OneDimension;
    private InterpolateMethodType m_noiseInterpolate = InterpolateMethodType.Point;
    private bool m_generateNewHash = false;
    private float m_noiseFrequency = 1f;
    private int m_noiseOctave = 1;
    private float m_noiseLacunarity = 2f;
    private float m_noisePersistence = 0.5f;
    private Texture2D m_tempNoiseTexture;

    private Vector3 m_point00 = new Vector3(-0.5f, -0.5f);
    private Vector3 m_point01 = new Vector3(-0.5f, 0.5f);
    private Vector3 m_point10 = new Vector3(0.5f, -0.5f);
    private Vector3 m_point11 = new Vector3(0.5f, 0.5f);

    [MenuItem("Tools/Noise Generator")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow<NoiseGeneratorWindow>().Show();
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
        m_resolution = EditorGUILayout.IntField("Texture Resolution", m_resolution);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Noise Settings", EditorStyles.boldLabel);
        m_noiseMethod = (NoiseMethodType)EditorGUILayout.EnumPopup("Method", m_noiseMethod);
        m_noiseDimension = (DimensionType)EditorGUILayout.EnumPopup("Dimension", m_noiseDimension);

        if (m_noiseMethod != NoiseMethodType.Simplex)
        {
            m_noiseInterpolate = (InterpolateMethodType)EditorGUILayout.EnumPopup("Interpolate Type", m_noiseInterpolate);
        }

        m_generateNewHash = EditorGUILayout.Toggle("Generate New Hash", m_generateNewHash);
        m_noiseFrequency = EditorGUILayout.FloatField("Frequency", m_noiseFrequency);
        m_noiseOctave = EditorGUILayout.IntField("Octave", m_noiseOctave);
        m_noiseLacunarity = EditorGUILayout.FloatField("Lacunarity", m_noiseLacunarity);
        m_noisePersistence = EditorGUILayout.FloatField("Persistence", m_noisePersistence);

        OnShowButtons();

        EditorGUILayout.EndVertical();
    }


    private void OnShowButtons()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Texture"))
        {
            GenerateNoiseTexture();
        }

        if (null != m_tempNoiseTexture)
        {
            if (GUILayout.Button("Save Texture"))
            {
                SaveTexture();
            }
        }

        EditorGUILayout.EndHorizontal();

        if (null != m_tempNoiseTexture)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            rect.x = EditorGUIUtility.currentViewWidth / 2 - m_tempNoiseTexture.width / 2;
            rect.height = m_tempNoiseTexture.height;
            rect.width = m_tempNoiseTexture.width;

            EditorGUI.DrawPreviewTexture(rect, m_tempNoiseTexture);
        }
    }


    private void GenerateNoiseTexture()
    {
        if (null == m_tempNoiseTexture)
        {
            m_tempNoiseTexture = new Texture2D(m_resolution, m_resolution, TextureFormat.RGBA32, false);
            m_tempNoiseTexture.wrapMode = TextureWrapMode.Clamp;
            m_tempNoiseTexture.filterMode = FilterMode.Bilinear;
            m_tempNoiseTexture.name = "NoiseTexture";
        }
        else
        {
            if (m_tempNoiseTexture.width != m_resolution || m_tempNoiseTexture.height != m_resolution)
            {
                m_tempNoiseTexture.Resize(m_resolution, m_resolution);
            }
        }

        GenerateHash();
        FillTexture();
    }


    private void GenerateHash()
    {
        if (m_generateNewHash)
        {
            Noise.GenerateHash();
        }
    }


    private void FillTexture()
    {
        Vector3 point0 = Vector3.zero;
        Vector3 point1 = Vector3.zero;
        Vector3 point = Vector3.zero;

        float stepSize = 1f / m_resolution;

        for (int y = 0; y < m_resolution; y++)
        {
            point0 = Vector3.Lerp(m_point00, m_point01, (y + 0.5f) * stepSize);
            point1 = Vector3.Lerp(m_point10, m_point11, (y + 0.5f) * stepSize);

            for (int x = 0; x < m_resolution; x++)
            {
                point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);

                float sample = Noise.GetValue(point, m_noiseMethod, m_noiseDimension, m_noiseInterpolate, m_noiseFrequency, m_noiseOctave, m_noiseLacunarity, m_noisePersistence);
                sample = sample * 0.5f + 0.5f;

                m_tempNoiseTexture.SetPixel(x, y, Color.white * sample);
            }
        }

        m_tempNoiseTexture.Apply();
    }


    private void SaveTexture()
    {
        string path = EditorUtility.SaveFilePanel("Save noise texture as JPG", "", m_tempNoiseTexture.name + ".jpg", "jpg");

        if (path.Length != 0)
        {
            byte[] bytes = m_tempNoiseTexture.EncodeToJPG();
            File.WriteAllBytes(path, bytes);

            AssetDatabase.Refresh();
        }
    }
}
