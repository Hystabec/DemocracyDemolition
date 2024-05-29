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
    AudioClip breakSoundClip;

    Vector2 unPausedVelocity = Vector2.zero;
    float unPausedGravity = 1.0f;
    public void Pause()
    {
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();

        //rb.freezeRotation = true;
        unPausedGravity = rb.gravityScale;
        rb.gravityScale = 0.0f;

        unPausedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
    }

    public void UnPause()
    {
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();

        //rb.freezeRotation = false;
        rb.gravityScale = unPausedGravity;
        unPausedGravity = 1.0f;

        rb.velocity = unPausedVelocity;
        unPausedVelocity = Vector2.zero;
    }

    public void DespawnProjectile()
    {
        bounceCount = 0;
        if (owner != null)
            owner.despawnProjectile(this.gameObject);
        else
            gameObject.SetActive(false);
    }

    public void ResetBounces()
    {
        bounceCount = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Trampoline"))
        {

            bounceCount++;
            //coconutParticles.SetActive(true);


            if (bounceCount >= maxBounceCount)
            {
                bounceCount = 0;
                FindFirstObjectByType<soundManager>().PlayOnce(breakSoundClip, 0.5f);
                DespawnProjectile();
            }



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
