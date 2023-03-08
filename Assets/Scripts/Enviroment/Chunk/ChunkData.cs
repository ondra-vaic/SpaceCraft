using UnityEngine;

public class ChunkData : MonoBehaviour
{
    public int Length => blocks.Length;
    
    [SerializeField] private Block[] blocks;
    
    public Block this[int x, int y, int z]
    {
        get => blocks[Utils.To1DId(x, y, z)];
        set => blocks[Utils.To1DId(x, y, z)] = value;
    }
    
    public void SetData(Block[] blocks)
    {
        this.blocks = blocks;
    }

    public void SetEmpty(int x, int y, int z)
    {
        blocks[Utils.To1DId(x, y, z)].BlockType = (int)BlockType.UNKNOWN;
    }

    public Block[] GetData()
    {
        return blocks;
    }
    
    
}
