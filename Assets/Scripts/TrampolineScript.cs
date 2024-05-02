using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    public void CustomOnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            //reflect projectile
            var projRB = collision.gameObject.GetComponent<Rigidbody2D>();

            var collisionPoint = collision.ClosestPoint(transform.position);
            var collisionNormal = transform.position - new Vector3(collisionPoint.x, collisionPoint.y);

            ReflectProj(projRB, collisionNormal);
            //play animation
        }
    }

    void ReflectProj(Rigidbody2D projectile, Vector2 reflectVector)
    {
        var _velocity = Vector2.Reflect(projectile.velocity, reflectVector);
        projectile.velocity = _velocity;
    }
}
