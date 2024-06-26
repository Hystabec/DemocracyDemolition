using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class randomBlockSpawn : MonoBehaviour
{

    private GameObject[] availableBlocks;
    private GameObject[] availableTraps;

    private bool trapsAllowed = false;

    private Vector3 spawnPos;

    [SerializeField]
    private GameObject player1, player2;


    void Awake()
    {
        availableBlocks = Resources.LoadAll<GameObject>("BlockPrefabs");
        availableTraps = Resources.LoadAll<GameObject>("TrapPrefabs");
        spawnPos = new Vector3(-15, 2, 0);

        //SpawnBlock();
        //SpawnBlock();
        //SpawnBlock();
        //SpawnBlock();
        //SpawnBlock();
        //SpawnBlock();

        //spawnBlockIn(player1, 3);
        //spawnBlockIn(player2, 3);
    }


    void SpawnBlock()
    {
        GameObject thisBlock = Instantiate(ChooseBlock(), spawnPos, Quaternion.identity);
        var blockScript = thisBlock.GetComponent<blockScript>();
       // thisBlock.SetActive(false);
    }

    public void spawnTrapIn(GameObject ToGiveTo, int numberToGive)
    {
        playerScript PS = ToGiveTo.GetComponent<playerScript>();

        bool applyOffset = false;

        if (PS.getPlayerIndex() == 1)
            applyOffset = true;

        for (int i = 0; i < numberToGive; i++)
        {
            GameObject thisBlock = Instantiate(randomTrap(), spawnPos, Quaternion.identity);

            if (applyOffset)
                thisBlock.transform.rotation = thisBlock.GetComponent<GenericBlockScript>().GetPlayer2RotationOffset();

            PS.AddBlockToList(thisBlock);
        }
    }

    public void spawnBlockIn(GameObject ToGiveTo, int numberToGive)
    {
        playerScript PS = ToGiveTo.GetComponent<playerScript>();

        bool applyOffset = false;

        if (PS.getPlayerIndex() == 1)
            applyOffset = true;

        for (int i = 0; i < numberToGive; i++)
        {
            GameObject thisBlock = Instantiate(randomBlock(), spawnPos, Quaternion.identity);

            if (applyOffset)
                thisBlock.transform.rotation = thisBlock.GetComponent<GenericBlockScript>().GetPlayer2RotationOffset();

            PS.AddBlockToList(thisBlock);
        }
    }

    GameObject randomTrap()
    {
        return availableTraps[UnityEngine.Random.Range(0, availableTraps.Length)];
    }

    GameObject randomBlock()
    {
        return availableBlocks[UnityEngine.Random.Range(0, availableBlocks.Length)];
    }

    GameObject ChooseBlock()
    {
        float trap = UnityEngine.Random.Range(0f, 1f);

        if (trapsAllowed && trap > 0.5f)
        {
   
            return availableTraps[UnityEngine.Random.Range(0, availableTraps.Length)];     
        }

        else
        {
            return availableBlocks[UnityEngine.Random.Range(0, availableBlocks.Length)];

        }
    }

    GameObject ChoosePlayer()
    {
        var p1Script = player1.GetComponent<playerScript>();

        if (p1Script.blockObjects.Count < 3)
        {
            return player1;
        }

        else
        {
            return player2;

        }
    }
}
