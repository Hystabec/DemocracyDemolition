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
        string prefabPath = "Assets/" + Selection.activeObject.name + ".prefab";

        GameObject go = new GameObject("temp");
        go.layer = LayerMask.NameToLayer("Block");

        go.AddComponent<SpriteRenderer>().sprite = Selection.activeObject as Sprite;

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

        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));

        EditorUtility.DisplayDialog("prefab created: " + prefabPath, "Check collider is correct\nAdd sprite to outline", "Ok");
    }

    [MenuItem("Assets/Gen Block Prefab", true)]
    private static bool Validation()
    {
        return Selection.activeObject.GetType() == typeof(Sprite);
    }
}
