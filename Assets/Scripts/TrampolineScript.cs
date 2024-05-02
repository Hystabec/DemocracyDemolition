using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    public void CustomOnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            if (typeof(PolygonCollider2D) == collision.otherCollider.GetType())
            {
                //Debug.Log("Bounce");
                //play animation
            }
        }
    }
}
