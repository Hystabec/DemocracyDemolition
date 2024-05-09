using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTScript : MonoBehaviour
{
    [SerializeField]
    float explosionRadius = 1.0f;

    GameObject radiusOutline;

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
                    GenericBlockScript GBS;
                    col.gameObject.TryGetComponent<GenericBlockScript>(out GBS);

                    if(GBS != null)
                    {
                        //if its a block checks its placed

                        //if its placed it can be deleted
                        if(GBS.IsPlaced())
                            Destroy(col.gameObject);
                    }
                    else
                    {
                        //if its anything else delete it 

                        Destroy(col.gameObject);
                    }  
                }
            }
        }
    }
}
