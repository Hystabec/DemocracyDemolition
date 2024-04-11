using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [SerializeField]
    private GameObject coconutProjectile;

    [SerializeField]
    private Camera mainCamera;

    private Vector3 mousePos;

    void Start()
    {
        
    }

    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float zrotation = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, zrotation - 90);


        if(Input.GetMouseButtonDown(0))
        {
            GameObject proj = Instantiate(coconutProjectile, transform.position, Quaternion.identity);

            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y).normalized * 10;
        }
    }
}
