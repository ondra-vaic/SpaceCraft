using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    private Transform shipController;
    
    private ChunkData chunkData;
    private ChunkRenderer chunkRenderer;
    private ChunkGenerator chunkGenerator;
    
    public bool Initialize(int chunkX, int chunkY, int chunkZ, Vector3 seed, Dictionary<(int x, int y, int z), Block> overrides)
    {
        chunkData = GetComponent<ChunkData>();
        chunkRenderer = GetComponent<ChunkRenderer>();
        chunkGenerator = GetComponent<ChunkGenerator>();

        Block[] cubes = chunkGenerator.Generate(chunkX, chunkY, chunkZ, seed, overrides);
        chunkData.SetData(cubes);
        return chunkRenderer.Render();
    }

    public void PlaceBlock(Block block, (int x, int y, int z) localPos)
    {
        chunkData[localPos.x, localPos.y, localPos.z] = block;
        chunkRenderer.Render();
    }

    public BlockType TypeAt(int x, int y, int z) => (BlockType) chunkData[x, y, z].BlockType;
}
