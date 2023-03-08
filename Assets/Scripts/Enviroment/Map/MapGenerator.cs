using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private ChunkController chunkControllerPrefab;

    private Dictionary<(int x, int y, int z), ChunkController> chunks = new Dictionary<(int x, int y, int z), ChunkController>();

    //first is key chunk pos, second key is position of block in chunk
    private Dictionary<(int x, int y, int z), Dictionary<(int x, int y, int z), Block>> worldOverrideBlocks = new Dictionary<(int x, int y, int z), Dictionary<(int x, int y, int z), Block>>();

    [SerializeField] private BiomConfig[] biomConfigs;
    [SerializeField] private Planet[] planets;
    [SerializeField] private Mountain[] mountains;
    
    [SerializeField] private int chunkRadius;
    [SerializeField] private int chunkClipDistance;
    [SerializeField] private int chunkWidth;
    
    [SerializeField] private Vector3 seed;

    private BreakingBlock breakingBlock;

    public static float CHUNK_CLIP_DISTANCE => Instance.chunkClipDistance;
    public static int CHUNK_WIDTH => Instance.chunkWidth;
    public static int MIN_CHUNKS_RADIUS => (int)CHUNK_CLIP_DISTANCE / CHUNK_WIDTH;
    
    public static float MAP_UPDATE_TIME = 0.4f;

    public static MapGenerator Instance;
    public static BiomConfig[] BiomConfigs => Instance.biomConfigs;
    public static Planet[] Planets => Instance.planets;
    public static Mountain[] Mountains => Instance.mountains;

    private void Start()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        Regenerate();
        StartCoroutine(updateMap());
    }

    private IEnumerator updateMap()
    {
        while (true)
        {
            clipChunks();
            generateChunks();

            yield return new WaitForSeconds(MAP_UPDATE_TIME);
        }
    }

    public void Regenerate()
    {
        chunks = new Dictionary<(int x, int y, int z), ChunkController>();
        Transform[] children = GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < children.Length; i++)
        {
            if(children[i].gameObject != gameObject)
                Destroy(children[i].gameObject);
        }

        for (int i = 0; i < chunkRadius; i++)
        {
            for (int j = 0; j < chunkRadius; j++)
            {
                for (int k = 0; k < chunkRadius; k++)
                {
                    GenerateChunk(i, j, k);   
                }
            }
        }
    }
    
    public void GenerateChunk(int chunkX, int chunkY, int chunkZ)
    {
        ChunkController chunkController = Instantiate(chunkControllerPrefab, new Vector3(chunkX * CHUNK_WIDTH, chunkY * CHUNK_WIDTH, chunkZ * CHUNK_WIDTH), Quaternion.identity);
        chunkController.Initialize(chunkX, chunkY, chunkZ, seed, getOverrides((chunkX, chunkY, chunkZ)));

        chunkController.transform.parent = transform;
        chunks[(chunkX, chunkY, chunkZ)] = chunkController;
    }

    public BlockType HitBlock(Vector3 pos, float strength)
    {
        Vector3 p = (pos / chunkWidth);
        (int x, int y, int z) posChunk = ((int)p.x, (int)p.y, (int)p.z);
        (int x, int y, int z) local = ((int)(pos.x -posChunk.x * chunkWidth), (int)(pos.y - posChunk.y * chunkWidth), (int)(pos.z - posChunk.z * chunkWidth));
        
        BlockType blockType = chunks[posChunk].TypeAt(local.x, local.y, local.z);
        float timeToBreak = BlockManager.GetBlock(blockType).TimeToBreak;
        
        if (breakingBlock == null || !(breakingBlock.ChunkPos == posChunk && breakingBlock.LocalPos == local))
        {
            breakingBlock = new BreakingBlock(posChunk, local, 0);
        }
        else
        {
            breakingBlock.TimeToBreak += Time.deltaTime * strength;
        }
        
        if (timeToBreak < breakingBlock.TimeToBreak)
        {
            placeBlock(posChunk, local, BlockType.UNKNOWN);
            return blockType;
        }
        
        return BlockType.UNKNOWN;
    }

    public void PlaceBlock(Vector3 pos, BlockType blockType)
    {
        Vector3 p = (pos / chunkWidth);
        (int x, int y, int z) posChunk = ((int)p.x, (int)p.y, (int)p.z);

        (int x, int y, int z)  local = ((int)(pos.x -posChunk.x * chunkWidth), (int)(pos.y - posChunk.y * chunkWidth), (int)(pos.z - posChunk.z * chunkWidth));
        
        placeBlock(posChunk, local, blockType);
    }

    private void placeBlock((int x, int y, int z) posChunk, (int x, int y, int z)  local, BlockType blockType)
    {
        Block block = new Block(new Vector3Int(local.x, local.y, local.z), (int)blockType);
        chunks[posChunk].PlaceBlock(block, local);
        addToOverrides(posChunk, local, block);
    }
    
    private void addToOverrides((int x, int y, int z) posChunk, (int x, int y, int z)  local, Block block)
    {
        if (!worldOverrideBlocks.ContainsKey(posChunk))
        {
            worldOverrideBlocks[posChunk] = new Dictionary<(int x, int y, int z), Block>();
        }  
        
        worldOverrideBlocks[posChunk][local] = block;
    }

    private Dictionary<(int x, int y, int z), Block> getOverrides((int x, int y, int z) posChunk)
    {
        if (worldOverrideBlocks.ContainsKey(posChunk))
        {
            return worldOverrideBlocks[posChunk];
        }
        return null;
    }
    
    private void generateChunks()
    {
        for (int i = -MIN_CHUNKS_RADIUS; i < MIN_CHUNKS_RADIUS; i++)
        {
            for (int j = -MIN_CHUNKS_RADIUS / 2; j < MIN_CHUNKS_RADIUS / 2; j++)
            {
                for (int k = 0; k < MIN_CHUNKS_RADIUS; k++)
                {
                    checkGenerateChunk(new Vector3Int(i, j, k));
                }
            }
        }
    }

    private void checkGenerateChunk(Vector3Int chunkLocalPosition)
    {
        var chunkPosition = character.TransformPoint(chunkLocalPosition * chunkWidth);
        (int x, int y, int z) pos = ((int)chunkPosition.x / chunkWidth, (int)chunkPosition.y / chunkWidth,  (int)chunkPosition.z / chunkWidth); 

        if(!chunks.ContainsKey(pos))
            GenerateChunk(pos.x, pos.y, pos.z);
    }

    private void clipChunks()
    {
        List<(int x, int y, int z)> chunksToRemove = new List<(int x, int y, int z)>();
        
        foreach (var kv in chunks)
            checkClipChunk(kv, chunksToRemove);

        foreach (var chunk in chunksToRemove)
            chunks.Remove(chunk);
    }

    private void checkClipChunk(KeyValuePair<(int x, int y, int z), ChunkController> chunkKV, List<(int x, int y, int z)> chunksToRemove)
    {
        if(!chunkKV.Value)
            return;
        
        Vector3 chunkInCharacterSpace = character.transform.InverseTransformPoint(chunkKV.Value.transform.position);

        if (chunkInCharacterSpace.z < -CHUNK_CLIP_DISTANCE)
        {
            Destroy(chunks[chunkKV.Key].gameObject);
            chunksToRemove.Add(chunkKV.Key);
        }   
    }
}

public class BreakingBlock
{
    public (int x, int y, int z) ChunkPos;
    public (int x, int y, int z) LocalPos;
    public float TimeToBreak;

    public BreakingBlock((int x, int y, int z) chunkPos, (int x, int y, int z) localPos, float timeToBreak)
    {
        this.ChunkPos = chunkPos;
        this.LocalPos = localPos;
        this.TimeToBreak = timeToBreak;
    }
}