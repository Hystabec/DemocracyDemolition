using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum blockType
{ 
    Wood,
    Stone,
    Metal,
    Glass
}

public class blockScript : MonoBehaviour
{
    [SerializeField]
    blockType type;

    [SerializeField]
    int TotalBlockHealth = 3;
    int CurrentBlockHealth = 3;
    [SerializeField]
    bool breakable;

    [SerializeField]
    private AudioClip blockHitSound;

    [SerializeField]
    private AudioSource aSource;

    public blockType getBlockType()
    {
        return type;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBlockHealth = TotalBlockHealth;

        //col = gameObject.GetComponent<Collider2D>();
        //col.enabled = false;
    }


    public void CustomOnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            aSource.PlayOneShot(blockHitSound);
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
    }
}
