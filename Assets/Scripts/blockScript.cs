using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockScript : MonoBehaviour
{
    [SerializeField]
    int TotalBlockHealth = 3;
    int CurrentBlockHealth = 3;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        CurrentBlockHealth = TotalBlockHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            CurrentBlockHealth--;

            if(CurrentBlockHealth <= 0)
            {
                CurrentBlockHealth = TotalBlockHealth;
                Debug.Log(gameObject.name + " destroyed");
            }
        }

        //Sets it in playerScript so the player cannot place the current block if its colliding with another block or the player

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            player.GetComponent<playerScript>().canPlace = false;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerScript>().canPlace = false;
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            player.GetComponent<playerScript>().canPlace = true;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerScript>().canPlace = true;
        }
    }
}
