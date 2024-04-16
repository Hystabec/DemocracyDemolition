using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TestTools;

public class EditorBlockPrefabGen : EditorWindow
{
    [MenuItem("Assets/Gen Block Prefab")]
    private static void GenerateBlockPrefab()
    {
        Object[] selectObjects = Selection.objects;
        string lastPath = "";

        foreach (Object obj in selectObjects)
        {
            string prefabPath = "Assets/" + obj + ".prefab";

            GameObject go = new GameObject("temp");
            go.layer = LayerMask.NameToLayer("Block");

            go.AddComponent<SpriteRenderer>().sprite = obj as Sprite;

            go.AddComponent<PolygonCollider2D>();

            Rigidbody2D rb2d = go.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0.0f;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            go.AddComponent<blockScript>();

            GameObject outline = new GameObject("outline");
            outline.layer = LayerMask.NameToLayer("Block");
            outline.AddComponent<SpriteRenderer>();

            outline.transform.parent = go.transform;
            outline.SetActive(false);

            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);

            GameObject.DestroyImmediate(outline);
            GameObject.DestroyImmediate(go);

            lastPath = prefabPath;
        }

        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(lastPath));

        EditorUtility.DisplayDialog("prefab created: " + lastPath, "Check collider is correct\nAdd sprite to outline", "Ok");
    }

    [MenuItem("Assets/Gen Block Prefab", true)]
    private static bool Validation()
    {
        Object[] selected = Selection.objects;
        bool canProceed = true;

        if (selected.Length == 0)
            return false;

        foreach(Object obj in selected)
        {
            if (obj.GetType() != typeof(Sprite))
                canProceed = false;
        }

        return canProceed;
    }

    
}
