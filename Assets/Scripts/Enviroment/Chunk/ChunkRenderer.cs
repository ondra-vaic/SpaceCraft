using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChunkRenderer : MonoBehaviour
{
    
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;
    private ChunkData chunkData; 
    
    private List<Vector3> verticies = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2> ();


    void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        chunkData = GetComponent<ChunkData>();
    }

    public bool Render()
    {
        verticies = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        
        addCubesToMesh();
        return setUpMesh();
    }

    private void addCubesToMesh()
    {
        int triangleOffset = 0;
        for (int x = 0; x < MapGenerator.CHUNK_WIDTH; x++)
        {
            for (int y = 0; y < MapGenerator.CHUNK_WIDTH; y++)
            {
                for (int z = 0; z < MapGenerator.CHUNK_WIDTH; z++)
                {
                    if (chunkData[x, y, z].IsInitialized())
                        addCubeToMesh(chunkData[x, y, z], x, y, z, ref triangleOffset);
                }
            }
        }
    }

    private void addCubeToMesh(Block block, int x, int y, int z, ref int triangleOffset)
    {
        for (int i = 0; i < 6; i++)
        {
            Block.Face face = (Block.Face) i;
            if (hasNeighborFace(x, y, z, face))
                continue;
            
            verticies.AddRange(block.GetFace(face));
            triangles.AddRange(Block.FaceTriangles.Offset(triangleOffset * 4));
            uvs.AddRange(block.GetUVs(face));
            triangleOffset += 1;
        }
    }

    bool hasNeighborFace(int x, int y, int z, Block.Face face)
    {
        if (x <= 0 || x >= MapGenerator.CHUNK_WIDTH - 1 || y <= 0 || y >= MapGenerator.CHUNK_WIDTH - 1 || z <= 0 || z >= MapGenerator.CHUNK_WIDTH - 1)
            return false;

        Vector3Int id = new Vector3Int(x, y, z) + Block.FaceDirection[face];
        
        return chunkData[id.x, id.y, id.z].IsInitialized();
    }
    
    private bool setUpMesh()
    {
        if (verticies.Count == 0)
        {
            return false;
        }
        
        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        
        mesh.vertices = verticies.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        return true;
    }
}
