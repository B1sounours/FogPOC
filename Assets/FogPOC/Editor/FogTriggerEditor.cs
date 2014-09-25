using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FogTrigger))]
public class FogTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FogTrigger fogTrig = (FogTrigger)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply"))
        {
            fogTrig.fogSett.ApplySettings();
        }
        if (GUILayout.Button("Load"))
        {
            fogTrig.fogSett.LoadSettings();
        }
    }
}
