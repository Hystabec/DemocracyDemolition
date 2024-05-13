using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using System;

public enum modes
{
    UI,
    Play,
    None
}

public class playerScript : MonoBehaviour
{
    modes currentMode = modes.None;

    [SerializeField]
    int totalAmmo = 3;
    int RemainingAmmo = 3;

    [SerializeField]
    int thisPlayerIndex = 0;

    int assignedControllerIndex = -1;

    int UIElementIndex = 0;
    List<GameObject> listOfUIElements = new List<GameObject>();

    int SelectedIndex = 0;
    List<GameObject> selectableObjects = new List<GameObject>();

    public int selectableObjectsNumber = 0;

    public List<GameObject> blockObjects;

    List<GameObject> placedBlocks = new();

    [SerializeField]
    Vector3 locationOne, locationTwo, locationThree;

    [SerializeField]
    GameObject fireMarker, rotationMarker;

    Vector3 rotationMarkerOffset = new Vector3(0, 0.5f, 0);
    Vector3 defaultRotationMarkerPosition = new Vector3(-10, -10, 0);

    bool locationOneFree = true;
    bool locationTwoFree = true;
    bool locationThreeFree = true;

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

    [SerializeField]
    private SpriteRenderer projInHand;

    
    int numOfProjectiles = 10;
    List<GameObject> projectList;


    public List<GameObject> thrownProjectiles;

    [SerializeField]
    float throwForce = 10.0f;


    private bool fightingStage = false;

    float timeBetweenBothPlayersJoiningAndInputsStarting = 0.2f;

    public Animator anim, rotationAnim;

    [SerializeField]
    TextMeshProUGUI currentAmmoText;

    private AudioSource playerThrowSound;

    //angles for clamping controller input
    [SerializeField]
    float MinAngle, MaxAngle;

    [SerializeField]
    float throwCooldown = 1.5f;

    private bool canThrow = true;

    [SerializeField]
    private GameObject cantThrowIcon;

    [SerializeField]
    private UnityEngine.UI.Image cooldownbar;

    private bool onCooldown;

    public bool gameEnded;

    float moveTime = 0;

    public void ResetData()
    {
        //this should be called by "InGameManagerScript"
        
        currentMode = modes.None;
        RemainingAmmo = totalAmmo;
        SelectedIndex = 0;

        foreach(GameObject go in selectableObjects)
        {
            Destroy(go);
        }
        selectableObjects.Clear();

        foreach(GameObject go in blockObjects)
        {
            Destroy(go);
        }
        blockObjects.Clear();

        foreach(GameObject go in placedBlocks)
        {
            Destroy(go);
        }

        ClearUIElements();

        placedBlocks.Clear();

        selectableObjectsNumber = 0;

        locationOneFree = true;
        locationTwoFree = true;
        locationThreeFree = true;
        hasBeenSelected = false;

        selectedBlockLocation = Vector3.zero;
        selectedBlockRotation = new Quaternion(0, 0, 0, 0);

        leftStickMoveVector = Vector2.zero;
        rightStickMoveVector = Vector2.zero;

        FireMarkerMoveVector = Vector3.zero;

        canSwap = true;
    }

    public void OnRoundStart()
    {
        //called by the InGameManagerScript at the start of the round when blocks have been added - should probably do through unity events
        gameEnded = false;

        SelectedIndex = 0;
        selectableObjects[0].GetComponent<GenericBlockScript>().ShowOutline(true);
    }

    public void setAmmo(int amount)
    {
        totalAmmo = amount;
        RemainingAmmo = totalAmmo;
        updateAmmoText();
    }

    public void resetAmmo()
    {
        RemainingAmmo = totalAmmo;
        updateAmmoText();
    }

    public void SetAssignedControllerIndex(int index)
    {
        assignedControllerIndex = index;
    }

    public int GetAssignedControllerIndex()
    {
        return assignedControllerIndex;
    }

    public int getPlayerIndex()
    {
        return thisPlayerIndex;
    }

    //public void switchMode(modes newMode)
    //{
    //    StartCoroutine(SwitchEndOfFrame(newMode));
    //}

    public void SwitchToNoMode()
    {
        StartCoroutine(SwitchEndOfFrame(modes.None));
    }

    public void SwitchToUIMode()
    {
        StartCoroutine(SwitchEndOfFrame(modes.UI));
    }

    public void SwitchToPlayMode()
    {
        StartCoroutine(SwitchEndOfFrame(modes.Play));
    }

    IEnumerator SwitchEndOfFrame(modes newMode) 
    {
        yield return new WaitForSeconds(timeBetweenBothPlayersJoiningAndInputsStarting);
        currentMode = newMode;
    }

    public void despawnProjectile(GameObject projectile)
    {
        if (projectile != null)
        {
            projectile.SetActive(false);
            projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            projectList.Add(projectile);
            // thrownProjectiles.Remove(projectile);
        }

    }

    public void GiveUIElements(List<GameObject> UIList)
    {
        listOfUIElements = UIList;
        UIElementIndex = 0;
    }

    public void ClearUIElements()
    {
        listOfUIElements.Clear();
        UIElementIndex = 0;
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
        if (hasBeenSelected)
        {
            selectableObjects[SelectedIndex].transform.position = selectedBlockLocation;
            selectableObjects[SelectedIndex].transform.rotation = selectedBlockRotation;

            hasBeenSelected = false;
        }

        foreach (GameObject go in selectableObjects)
            Destroy(go);

        selectableObjects.Clear();
        locationOneFree = true;
        locationTwoFree = true;
        locationThreeFree = true;

        rotationMarker.transform.position = defaultRotationMarkerPosition;
    }

    public void ProjInHandVisible(bool show)
    {
        projInHand.enabled = show;  
    }

    public void A()
    {
        //if (!(selectableObjects.Count > 0))
        //    return;

        if(currentMode == modes.UI)
        {
            if(listOfUIElements.Count > 0)
            {
                listOfUIElements[UIElementIndex].GetComponent<GenericUIButton>().ActivateButton();
            }
        }
        else if (currentMode == modes.Play)
        {
            if (!hasBeenSelected)
            {
                hasBeenSelected = true;
                selectedBlockLocation = selectableObjects[SelectedIndex].transform.position;
                selectedBlockRotation = selectableObjects[SelectedIndex].transform.rotation;
                selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().CurrentlyPlacing(true);
            }
            else if (selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().CanPlaceBlock())
            {
                //remvoe the selected object from the list - it has been placed
                selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().ShowOutline(false);
                selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().Placed();

                placedBlocks.Add(selectableObjects[SelectedIndex]);

                selectableObjects.RemoveAt(SelectedIndex);

                rotationMarker.transform.position = defaultRotationMarkerPosition;

                SelectedIndex = 0;

                if (selectableObjects.Count != 0)
                {
                    selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().ShowOutline(true);
                }

                hasBeenSelected = false;
            }
        }
    }

    public void B()
    {
        if (!(selectableObjects.Count > 0))
            return;

        if (currentMode == modes.UI)
        {

        }
        else if (currentMode == modes.Play)
        {
            if (hasBeenSelected)
            {
                selectableObjects[SelectedIndex].transform.position = selectedBlockLocation;
                selectableObjects[SelectedIndex].transform.rotation = selectedBlockRotation;
                selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().CurrentlyPlacing(false);
                rotationMarker.transform.position = defaultRotationMarkerPosition;

                hasBeenSelected = false;
            }
        }
    }

    public void RB()
    {
        if (!hasBeenSelected)
            return;

        if (!(selectableObjects.Count > 0))
            return;

        if (currentMode == modes.UI)
        {

        }
        else if (currentMode == modes.Play)
        {
            selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, -selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().GetRotationAmount()));
            rotationAnim.SetTrigger("Right");
        }
    }

    public void LB()
    {
        if (!hasBeenSelected)
            return;

        if (!(selectableObjects.Count > 0))
            return;

        if (currentMode == modes.UI)
        {

        }
        else if (currentMode == modes.Play)
        {
            selectableObjects[SelectedIndex].transform.Rotate(new Vector3(0, 0, selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>().GetRotationAmount()));
            rotationAnim.SetTrigger("Left");
        }
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
    public void DPad(Vector2 val)
    {
        leftStickMoveVector += val;
    }

    public void fakeRS(Vector2 value)
    {
        rightStickMoveVector = value;
    }

    public void RS(InputValue value)
    {
        rightStickMoveVector = value.Get<Vector2>();
    }

    

    public void RT()
    {
        if (!fightingStage)
            return;

        //shoot projectile
        if (projectList.Count <= 0 || RemainingAmmo <= 0)
            return; //projects are empty - do nothing

        if (currentMode == modes.UI)
        {

        }
        else if (currentMode == modes.Play)
        {
            if (canThrow)
            {
                GameObject proj = projectList[0];
                proj.transform.position = fireMarker.transform.GetChild(0).transform.position;
                proj.SetActive(true);
                thrownProjectiles.Add(proj);

                ProjInHandVisible(false);

                anim.SetTrigger("Throw");
                playerThrowSound.Play();

                Vector3 rotation = fireMarker.transform.GetChild(0).transform.position - fireMarker.transform.position;

                proj.GetComponent<Rigidbody2D>().velocity = new Vector2(rotation.x, rotation.y).normalized * throwForce;
                projectList.Remove(proj);

                RemainingAmmo--;
                updateAmmoText();

                StartCoroutine(ThrowCooldown());
            


            }
        }
    }
    private IEnumerator ThrowCooldown()
    {
        onCooldown = true;
        canThrow = false;
        cantThrowIcon.SetActive(true);
        yield return new WaitForSeconds(throwCooldown);
        if (RemainingAmmo > 0)
        {
            if (!gameEnded)
            {
                ProjInHandVisible(true);
            }
        }
        canThrow = true;
        cantThrowIcon.SetActive(false);
        onCooldown = false;

    }

    void CooldownBar() 
    {
        if (onCooldown) 
        {
            moveTime += Time.deltaTime * 0.1f;

            cooldownbar.fillAmount = Mathf.Lerp(cooldownbar.fillAmount, 1, moveTime);
        }

        if(!onCooldown)
        {
            if(cooldownbar.fillAmount > 0) 
            {

                cooldownbar.fillAmount = 0;
                moveTime = 0;
            }

        }
    }

    public void CanFight(bool canFight)
    {
        fightingStage = canFight;
    }

    public void updateAmmoText()
    {
        currentAmmoText.text = RemainingAmmo + "/3";
    }

    private IEnumerator waitToSwap()
    {
        yield return new WaitForSeconds(timeBetweenSwaps);
        canSwap = true;
    }

    /*
    private IEnumerator WaitToShowProj()
    {
        yield return new WaitForSeconds(0.9f);
       
        if (projInHand.enabled == false)
        {
            ProjInHandVisible(true);
            //Debug.Log(gameObject);
        }

    }
    */


    void handleLeftStick()
    {
        //if (!(selectableObjects.Count > 0))
        //    return;

        if (currentMode == modes.UI)
        {
            if(listOfUIElements.Count > 0)
            {
                if(canSwap)
                {
                    if (leftStickMoveVector.x != 0 && leftStickMoveVector.x > 0)
                    {
                        UIElementIndex--;

                        if(UIElementIndex < 0)
                            UIElementIndex = listOfUIElements.Count - 1;

                        canSwap = false;

                        StartCoroutine(waitToSwap());
                    }
                    else if (leftStickMoveVector.x != 0 && leftStickMoveVector.x < 0)
                    {

                        UIElementIndex++;

                        if (UIElementIndex >= listOfUIElements.Count)
                            UIElementIndex = 0;

                        canSwap = false;

                        StartCoroutine(waitToSwap());
                    }
                }
            }
        }
        else if (currentMode == modes.Play)
        {
            //this could probably be done better, but oh well it works :)
            if (!hasBeenSelected)
            {
                if (canSwap)
                {
                    if (leftStickMoveVector.x != 0 && leftStickMoveVector.x > 0)
                    {
                        GenericBlockScript bs = selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>();
                        bs.ShowOutline(false);
                        bs.Deselected();

                        //selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

                        SelectedIndex--;

                        if (SelectedIndex < 0)
                        {
                            SelectedIndex = selectableObjects.Count - 1;
                        }

                        bs = selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>();
                        bs.ShowOutline(true);
                        bs.Selected();

                        canSwap = false;

                        StartCoroutine(waitToSwap());
                    }
                    else if (leftStickMoveVector.x != 0 && leftStickMoveVector.x < 0)
                    {
                        GenericBlockScript bs = selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>();
                        bs.ShowOutline(false);
                        bs.Deselected();

                        //  selectableObjects[SelectedIndex].GetComponent<SpriteRenderer>().color = previousCol; //resets the color

                        SelectedIndex++;

                        if (SelectedIndex >= selectableObjects.Count)
                        {
                            SelectedIndex = 0;
                        }

                        bs = selectableObjects[SelectedIndex].GetComponent<GenericBlockScript>();
                        bs.ShowOutline(true);
                        bs.Selected();


                        canSwap = false;

                        StartCoroutine(waitToSwap());
                    }
                }
            }
            else
            {
                selectableObjects[SelectedIndex].transform.position += (new Vector3(leftStickSensitivity * leftStickMoveVector.x, leftStickSensitivity * leftStickMoveVector.y, 0)) * Time.deltaTime;
                rotationMarker.transform.position = selectableObjects[SelectedIndex].transform.position + rotationMarkerOffset;
            }
        }
    }

    void handleRightStick()
    {
        if (currentMode == modes.UI)
        {

        }
        else if (currentMode == modes.Play)
        {
            if (rightStickMoveVector.x != 0 || rightStickMoveVector.y != 0)
            {
                FireMarkerMoveVector = (Vector3.up * rightStickMoveVector.x + Vector3.left * rightStickMoveVector.y);

                Quaternion rot = quaternion.LookRotation(Vector3.forward, FireMarkerMoveVector);

                var temp = Mathf.Atan2(FireMarkerMoveVector.y - 0, FireMarkerMoveVector.x - 0);

                if ((temp >= MinAngle*Mathf.Deg2Rad) && (temp <= MaxAngle * Mathf.Deg2Rad))
                {
                    //clammping
                    fireMarker.transform.rotation = rot;
                }
            }
        }
    }

    public void AddBlocks(GameObject obj)
    {
        blockObjects.Add(obj);
        AddBlockToList(obj);
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
        playerThrowSound = GetComponent<AudioSource>();
        RemainingAmmo = totalAmmo;

        updateAmmoText();

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

    void FixedUpdate()
    {
        handleLeftStick();
        handleRightStick();
        CooldownBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
                      
            if (collision.gameObject.GetComponent<pooledProjectileScript>().owner != this)
            {
                FindObjectOfType<InGameManagerScript>().playerKilled(this.gameObject);

            }

            else
            {
                Debug.Log("Own projectile");
            }
        }
    }
}
