using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TestTools;
using Unity.VisualScripting;
using System;

public class EditorAddScriptToAll : EditorWindow
{
    static GameObject[] selObject = null;
    static UnityEngine.Component compToAdd = null;

    private void OnGUI()
    {
        if (selObject == null)
            return;

        EditorGUILayout.LabelField("This tool isnt great - i wouldnt use it if i were you.");
        EditorGUILayout.Space();

        for (int i = 0; i < selObject.Length; i++)
        {
            selObject[i] = EditorGUILayout.ObjectField("GameObject " + i.ToString(), selObject[i], typeof(GameObject), false) as GameObject;
        }

        EditorGUILayout.Space();

        compToAdd =  EditorGUILayout.ObjectField("Script to add", compToAdd, typeof(UnityEngine.Component), false) as UnityEngine.Component;
        
        //Converting monoscripts to components - seems impossible

        //MonoBehaviour temp = scriptToAdd.GetType();
        //MonoBehaviour temp = scriptToAdd.GetType();
        //System.Type m_scriptClass = scriptToAdd.GetType();
        //Component tempComp = (scriptToAdd as Component); 

        if (GUILayout.Button("Add") && compToAdd != null )
        {
            foreach(GameObject go in selObject)
            {
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);

                GameObject tempGO = new();
                tempGO = PrefabUtility.LoadPrefabContents(path);

                tempGO.AddComponent(compToAdd.GetType());

                PrefabUtility.SaveAsPrefabAsset(tempGO, path);

                GameObject.DestroyImmediate(tempGO);

                //UnityEngine.Object temp = PrefabUtility.GetPrefabInstanceHandle(go);
                //temp.AddComponent(m_scriptClass);
            }
        }
    }

    private void OnDestroy()
    {
        compToAdd = null;
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
