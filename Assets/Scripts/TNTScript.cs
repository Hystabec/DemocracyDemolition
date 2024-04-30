using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTScript : MonoBehaviour
{
    [SerializeField]
    float explosionRadius = 1.0f;

    public void CustomOnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            Collider2D[] goInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach(Collider2D col in goInRadius)
            {
                if(col.gameObject.layer == LayerMask.NameToLayer("projectile") || 
                    col.gameObject.layer == LayerMask.NameToLayer("Block") || 
                    col.gameObject.layer == LayerMask.NameToLayer("Trap"))
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}
