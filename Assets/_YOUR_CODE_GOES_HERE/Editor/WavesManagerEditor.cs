
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(WavesManager))]
public class WavesManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WavesManager manager = (WavesManager)target;
        if(GUILayout.Button("Generate wave file"))
        {
            manager.GenerateWave();
        }
    }
}
