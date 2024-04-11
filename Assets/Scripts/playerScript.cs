using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    int SelectedIndex = 0;
    List<GameObject> selectableObjects = new List<GameObject>(4);

    public GameObject tempObject1;
    public GameObject tempObject2;
    public GameObject tempObject3;
    public GameObject tempObject4;

    Color previousCol;

    bool hasBeenSelected = false;

    Vector3 selectedBlockLocation = new Vector3(0, 0, 0);
    Quaternion selectedBlockRotation = new Quaternion(0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = tempObject1.GetComponent<SpriteRenderer>();

        previousCol = sr.color;
        sr.color = Color.black;


        //populates the arrray with the gameObjects
        selectableObjects.Add(tempObject1);
        selectableObjects.Add(tempObject2);
        selectableObjects.Add(tempObject3);
        selectableObjects.Add(tempObject4);
    }

    // Update is called once per frame
    void Update()
    {
        if (!(selectableObjects.Count > 0))
            return;
        
        if (hasBeenSelected)
        {
            if (Input.GetKey(KeyCode.W))
            {
                //leftstick up

                selectableObjects[SelectedIndex].transform.position += new Vector3(0,0.1f,0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                //leftstick down

                selectableObjects[SelectedIndex].transform.position += new Vector3(0, -0.1f, 0);
            }

            if (Input.GetKey(KeyCode.A))
            {
                //leftstick left

                selectableObjects[SelectedIndex].transform.position += new Vector3(-0.1f, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                //leftstick right

                selectableObjects[SelectedIndex].transform.position += new Vector3(0.1f, 0, 0);
            }

            if(Input.GetKeyUp(KeyCode.Q))
            {
                //leftbumper

                selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90)); 
            }

            if(Input.GetKeyUp(KeyCode.E))
            {
                //rightbumper

                selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, -90));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //this will mimic A being pressed
            if (!hasBeenSelected)
            {
                hasBeenSelected = true;
                selectedBlockLocation = selectableObjects[SelectedIndex].transform.position;
                selectedBlockRotation = selectableObjects[SelectedIndex].transform.rotation;
            }
            else
            {
                //remvoe the selected object from the list - it has been placed
                selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol;

                selectableObjects.RemoveAt(SelectedIndex);

                SelectedIndex=0;

                if (selectableObjects.Count != 0)
                {
                    SpriteRenderer sr = selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>();
                    previousCol = sr.color;
                    sr.color = Color.black;
                }

                hasBeenSelected = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.X) && hasBeenSelected)
        {
            //this will mimic B being pressed
            selectableObjects[SelectedIndex].transform.position = selectedBlockLocation;
            selectableObjects[SelectedIndex].transform.rotation = selectedBlockRotation;

            hasBeenSelected = false;
        }

        if(Input.GetKeyUp(KeyCode.S) && !hasBeenSelected)   //old system, will need to use new system
        {
            //this will mimic the L stick down

            selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

            SelectedIndex++;

            if(SelectedIndex >= selectableObjects.Count)
            {
                SelectedIndex = 0;
            }

            SpriteRenderer sr = selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>();

            previousCol = sr.color; //save old color
            sr.color = Color.black; //change colour of selected object
        }  
    }
}
