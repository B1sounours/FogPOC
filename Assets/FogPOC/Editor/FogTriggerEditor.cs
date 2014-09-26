using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;

public static class MemberInfoGetting
{
    public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
    {
        MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
        return expressionBody.Member.Name;
    }
}

[CustomEditor(typeof(FogTrigger))]
public class FogTriggerEditor : Editor
{
    void displayProperty(ref bool applicable, int data)
    {
        EditorGUILayout.BeginHorizontal();
        applicable = EditorGUILayout.Toggle(applicable);

        // Useful for class name: string label = typeof(T).GetProperties()[0].Name;
        string name = MemberInfoGetting.GetMemberName(() => data);
        Type dataType = data.GetType();

        data = EditorGUILayout.IntField(data);

        EditorGUILayout.EndHorizontal();
    }

    void displayProperty(ref bool applicable, float data)
    {
        EditorGUILayout.BeginHorizontal();
        applicable = EditorGUILayout.Toggle(applicable);

        // Useful for class name: string label = typeof(T).GetProperties()[0].Name;
        string name = MemberInfoGetting.GetMemberName(() => data);
        Type dataType = data.GetType();

        data = EditorGUILayout.FloatField(data);

        EditorGUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        FogTrigger fogTrig = (FogTrigger)target;
        //displayProperty(ref fogTrig.fogSett.applyFlareStrength, fogTrig.fogSett.FlareStrength);

        if (!EditorApplication.isPlaying)
        { 
            EditorGUILayout.HelpBox("Applying this in the editor will permanently change your rendering settings!", MessageType.Warning);
        }
        GUILayout.BeginHorizontal();

        
        if (GUILayout.Button("Apply"))
        {
            fogTrig.fogSett.ApplySettings();
        }
        if (GUILayout.Button("Reset"))
        {
            fogTrig.fogSett.LoadSettings();
        }
        GUILayout.EndHorizontal();
    }
}
