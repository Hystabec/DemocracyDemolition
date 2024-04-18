using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockScript : MonoBehaviour
{
    [SerializeField]
    int TotalBlockHealth = 3;
    int CurrentBlockHealth = 3;
    [SerializeField]
    bool breakable;

    private GameObject[] players;
    private GameObject chosenPlayer;


    // Start is called before the first frame update
    void Start()
    {
        CurrentBlockHealth = TotalBlockHealth;
        players = GameObject.FindGameObjectsWithTag("Player");
        ChosePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChosePlayer()
    {
        var p1Script = players[0].GetComponent<playerScript>();

        if(p1Script.selectableObjectsNumber < 3)
        {
            chosenPlayer = players[0];
        }

        else
        {
            chosenPlayer = players[1];
        }

        Debug.Log(chosenPlayer);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            if (breakable)
            {
                CurrentBlockHealth--;

                if (CurrentBlockHealth <= 0)
                {
                    CurrentBlockHealth = TotalBlockHealth;
                    Debug.Log(gameObject.name + " destroyed");
                    gameObject.SetActive(false);
                }
            }
        }

        //Sets it in playerScript so the player cannot place the current block if its colliding with another block or the player

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            chosenPlayer.GetComponent<playerScript>().canPlace = false;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            chosenPlayer.GetComponent<playerScript>().canPlace = false;
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            chosenPlayer.GetComponent<playerScript>().canPlace = true;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            chosenPlayer.GetComponent<playerScript>().canPlace = true;
        }
    }
}
