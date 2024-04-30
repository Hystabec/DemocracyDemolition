using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TestTools;
using Unity.VisualScripting;

public class EditorAddComponentToAll : EditorWindow
{
    static GameObject[] selObject = null;

    private void OnGUI()
    {
        for(int i = 0; i < selObject.Length; i++)
        {
            selObject[i] = EditorGUILayout.ObjectField("GameObject " + i.ToString(), selObject[i], typeof(GameObject), false) as GameObject;
        }

        Component compToAdd = null;
        compToAdd =  EditorGUILayout.ObjectField("Component to add", compToAdd, typeof(Component), false) as Component;

        if(GUILayout.Button("Add") && compToAdd != null )
        {
            foreach(GameObject go in selObject)
            {
                PrefabUtility.ApplyAddedComponent(compToAdd, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go), InteractionMode.AutomatedAction);
            }
        }
    }

    private void OnDestroy()
    {
        selObject = null;
    }

    [MenuItem("Tools/Add Component to selected")]
    private static void AddCompToObjects()
    {
        EditorWindow.GetWindow(typeof(EditorAddComponentToAll));
    }

    [MenuItem("Tools/Add Component to selected", true)]
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
