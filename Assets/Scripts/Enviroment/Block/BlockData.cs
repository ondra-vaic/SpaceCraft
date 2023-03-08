using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BlockData.asset", menuName = "ScriptableObjects/BlockData")]
public class BlockData : ScriptableObject
{
    [HideInInspector] public BlockType BlockType;
    public float TimeToBreak;
    
    [SerializeField] private Textures textures;
    private Dictionary<Block.Face, string> faceToTextureName;
    
    public Vector2[] GetUVs(Block.Face face)
    {
        return BlockManager.GetTextureUVs(faceToTextureName[face]);
    }

    public void Initialize()
    {
        Debug.Assert(textures.Default != string.Empty);
        
        faceToTextureName = new Dictionary<Block.Face, string>()
        {
            {Block.Face.TOP, getTextureName(textures.Top)},
            {Block.Face.DOWN, getTextureName(textures.Bot)},
            {Block.Face.FRONT,getTextureName(textures.Front)},
            {Block.Face.BACK, getTextureName(textures.Back)},
            {Block.Face.LEFT, getTextureName(textures.Left)},
            {Block.Face.RIGHT,getTextureName(textures.Right)},
        };
    }

    private string getTextureName(string textureName)
    {
        return textureName == String.Empty ? textures.Default : textureName;
    }
}

[Serializable]
public struct Textures
{
    public string Default;
    
    public string Top;
    public string Bot;
    public string Left;
    public string Right;
    public string Front;
    public string Back;
}




