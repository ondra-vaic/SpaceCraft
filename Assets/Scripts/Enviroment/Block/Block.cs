using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Block
{
    public int BlockType;
    public int BiomType;
    public Vector3Int Position;
    public int Visible;
   
    public Block(Vector3Int position, int blockType)
    {
        Position = position;
        BlockType = blockType;
        Visible = 0;
        BiomType = 0;
    }
    
    public Vector3[] GetFace(Face face)
    {
        return FaceToVertices[face].Offset(Position);
    }
  
    public Vector2[] GetUVs(Face face)
    {
        return BlockManager.GetBlock((BlockType)BlockType).GetUVs(face);
    }

    public bool IsInitialized() => BlockType != 0;
    
    public static Dictionary<Face, Vector3Int[]> FaceToVertices = new Dictionary<Face, Vector3Int[]>()
    {
        {Face.FRONT,
            new[] {
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(1, 1, 0)}},
                
            //
        {Face.BACK, 
            new[] {
                new Vector3Int(1, 0, 1),
                new Vector3Int(1, 1, 1),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 1, 1)
                }},
            
        {Face.LEFT,
            new[] {
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 1, 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 1, 0),
                }},
            
        {Face.RIGHT,
            new[] {
                new Vector3Int(1, 0, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(1, 0, 1),
                new Vector3Int(1, 1, 1)}},
        
        {Face.TOP, 
            new[] {
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, 1, 1),
                new Vector3Int(1, 1, 0),
                new Vector3Int(1, 1, 1)}},
        
        {Face.DOWN,
            new[] {
                new Vector3Int(1, 0, 0),
                new Vector3Int(1, 0, 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 0, 1)}}
    };
    
    public static readonly int[] FaceTriangles =
    {
        0, 1, 2, 1, 3, 2
    };
    
    public static Dictionary<Face, Vector3Int> FaceDirection = new Dictionary<Face, Vector3Int>()
    {
        {Face.TOP, 
            Vector3Int.up},
        
        {Face.DOWN,
            Vector3Int.down},
        
        {Face.FRONT,
            new Vector3Int(0, 0, -1)},
        
        {Face.BACK, 
            new Vector3Int(0, 0, 1)},
        
        {Face.LEFT, 
            Vector3Int.left},
        
        {Face.RIGHT,
            Vector3Int.right},
    };
    
    public enum Face
    {
        TOP = 0, DOWN = 1, FRONT = 2, BACK = 3, LEFT = 4, RIGHT = 5 
    }
}

