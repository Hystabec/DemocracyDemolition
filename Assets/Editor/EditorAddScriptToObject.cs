using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TestTools;
using Unity.VisualScripting;

public class EditorAddScriptToObject : EditorWindow
{
    static GameObject[] selObject = null;
    static MonoScript ms = null;

    private void OnGUI()
    {
        if (selObject == null)
            return;

        for (int i = 0; i < selObject.Length; i++)
        {
            selObject[i] = EditorGUILayout.ObjectField("GameObject " + i.ToString(), selObject[i], typeof(GameObject), false) as GameObject;
        }

        EditorGUILayout.Space();

        ms = EditorGUILayout.ObjectField("Script to add", ms, typeof(MonoScript), true) as MonoScript;

        if (GUILayout.Button("Add") && ms != null && ms.GetClass().IsSubclassOf(typeof(MonoBehaviour))) 
        {
            foreach (GameObject go in selObject)
            {
                go.AddComponent(ms.GetClass());
            }
        }
    }

    private void OnDestroy()
    {
        selObject = null;
        ms = null;
    }

    
    [MenuItem("Tools/Add Script to selected")]
    [MenuItem("Assets/Tools/Add Script to selected")]
    private static void AddCompToObjects()
    {
        selObject = Selection.gameObjects;
        EditorWindow.GetWindow(typeof(EditorAddScriptToObject));
    }

    [MenuItem("Tools/Add Script to selected", true)]
    [MenuItem("Assets/Tools/Add Script to selected", true)]
    private static bool Validation()
    {
        if (Selection.gameObjects.Length == 0)
            return false;

        return true;
    }
}
