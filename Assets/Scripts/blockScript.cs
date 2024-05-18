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
    private Sprite damagedSprite1 = null, damagedSprite2 = null;

    [SerializeField]
    private ParticleSystem hitEffect = null;

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

    void DestroyBlock()
    {
        if(GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().enabled = false;
        }

        if(GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }


    public void CustomOnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            aSource.PlayOneShot(blockHitSound);
            if (breakable)
            {
                if (hitEffect != null)
                {
                    hitEffect.Play();
                }
                Debug.Log("hit");

                CurrentBlockHealth--;

                if (CurrentBlockHealth == TotalBlockHealth - 1)
                {
                    if (damagedSprite1 != null)
                    {
                        GetComponent<SpriteRenderer>().sprite = damagedSprite1;
                    }
                }


                if (CurrentBlockHealth == TotalBlockHealth - 2)
                {
                    if (damagedSprite2 != null)
                    {
                        GetComponent<SpriteRenderer>().sprite = damagedSprite2;
                    }
                }

                if (CurrentBlockHealth <= 0)
                {
                    CurrentBlockHealth = TotalBlockHealth;
                    Debug.Log(gameObject.name + " destroyed");
                    DestroyBlock();
                    //gameObject.SetActive(false);
                }
            }
        }
    }
}
