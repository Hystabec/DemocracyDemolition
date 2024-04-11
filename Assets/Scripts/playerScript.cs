using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    int SelectedIndex = 0;
    List<GameObject> selectableObjects = new List<GameObject>(4);

    public Vector3 locationOne;
    public Vector3 locationTwo;
    public Vector3 locationThree;

    bool locationOneFree = true;
    bool locationTwoFree = true;
    bool locationThreeFree = true;

    public GameObject tempObject1;
    public GameObject tempObject2;
    public GameObject tempObject3;

    Color previousCol;

    bool hasBeenSelected = false;

    Vector3 selectedBlockLocation = new Vector3(0, 0, 0);
    Quaternion selectedBlockRotation = new Quaternion(0, 0, 0, 0);

    Vector2 leftStickMoveVector = new Vector2(0,0);

    float StickSensitivity = 10.0f;
    float timeBetweenSwaps = 0.2f;
    bool canSwap = true;

    public void AddBlockToList(GameObject blockToAdd)
    {
        selectableObjects.Add(blockToAdd);

        //put the block in place - and sets locations to false
        if(locationOneFree)
        {
            blockToAdd.transform.position = locationOne;
            locationOneFree = false;
            return;
        }

        if(locationTwoFree)
        {
            blockToAdd.transform.position = locationTwo;
            locationTwoFree = false;
            return;
        }

        if(locationThreeFree)
        {
            blockToAdd.transform.position = locationThree;
            locationThreeFree = false;
            return;
        }
    }

    public void OnA()
    {
        if (!(selectableObjects.Count > 0))
            return;

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

            SelectedIndex = 0;

            if (selectableObjects.Count != 0)
            {
                SpriteRenderer sr = selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>();
                previousCol = sr.color;
                sr.color = Color.black;
            }

            hasBeenSelected = false;
        }
    }

    public void OnB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        if (hasBeenSelected)
        {
            selectableObjects[SelectedIndex].transform.position = selectedBlockLocation;
            selectableObjects[SelectedIndex].transform.rotation = selectedBlockRotation;

            hasBeenSelected = false;
        }
    }

    public void OnRB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90));
    }

    public void OnLB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90));
    }

    public void OnLS(InputValue value)
    {
        leftStickMoveVector = value.Get<Vector2>();
    }

    private IEnumerator waitToSwap()
    {
        yield return new WaitForSeconds(timeBetweenSwaps);
        canSwap = true;
    }

    void handleLeftStick()
    {
        if (!(selectableObjects.Count > 0))
            return;


        if (!hasBeenSelected)
        {
            if (canSwap)
            {
                if (leftStickMoveVector.y != 0 && leftStickMoveVector.y > 0)
                {
                    selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

                    SelectedIndex--;

                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = selectableObjects.Count-1;
                    }

                    SpriteRenderer sr = selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>();

                    previousCol = sr.color; //save old color
                    sr.color = Color.black; //change colour of selected object

                    canSwap = false;

                    StartCoroutine(waitToSwap());
                }
                else if (leftStickMoveVector.y != 0 && leftStickMoveVector.y < 0)
                {
                    selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

                    SelectedIndex++;

                    if (SelectedIndex >= selectableObjects.Count)
                    {
                        SelectedIndex = 0;
                    }

                    SpriteRenderer sr = selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>();

                    previousCol = sr.color; //save old color
                    sr.color = Color.black; //change colour of selected object

                    canSwap = false;

                    StartCoroutine(waitToSwap());
                }
            }
        }
        else
        {
            selectableObjects[SelectedIndex].transform.position += (new Vector3(StickSensitivity*leftStickMoveVector.x, StickSensitivity*leftStickMoveVector.y, 0))*Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = tempObject1.GetComponent<SpriteRenderer>();

        previousCol = sr.color;
        sr.color = Color.black;

        //populates the arrray with the gameObjects
        AddBlockToList(tempObject1);
        AddBlockToList(tempObject2);
        AddBlockToList(tempObject3);
    }

    // Update is called once per frame
    void Update()
    {
        handleLeftStick();   
    }
}
