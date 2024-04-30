using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TestTools;
using Unity.VisualScripting;
using System;
using Codice.Utils;

public class EditorAddScriptToAll : EditorWindow
{
    static GameObject[] selObject = null;
    static MonoScript scriptToAdd = null;

    private void OnGUI()
    {
        for(int i = 0; i < selObject.Length; i++)
        {
            selObject[i] = EditorGUILayout.ObjectField("GameObject " + i.ToString(), selObject[i], typeof(GameObject), false) as GameObject;
        }

        EditorGUILayout.Space();
        
        scriptToAdd =  EditorGUILayout.ObjectField("Script to add", scriptToAdd, typeof(MonoScript), false) as MonoScript;
        System.Type type = scriptToAdd.GetType();

        //MonoBehaviour temp = scriptToAdd.GetType();
        //MonoBehaviour temp = scriptToAdd.GetType();
        //System.Type m_scriptClass = scriptToAdd.GetType();
        //Component tempComp = (scriptToAdd as Component); 

        if (GUILayout.Button("Add") && scriptToAdd != null )
        {
            foreach(GameObject go in selObject)
            {
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);

                GameObject tempGO = new GameObject("temp");
                Component tempComp = tempGO.AddComponent(type);

                PrefabUtility.ApplyAddedComponent(tempComp, path, InteractionMode.AutomatedAction);

                DestroyImmediate(tempGO);

                //UnityEngine.Object temp = PrefabUtility.GetPrefabInstanceHandle(go);
                //temp.AddComponent(m_scriptClass);
            }
        }
    }

    private void OnDestroy()
    {
        scriptToAdd = null;
        selObject = null;
    }

    [MenuItem("Tools/Add Script to selected Prefab")]
    private static void AddCompToObjects()
    {
        selObject = Selection.gameObjects;

        EditorWindow.GetWindow(typeof(EditorAddScriptToAll));
    }

    [MenuItem("Tools/Add Script to selected Prefab", true)]
    private static bool Validation()
    {
        GameObject[] selected = Selection.gameObjects;
        bool canProcced = true;

        if (selected.Length == 0)
            return false;

        foreach (GameObject item in selected)
        {
            if(PrefabUtility.IsAnyPrefabInstanceRoot(item))
                canProcced = false;
        }

        return canProcced;
    }
}
