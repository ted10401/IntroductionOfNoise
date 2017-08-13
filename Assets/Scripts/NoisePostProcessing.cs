using UnityEngine;

[ExecuteInEditMode]
public class NoisePostProcessing : MonoBehaviour
{
    public enum FractalMethod
    {
        Sum,
        SumAbs,
        SumAbsSin
    }

    [SerializeField] private NoiseMethodType m_noiseMethod = NoiseMethodType.Value;
    [SerializeField] private FractalMethod m_fractalMethod = FractalMethod.Sum;
    [SerializeField] private float m_noiseFrequency = 16;
    [SerializeField] private float m_noiseSpeed = 1;
    [Range(1, 8)]
    [SerializeField] private int m_noiseOctave = 1;
    [SerializeField] private float m_noiseLacunarity = 2f;
    [SerializeField] private float _noisePersistence = 0.5f;

    private Material m_material;
    private string m_noiseFrequencyPropertyName = "_NoiseFrequency";
    private string m_noiseSpeedPropertyName = "_NoiseSpeed";
    private string m_noiseOctavePropertyName = "_NoiseOctave";
    private string m_noiseLacunarityPropertyName = "_NoiseLacunarity";
    private string m_noisePersistencePropertyName = "_NoisePersistence";
    private int m_noiseFrequencyPropertyId;
    private int m_noiseSpeedPropertyId;
    private int m_noiseOctavePropertyId;
    private int m_noiseLacunarityPropertyId;
    private int m_noisePersistencePropertyId;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (!CheckMaterial())
        {
            Graphics.Blit(src, dst);
        }
        else
        {
            m_material.SetFloat(m_noiseFrequencyPropertyId, m_noiseFrequency);
            m_material.SetFloat(m_noiseSpeedPropertyId, m_noiseSpeed);
            m_material.SetFloat(m_noiseOctavePropertyId, m_noiseOctave);
            m_material.SetFloat(m_noiseLacunarityPropertyId, m_noiseLacunarity);
            m_material.SetFloat(m_noisePersistencePropertyId, _noisePersistence);

            Graphics.Blit(src, dst, m_material, (int)m_noiseMethod * 3 + (int)m_fractalMethod);
        }
    }

    private bool CheckMaterial()
    {
        Shader shader = Shader.Find("Hidden/Noise");
        if (null == shader)
        {
            return false;
        }

        if (null == m_material || m_material.shader != shader)
        {
            m_material = new Material(shader);
        }

        m_noiseFrequencyPropertyId = Shader.PropertyToID(m_noiseFrequencyPropertyName);
        m_noiseSpeedPropertyId = Shader.PropertyToID(m_noiseSpeedPropertyName);
        m_noiseOctavePropertyId = Shader.PropertyToID(m_noiseOctavePropertyName);
        m_noiseLacunarityPropertyId = Shader.PropertyToID(m_noiseLacunarityPropertyName);
        m_noisePersistencePropertyId = Shader.PropertyToID(m_noisePersistencePropertyName);

        return true;
    }
}
