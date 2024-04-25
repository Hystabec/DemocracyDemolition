using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Trampoline : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector3 lastVelocity;
    public GameObject trampoline;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        lastVelocity = rb.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            var rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            var speed = rigidbody.velocity.magnitude;
            var direction = Vector3.Reflect(rigidbody.velocity.normalized, collision.gameObject.transform.forward);
            rigidbody.velocity = direction * Mathf.Max(speed, 0f);
            Debug.Log("trampoline coll");
        }
    }
}