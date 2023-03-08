using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;

    private void setOverrides(Block[] blocks, Dictionary<(int x, int y, int z), Block> overrides)
    {
        if(overrides == null)
            return;
        
        foreach (var overrideBlock in overrides)
        {
            blocks[Utils.To1DId(overrideBlock.Key.x, overrideBlock.Key.y, overrideBlock.Key.z)] = overrideBlock.Value;
        }
    }
    
    public Block[] Generate(int chunkX, int chunkY, int chunkZ, Vector3 seed, Dictionary<(int x, int y, int z), Block> overrides)
    {
        int chunkWidth = MapGenerator.CHUNK_WIDTH;
        Block[] blocks = new Block[chunkWidth * chunkWidth * chunkWidth];
        setOverrides(blocks, overrides);
        
        int numThreads = chunkWidth / 8;
        int octavesSize = sizeof(float) * 3 * 3;
        
        //Blocks
        int enumSize = sizeof(int);
        int positionSize = sizeof(int) * 3;
        int boolSize = sizeof(int) * 2;

        ComputeBuffer cubesBuffer = new ComputeBuffer(blocks.Length, enumSize + positionSize + boolSize);
        cubesBuffer.SetData(blocks);
        
        //Bioms
        int biomSize = octavesSize;
        biomSize += sizeof(float) * 3;
        biomSize += sizeof(float);
        biomSize += sizeof(int);
        biomSize += sizeof(int) * 3;

        ComputeBuffer biomsBuffer = new ComputeBuffer(MapGenerator.BiomConfigs.Length, biomSize);
        biomsBuffer.SetData(MapGenerator.BiomConfigs);
        
        //Planets
        int planetSize = sizeof(float) * 2;
        planetSize += sizeof(float) * 2;
        planetSize += sizeof(int);
        planetSize += sizeof(float);
        planetSize += sizeof(int) * 2;
        planetSize += sizeof(float);
        planetSize += octavesSize;

        ComputeBuffer planetsBuffer = new ComputeBuffer(MapGenerator.Planets.Length, planetSize);
        planetsBuffer.SetData(MapGenerator.Planets);

        //Mountains
        int mountainSize = sizeof(float); 
        
        mountainSize += octavesSize;
        mountainSize += sizeof(int);
        
        ComputeBuffer mountainsBuffer = new ComputeBuffer(MapGenerator.Mountains.Length, mountainSize);
        mountainsBuffer.SetData(MapGenerator.Mountains);

        
        computeShader.SetBuffer(0, "blocks", cubesBuffer);
        computeShader.SetBuffer(0, "bioms", biomsBuffer);
        computeShader.SetBuffer(0, "planets", planetsBuffer);
        computeShader.SetBuffer(0, "mountains", mountainsBuffer);
        
        computeShader.SetInt("numBioms", MapGenerator.BiomConfigs.Length);
        computeShader.SetInt("chunkWidth", chunkWidth);
        computeShader.SetFloats("seed", new float[]{seed.x, seed.y, seed.z});
        computeShader.SetInts("chunkPosition", new int[]{chunkX * chunkWidth, chunkY * chunkWidth, chunkZ * chunkWidth});
        
        computeShader.Dispatch(0,  numThreads, numThreads, numThreads);
        
        cubesBuffer.GetData(blocks);
        cubesBuffer.Dispose();
        biomsBuffer.Dispose();
        planetsBuffer.Dispose();
        mountainsBuffer.Dispose();
        
        return blocks;
    }
}
