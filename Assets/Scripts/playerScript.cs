using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class playerScript : MonoBehaviour
{
    int SelectedIndex = 0;
    List<GameObject> selectableObjects = new List<GameObject>();

    [SerializeField]
    public GameObject tempObject1, tempObject2, tempObject3;

    [SerializeField]
    Vector3 locationOne, locationTwo, locationThree;

    [SerializeField]
    GameObject fireMarker;

    bool locationOneFree = true;
    bool locationTwoFree = true;
    bool locationThreeFree = true;

    Color previousCol;

    bool hasBeenSelected = false;

    Vector3 selectedBlockLocation = Vector3.zero;
    Quaternion selectedBlockRotation = new Quaternion(0, 0, 0, 0);

    Vector2 leftStickMoveVector = Vector2.zero;
    Vector2 rightStickMoveVector = Vector2.zero;

    float leftStickSensitivity = 10.0f;

    Vector3 FireMarkerMoveVector = Vector3.zero;

    float timeBetweenSwaps = 0.2f;
    bool canSwap = true;

    [SerializeField]
    GameObject projectile;

    
    int numOfProjectiles = 10;
    List<GameObject> projectList;

    [SerializeField]
    float throwForce = 10.0f;

    public void despawnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projectList.Add(projectile);
    }

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

    public void clearBlockList()
    {
        selectableObjects.Clear();

        locationOneFree = true;
        locationTwoFree = true;
        locationThreeFree = true;
    }

    void OnA()
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

    void OnB()
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

    void OnRB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90));
    }

    void OnLB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90));
    }

    void OnLS(InputValue value)
    {
        leftStickMoveVector = value.Get<Vector2>();
    }

    void OnRS(InputValue value)
    {
        rightStickMoveVector = value.Get<Vector2>();
    }

    void OnRT()
    {
        //shoot projectile
        if (projectList.Count <= 0)
            return; //projects are empty - do nothing

        GameObject proj = projectList[0];
        proj.transform.position = fireMarker.transform.GetChild(0).transform.position;
        proj.SetActive(true);

        Vector3 rotation = fireMarker.transform.GetChild(0).transform.position - fireMarker.transform.position;

        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y).normalized * throwForce;
        projectList.Remove(proj);
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

        //this could probably be done better, but oh well it works :)
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
            selectableObjects[SelectedIndex].transform.position += (new Vector3(leftStickSensitivity*leftStickMoveVector.x, leftStickSensitivity*leftStickMoveVector.y, 0))*Time.deltaTime;
        }
    }

    void handleRightStick()
    {
        FireMarkerMoveVector = (Vector3.up*rightStickMoveVector.x + Vector3.left*rightStickMoveVector.y);

        if(rightStickMoveVector.x != 0 || rightStickMoveVector.y != 0)
            fireMarker.transform.rotation = quaternion.LookRotation(Vector3.forward, FireMarkerMoveVector);
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

        projectList = new List<GameObject>(numOfProjectiles);

        GameObject temp;
        for(int i = 0; i < numOfProjectiles; i++)
        {
            temp = Instantiate(projectile);
            temp.transform.SetParent(this.transform);
            temp.GetComponent<pooledProjectileScript>().owner = this;
            projectList.Add(temp);
            temp.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleLeftStick();
        handleRightStick();
    }
}
