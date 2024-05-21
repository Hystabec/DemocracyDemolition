using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LauncherTrapScript : MonoBehaviour
{
    [SerializeField]
    GameObject prefabProjectile;

    GameObject spawnedProj;

    GameObject firePosition;
    float projectileRadius = 0.5f;

    [SerializeField]
    AudioSource soundPlayer;

    [SerializeField]
    AudioClip fireSoundClip;

    bool canFire = true;

    [SerializeField]
    float launchForce = 10.0f;

    [SerializeField]
    Animator launcherAnim;

    [SerializeField]
    ParticleSystem smokeEffect;

    private void Awake()
    {
        if (fireSoundClip == null)
            soundPlayer = null;

        firePosition = gameObject.transform.Find("firePosition").gameObject;

        spawnedProj = Instantiate(prefabProjectile);
        spawnedProj.SetActive(false);

        projectileRadius = spawnedProj.GetComponent<CircleCollider2D>().radius;
    }

    private void OnDestroy()
    {
        Destroy(spawnedProj);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //bit scuffed - "reloads" the cannon once the last coconut has despawned
        if(!canFire)
        {
            if (spawnedProj.activeSelf == false)
            {
                canFire = true;
            }
        }
    }

    public void CustomOnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile") && canFire)
        {
            spawnedProj.transform.position = firePosition.transform.position + (firePosition.transform.right * projectileRadius);

            //find the normal of firePosition and apply force in that direction
            spawnedProj.SetActive(true);
            spawnedProj.GetComponent<Rigidbody2D>().velocity = firePosition.transform.right.normalized * launchForce;

            //canFire = false;

            launcherAnim.SetTrigger("Launch");
            smokeEffect.Play();
            soundPlayer?.PlayOneShot(fireSoundClip);

            canFire = false;
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("projectile") && canFire)
        {
            spawnedProj.transform.position = firePosition.transform.position + (firePosition.transform.right * projectileRadius);

            //find the normal of firePosition and apply force in that direction
            spawnedProj.SetActive(true);
            spawnedProj.GetComponent<Rigidbody2D>().velocity = firePosition.transform.right.normalized * launchForce;

            canFire = false;
        }
    }*/
}
