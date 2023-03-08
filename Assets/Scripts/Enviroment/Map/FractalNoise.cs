using System;
using UnityEngine;

public static class FractalNoise
{
    public static float Noise(float _x, float _y, Octave[] octaves)
    {
        float noise = 0;
        float maxAmplitude = 0;
        
        for (int i = 0; i < octaves.Length; i++)
        {
            float x = _x * octaves[i].Frequency;
            float y = _y * octaves[i].Frequency;

            noise += Mathf.Pow(Perlin.Noise(x, y), octaves[i].Power) * octaves[i].Amplitude;

            maxAmplitude += octaves[i].Amplitude;
        }

        return noise / maxAmplitude;
    }
    
    public static float Noise(float _x, float _y, float _z, Octave[] octaves)
    {
        float noise = 0;
        float maxAmplitude = 0;
        
        for (int i = 0; i < octaves.Length; i++)
        {
            float x = _x * octaves[i].Frequency;
            float y = _y * octaves[i].Frequency;
            float z = _z * octaves[i].Frequency;

            noise += Mathf.Pow(Perlin.Noise(x, y, z), octaves[i].Power) * octaves[i].Amplitude;

            maxAmplitude += octaves[i].Amplitude;
        }

        return noise / maxAmplitude;
    }
}

[Serializable]
public struct Octave
{
    public float Frequency;
    public float Amplitude;
    public float Power;
}
