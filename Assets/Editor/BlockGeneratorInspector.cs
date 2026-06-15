using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(BlockGenerator))]
public class BlockGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BlockGenerator generator = (BlockGenerator)target;

        if (GUILayout.Button("Generate Starting Layout"))
        {
            generator.ResetGeneration();
            generator.GenerateChunk(generator.chunkSize.y, generator.chunkSize.y);
            generator.GenerateChunk(0, 0);
        }

        if (GUILayout.Button("Generate Flat Layout"))
        {
            generator.ResetGeneration();
            
            generator.GenerateChunk(1, 1);
        }

        if (GUILayout.Button("Generating next Level Layout"))
        {
            //generator.GenerateNextLevel();
            generator.ResetGeneration();

            generator.GenerateChunk(0,0);
        }

        if (GUILayout.Button("Reset"))
        {
            generator.ResetGeneration();
        }
    }
}
