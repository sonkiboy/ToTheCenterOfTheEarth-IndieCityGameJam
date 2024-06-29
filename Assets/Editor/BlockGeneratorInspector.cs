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
            generator.GenerateStart();
        }

        if (GUILayout.Button("Generating next Level Layout"))
        {
            generator.GenerateNextLevel();
        }

        if (GUILayout.Button("Reset"))
        {
            generator.ResetGeneration();
        }
    }
}
