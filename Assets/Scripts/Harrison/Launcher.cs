using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Launcher")
        {
            float launch = collision.getComponent<playerScript>().launch;
        }
    }
}
