using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pooledProjectileScript : MonoBehaviour
{
    public playerScript owner;

    [SerializeField]
    int maxBounceCount = 3;

    int bounceCount = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCount++;
        if (bounceCount >= maxBounceCount)
        {
            bounceCount = 0;

            if(owner != null) 
                owner.despawnProjectile(this.gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}