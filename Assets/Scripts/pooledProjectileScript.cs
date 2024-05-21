using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pooledProjectileScript : MonoBehaviour
{
    public playerScript owner;

    [SerializeField]
    int maxBounceCount = 3;

    int bounceCount = 0;

    [SerializeField]
    GameObject coconutParticles;

    public void DespawnProjectile()
    {
        if (owner != null)
            owner.despawnProjectile(this.gameObject);
        else
            gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCount++;

        coconutParticles.SetActive(true);

        if (bounceCount >= maxBounceCount)
        {
            bounceCount = 0;

            DespawnProjectile();
        }

        

        /*if (collision.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            blockType type = collision.gameObject.GetComponent<blockScript>().getBlockType();
            Debug.Log(type);

            switch (type) 
            {
                case blockType.Glass:

                    break;

                case blockType.Wood:

                    break;

                case blockType.Stone:

                    break;

                case blockType.Metal:

                    break;

                default: Debug.Log("Block has no type"); break;
            }
        }*/
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
