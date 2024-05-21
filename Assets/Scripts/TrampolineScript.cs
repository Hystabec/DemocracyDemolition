using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    [SerializeField]
    Animator trampolineAnim;
    public AudioSource soundPlayer;
    public void CustomOnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            if (typeof(PolygonCollider2D) == collision.otherCollider.GetType())
            {
                trampolineAnim.SetTrigger("Bounce");
                //Debug.Log("Bounce");
                //play animation
                soundPlayer.Play();
            }
        }
    }
}
