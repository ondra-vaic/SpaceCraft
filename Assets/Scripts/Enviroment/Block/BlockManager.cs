using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class BlockManager : MonoBehaviour
{   
    [SerializeField] private TextAsset texturesDataJson;
    [SerializeField] private List<BlockWrapper> blocks;
    
    private Dictionary<BlockType, BlockData> blocksLU = new Dictionary<BlockType, BlockData>();
    public static BlockManager Instance;
    private TextureUVS textureUVs;
    
    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
        
        initBlocksLU();
        initTextureUVs();
    }

    private void initBlocksLU()
    {
        foreach (var block in blocks)
        {
            BlockData blockData = block.BlockData;
            
            blockData.Initialize();
            blockData.BlockType = block.BlockType;
            blocksLU[block.BlockType] = blockData;
        }
    }

    private void initTextureUVs()
    {
        textureUVs = JsonConvert.DeserializeObject<TextureUVS>(texturesDataJson.text);
        
        float xScale = 1f / textureUVs.Meta.Size.W;
        float yScale = 1f / textureUVs.Meta.Size.H;
        
        foreach (var uvFrame in textureUVs.Frames)
        {
            uvFrame.Value.Frame.X *= xScale;
            uvFrame.Value.Frame.W *= xScale;
            uvFrame.Value.Frame.Y *= yScale;
            uvFrame.Value.Frame.H *= yScale;
        }
    }

    public static Vector2[] GetTextureUVs(string textureName)
    {
        Frame frame = Instance.textureUVs.Frames[textureName].Frame;
        
        Vector2[] uvs = 
        {
            new Vector2(frame.X, 1 - (frame.Y + frame.H)),
            new Vector2(frame.X, 1 - frame.Y),
            new Vector2(frame.X + frame.W, 1 - (frame.Y + frame.H)),
            new Vector2(frame.X + frame.W, 1 - frame.Y)
        };

        return uvs;
    }
    
    public static BlockData GetBlock(BlockType blockType)
    {
        if(!Instance.blocksLU.ContainsKey(blockType))
            print(blockType);
        
        return Instance.blocksLU[blockType];
    }
}

[Serializable]
public class TextureUVS
{
    public Dictionary<string, TextureInfo> Frames;
    public TextureMeta Meta;
}

[Serializable]
public class TextureInfo
{
    public Frame Frame;
}

[Serializable]
public class Frame
{
    public float X;
    public float Y;
    public float W;
    public float H;
}

[Serializable]
public class TextureMeta
{
    public Size Size;
}

[Serializable]
public class Size
{
    public int W;
    public int H;
}

[Serializable]
public class BlockWrapper
{
    public BlockType BlockType;
    public BlockData BlockData;
}