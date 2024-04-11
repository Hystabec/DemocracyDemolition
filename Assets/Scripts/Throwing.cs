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

    [SerializeField]
    private List<GameObject> avaialableProjectiles;

    private int thrownCount = 0;

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
            if (thrownCount < 3)
            {
                GameObject proj = avaialableProjectiles[0];
                avaialableProjectiles.Remove(avaialableProjectiles[0]);
                proj.transform.position = transform.position;
                proj.SetActive(true);

                proj.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y).normalized * 10;
                thrownCount++;
            }
        }
    }
}
