using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    // some declaration missing??
   
    override public void  OnInspectorGUI () {
        
        MapGenerator mapGenerator = (MapGenerator)target;
        
        if(GUILayout.Button("Generate")) {
            mapGenerator.Regenerate(); // how do i call this?
        }
        DrawDefaultInspector();
    }
}

 
