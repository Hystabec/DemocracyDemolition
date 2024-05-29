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

    Vector2 mousePos;
    
    
    void Start()
    {
        points = new GameObject[numberPoints];

        for( int i = 0; i < numberPoints; i++ )
        {
            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
        }


    }

    void Update()
    {
        
    }




}
