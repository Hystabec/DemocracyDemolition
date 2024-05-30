using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField]
    private GameObject pointPrefab;

    private GameObject[] points;

    [SerializeField]
    private int numberPoints;

    [SerializeField]
    private float force;
    [SerializeField]
    float lengthOfTrej= 2.0f;

    private Vector2 direction;

    [SerializeField]
    private playerScript pScript;

    [SerializeField]
    private GameObject fireCentre;
    
    void Start()
    {
        force = pScript.GetThrowForce();

        points = new GameObject[numberPoints];

        for( int i = 0; i < numberPoints; i++ )
        {
            if (i == 0)
                continue;

            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
        }

    }

    void Update()
    {
        
        for(int i =0; i < points.Length; i++)
        {
            if (i == 0)
                continue;

            points[i].transform.position = PointPosition(i * 0.1f);
        }
        direction = fireCentre.transform.right;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)transform.position + (direction.normalized * (force) * (t /lengthOfTrej)) + 0.5f * Physics2D.gravity * ((t/lengthOfTrej) * (t/ lengthOfTrej));

        return currentPointPos;
    }

    public void SetInvisible()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i == 0)
                continue;

            points[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void SetVisible()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i == 0)
                continue;

            points[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }




}
