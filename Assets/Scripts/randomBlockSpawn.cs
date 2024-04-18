using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomBlockSpawn : MonoBehaviour
{

    private GameObject[] availableBlocks;
    private GameObject[] availableTraps;

    private bool trapsAllowed = false;

    private Vector3 spawnPos;



    void Start()
    {
        availableBlocks = Resources.LoadAll<GameObject>("BlockPrefabs");
        availableTraps = Resources.LoadAll<GameObject>("TrapPrefabs");
        spawnPos = new Vector3(-15, 2, 0);

        SpawnBlock();
        SpawnBlock();
        SpawnBlock();
        SpawnBlock();
        SpawnBlock();
        SpawnBlock();

    }


    void SpawnBlock()
    {
        GameObject thisBlock = Instantiate(ChooseBlock(), spawnPos, Quaternion.identity);
        thisBlock.SetActive(false);
        Debug.Log(thisBlock);
    }


    GameObject ChooseBlock()
    {
        float trap = Random.Range(0f, 1f);

        if (trapsAllowed && trap > 0.5f)
        {
   
            return availableTraps[Random.Range(0, availableTraps.Length)];     
        }

        else
        {
            return availableBlocks[Random.Range(0, availableBlocks.Length)];

        }

    }
   
}
