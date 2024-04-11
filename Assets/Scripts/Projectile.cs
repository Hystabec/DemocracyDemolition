using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   private CircleCollider2D col;

    private int bounceCount = 0;

    void Start()
    {
        col = GetComponent<CircleCollider2D>();
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
