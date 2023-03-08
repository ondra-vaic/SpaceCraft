using UnityEngine;

public static class Utils
{
    public static int[] Offset(this int[] ints, int offset)
    {
        int[] offsetInts = new int[ints.Length];

        for (int i = 0; i < ints.Length; i++)
        {
            offsetInts[i] = ints[i] + offset;
        }
        
        return offsetInts;
    }
    
    public static Vector3[] Offset(this Vector3Int[] vs, Vector3 offset)
    {
        Vector3[] offsetVectors = new Vector3[vs.Length];

        for (int i = 0; i < vs.Length; i++)
        {
            offsetVectors[i] = vs[i] + offset;
        }
        
        return offsetVectors;
    }
    
    public static int To1DId(int x, int y, int z) {
        return (z * MapGenerator.CHUNK_WIDTH * MapGenerator.CHUNK_WIDTH) + (y * MapGenerator.CHUNK_WIDTH) + x;
    }
}
