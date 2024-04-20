using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class playerScript : MonoBehaviour
{
    [SerializeField]
    int totalAmmo = 3;
    int RemainingAmmo = 3;

    [SerializeField]
    int thisPlayerIndex = 0;

    int SelectedIndex = 0;
    List<GameObject> selectableObjects = new List<GameObject>();

    public int selectableObjectsNumber = 0;

    [SerializeField]
   // public GameObject tempObject1, tempObject2, tempObject3;

    public List<GameObject> blockObjects;

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

    private bool canPlace = true;

    bool playMode = false;

    float timeBetweenBothPlayersJoiningAndInputsStarting = 0.2f;

    public void OnRoundStart()
    {
        //called by the InGameManagerScript at the start of the round when blocks have been added - should probably do through unity events

        SelectedIndex = 0;
        selectableObjects[0].GetComponent<blockScript>().ShowOutline(true);
    }

    public void setAmmo(int amount)
    {
        totalAmmo = amount;
        RemainingAmmo = totalAmmo;
    }

    public void resetAmmo()
    {
        RemainingAmmo = totalAmmo;
    }

    public int getPlayerIndex()
    {
        return thisPlayerIndex;
    }

    public void switchMode()
    {
        StartCoroutine(SwitchEndOfFrame());
    }

    IEnumerator SwitchEndOfFrame() 
    {
        yield return new WaitForSeconds(timeBetweenBothPlayersJoiningAndInputsStarting);
        playMode = !playMode;
    }

    public void despawnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projectList.Add(projectile);
    }

    public void AddBlockToList(GameObject blockToAdd)
    {
        selectableObjects.Add(blockToAdd);
        selectableObjectsNumber++;

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

    public void clearAndDeleteBlockList()
    {
        foreach (GameObject go in selectableObjects)
            Destroy(go);

        selectableObjects.Clear();

        locationOneFree = true;
        locationTwoFree = true;
        locationThreeFree = true;
    }

    public void A()
    {
        if (!(selectableObjects.Count > 0))
            return;

        if (!playMode)
            return;

        if (!hasBeenSelected)
        {
            hasBeenSelected = true;
            selectedBlockLocation = selectableObjects[SelectedIndex].transform.position;
            selectedBlockRotation = selectableObjects[SelectedIndex].transform.rotation;
        }
        else if (canPlace)
        {
            //remvoe the selected object from the list - it has been placed
            selectableObjects[SelectedIndex].GetComponent<blockScript>().ShowOutline(false);

            selectableObjects.RemoveAt(SelectedIndex);

            SelectedIndex = 0;

            if (selectableObjects.Count != 0)
            {
                selectableObjects[SelectedIndex].GetComponent<blockScript>().ShowOutline(true);

            }

            hasBeenSelected = false;
        }
    }

    public void B()
    {
        if (!(selectableObjects.Count > 0))
            return;

        if (!playMode)
            return;

        if (hasBeenSelected)
        {
            selectableObjects[SelectedIndex].transform.position = selectedBlockLocation;
            selectableObjects[SelectedIndex].transform.rotation = selectedBlockRotation;

            hasBeenSelected = false;
        }
    }

    public void RB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        if (!playMode)
            return;

        selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90));
    }

    public void LB()
    {
        if (!(selectableObjects.Count > 0))
            return;

        if (!playMode)
            return;

        selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, 90));
    }

    public void fakeLS(Vector2 value)
    {
        //used for PC control override
        leftStickMoveVector = value;
    }

    public void LS(InputValue value)
    {
        leftStickMoveVector = value.Get<Vector2>();
    }

    public void RS(InputValue value)
    {
        rightStickMoveVector = value.Get<Vector2>();
    }

    public void RT()
    {
        //shoot projectile
        if (projectList.Count <= 0 || RemainingAmmo <= 0)
            return; //projects are empty - do nothing

        if (!playMode)
            return;

        GameObject proj = projectList[0];
        proj.transform.position = fireMarker.transform.GetChild(0).transform.position;
        proj.SetActive(true);

        Vector3 rotation = fireMarker.transform.GetChild(0).transform.position - fireMarker.transform.position;

        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y).normalized * throwForce;
        projectList.Remove(proj);

        RemainingAmmo--;
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

        if (!playMode)
            return;

        //this could probably be done better, but oh well it works :)
        if (!hasBeenSelected)
        {
            if (canSwap)
            {
                if (leftStickMoveVector.y != 0 && leftStickMoveVector.y > 0)
                {
                    selectableObjects[SelectedIndex].GetComponent<blockScript>().ShowOutline(false);

                  //  selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

                    SelectedIndex--;

                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = selectableObjects.Count-1;
                    }

                    selectableObjects[SelectedIndex].GetComponent<blockScript>().ShowOutline(true);

                    canSwap = false;

                    StartCoroutine(waitToSwap());
                }
                else if (leftStickMoveVector.y != 0 && leftStickMoveVector.y < 0)
                {
                    selectableObjects[SelectedIndex].GetComponent<blockScript>().ShowOutline(false);

                  //  selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

                    SelectedIndex++;

                    if (SelectedIndex >= selectableObjects.Count)
                    {
                        SelectedIndex = 0;
                    }

                    selectableObjects[SelectedIndex].GetComponent<blockScript>().ShowOutline(true);


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
        if (!playMode)
            return;

        FireMarkerMoveVector = (Vector3.up*rightStickMoveVector.x + Vector3.left*rightStickMoveVector.y);

        if(rightStickMoveVector.x != 0 || rightStickMoveVector.y != 0)
            fireMarker.transform.rotation = quaternion.LookRotation(Vector3.forward, FireMarkerMoveVector);
    }

    public void AddBlocks(GameObject obj)
    {
        blockObjects.Add(obj);
        AddBlockToList(obj);

    }
    public void CanPlaceBlock(bool canPlaceBlock)
    {
        canPlace = canPlaceBlock;
    }

    void Start()
    {
        // SpriteRenderer sr = tempObject1.GetComponent<SpriteRenderer>();

        //   previousCol = sr.color;
        //   sr.color = Color.black;

        //populates the arrray with the gameObjects
        //AddBlockToList(tempObject1);
        // AddBlockToList(tempObject2);
        // AddBlockToList(tempObject3);

        RemainingAmmo = totalAmmo;

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

    void Update()
    {
        handleLeftStick();
        handleRightStick();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            FindObjectOfType<InGameManagerScript>().playerKilled(this.gameObject);
        }
    }
}
