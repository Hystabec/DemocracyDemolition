using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTScript : MonoBehaviour
{
    [SerializeField]
    float explosionRadius = 1.0f;

    [SerializeField]
    private GameObject aoe;

    private bool aoeActive;

    private screenShake screenShakeScript;

    private GenericBlockScript gbs;

    [SerializeField]
    GameObject Explosion;

    private void Awake()
    {
        screenShakeScript = FindAnyObjectByType<screenShake>();
        gbs = GetComponent<GenericBlockScript>();
    }

    private void Update()
    {
        if (gbs.isPlacing)
        {
            if (!aoeActive)
            {
                aoe.SetActive(true);
                aoeActive = true;
            }
        }

        if (gbs.IsPlaced())
        {
            if(aoeActive)
            {
                aoe.SetActive(false);
                aoeActive = false;
            }
        }
    }

    public void CustomOnCollisionEnter2D(Collision2D other)
    {
        if (gbs.IsPlaced())
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
            {
                Collider2D[] goInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

                foreach (Collider2D col in goInRadius)
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("projectile") ||
                        col.gameObject.layer == LayerMask.NameToLayer("Block") ||
                        col.gameObject.layer == LayerMask.NameToLayer("Trap"))
                    {
                        GenericBlockScript GBS;
                        col.gameObject.TryGetComponent<GenericBlockScript>(out GBS);

                        if (GBS != null)
                        {
                            //if its a block checks its placed

                            //if its placed it can be deleted
                            if (GBS.IsPlaced())
                                Instantiate(Explosion, transform.position, Quaternion.identity);
                            Destroy(col.gameObject.transform.root.gameObject);
                        }
                        else if (col.gameObject.layer == LayerMask.NameToLayer("projectile"))
                        {
                            //if its anything else delete it 
                            col.gameObject.GetComponent<pooledProjectileScript>().DespawnProjectile();
                        }
                    }
                }
                screenShakeScript.TriggerShake();
            }
        }
    }
}
