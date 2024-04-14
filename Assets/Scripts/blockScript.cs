using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockScript : MonoBehaviour
{
    [SerializeField]
    int TotalBlockHealth = 3;
    int CurrentBlockHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        CurrentBlockHealth = TotalBlockHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            CurrentBlockHealth--;

            if(CurrentBlockHealth <= 0)
            {
                CurrentBlockHealth = TotalBlockHealth;
                Debug.Log(gameObject.name + " destroyed");
            }
        }
    }
}
