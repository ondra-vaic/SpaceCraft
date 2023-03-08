using System;
using UnityEngine;


[Serializable]
public struct Planet
{
    [SerializeField] Vector2 sizeRange;
    [SerializeField] Vector2 heightRange;
    [SerializeField] BlockType blockType;
    [SerializeField] float rarity;
    
    [SerializeField] int mountain0;
    [SerializeField] int mountain1;
    
    [SerializeField] float noiseAmplitude;
    [SerializeField] Octaves octaves;
}

[Serializable]
public struct BiomConfig
{
    [SerializeField] float threshold;
    [SerializeField] Vector3 seed;
    [SerializeField] public Octaves octaves;
    
    [SerializeField] int maxPlanetSize;
    [SerializeField] int planet0;
    [SerializeField] int planet1;
    [SerializeField] int planet2;
}

[Serializable]
public struct Octaves
{
    [SerializeField] Vector3 Octave1; // x - freq, y - amp, z - pow
    [SerializeField] Vector3 Octave2;
    [SerializeField] Vector3 Octave3;
}

[Serializable]
public struct Mountain
{
    [SerializeField] private float height;
    [SerializeField] Octaves octaves;
    [SerializeField] private BlockType blockType;
};