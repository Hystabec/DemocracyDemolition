using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManagerScript : MonoBehaviour
{
    /// <summary>
    /// when placing a block its collider should be disabled, once placed the collider should be enabled.
    /// when not selected block should have its outline hidden, when selected enable outline
    /// </summary>


    List<GameObject> blockList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objectsFromFolder = Resources.LoadAll<GameObject>("BlockPrefabs");

        foreach(GameObject obj in objectsFromFolder) 
        {
            blockList.Add(obj); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
