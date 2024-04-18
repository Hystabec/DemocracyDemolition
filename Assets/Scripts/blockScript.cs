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

    private GameObject Player;
    private playerScript chosenPlayerScript;

    Collider2D col;

    private bool canPlaceBlock = true;


    // Start is called before the first frame update
    void Start()
    {
        CurrentBlockHealth = TotalBlockHealth;

        col = gameObject.GetComponent<Collider2D>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChosePlayer(GameObject playerobj)
    {
        Player = playerobj;
        chosenPlayerScript = Player.GetComponent<playerScript>();
        chosenPlayerScript.AddBlocks(gameObject);

    }

    public void assignPlayer(GameObject owningPlayer)
    {
        Player = owningPlayer;
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
            if (Player)
            {
                canPlaceBlock = false;
                chosenPlayerScript.CanPlaceBlock(false);

            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (Player)
            {
                canPlaceBlock = false;
                chosenPlayerScript.CanPlaceBlock(false);
            }
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            if (Player)
            {
                canPlaceBlock = true;
                chosenPlayerScript.CanPlaceBlock(true);
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (Player)
            {
                canPlaceBlock = true;
                chosenPlayerScript.CanPlaceBlock(true);

            }
        }
    }
}
