using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   private PolygonCollider2D col;

    private int bounceCount = 0;

    [SerializeField]
    ParticleSystem breakEffect;

    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("coconut collided");
        bounceCount++;
        if (bounceCount >= 3)
        {
            gameObject.SetActive(false);
        }
        
    }
}
