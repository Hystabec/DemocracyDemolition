using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputs : MonoBehaviour
{
    bool hasOwner = false;

    int requestedPlayerIndex = 0;
    int playerIndex = -1;
    Vector2 leftStickMoveVector = Vector2.zero;

    [SerializeField]
    List<GameObject> selectableMenuItems = new();
    int currentMenuItemIndex = 0;

    [SerializeField]
    Animator redCrowdAnim, blueCrowdAnim;

    bool canSwap = true;
    bool canSlide = true;
    bool itemSelected = false;
    [SerializeField]
    float timeBetweenSwaps = 0.5f, timeBetweenSlides = 0.0f;

    public bool GetHasOwner()
    { 
        return hasOwner;
    }
    public void SetHasOwner(bool hasOwner)
    {
        this.hasOwner = hasOwner;
    }

    public int GetRequestedPlayerIndex()
    {
        return requestedPlayerIndex;
    }

    public void SetPlayerIndex(int pIndex)
    {
        playerIndex = pIndex;
    }

    public void A()
    {
        if (!itemSelected && selectableMenuItems.Count > 0)
        {
            var GMI = selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>();
            GMI.Activate();

            if (GMI.GetItemType() == MenuItemType.Slider)
            {
                canSwap = false;
                itemSelected = true;
            }
        }
        else if(itemSelected && selectableMenuItems.Count > 0)
        {
            selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().Deactivate();
            canSwap = true;
            itemSelected = false;
        }
    }

    public void B()
    {
        if(itemSelected && selectableMenuItems.Count > 0)
        {
            selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().Deactivate();
            canSwap = true;
            itemSelected = false;
        }
    }

    public void LS(InputValue val)
    {
        leftStickMoveVector = val.Get<Vector2>();
    }

    public void FakeLS(Vector2 val)
    {
        leftStickMoveVector = val;
    }

    public void DPad(Vector2 val)
    {
        leftStickMoveVector += val;
    }

    void handleLeftStick()
    {
        if (leftStickMoveVector.y > 0.1f || leftStickMoveVector.y < -0.1f)
        {
            if(canSwap && !itemSelected)
            {
                if (leftStickMoveVector.y > 0)
                {
                    //move up
                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().HideHovered();

                    currentMenuItemIndex--;

                    if (currentMenuItemIndex < 0)
                        currentMenuItemIndex = selectableMenuItems.Count - 1;



                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().ShowHovered();
                }
                else if(leftStickMoveVector.y < 0)
                {
                    //move down
                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().HideHovered();

                    currentMenuItemIndex++;

                    if (currentMenuItemIndex >= selectableMenuItems.Count)
                        currentMenuItemIndex = 0;

                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().ShowHovered();
                }
                if (currentMenuItemIndex == 0)
                {
                    redCrowdAnim.SetTrigger("GameWin");
                    blueCrowdAnim.SetTrigger("GameWin");
                }
                if (currentMenuItemIndex > 0)
                {
                    redCrowdAnim.SetTrigger("Idle");
                    blueCrowdAnim.SetTrigger("Idle");
                }

                canSwap = false;
                StartCoroutine(waitToSwap());
            }
        }

        if(leftStickMoveVector.x > 0.1f || leftStickMoveVector.x < -0.1f)
        {
            //slide handling goes here
            if (canSlide)
            {
                selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().GiveStickValues(leftStickMoveVector);
                canSlide = false;
                StartCoroutine(waitToSlide());
            }
        }
    }

    void Start()
    {
        selectableMenuItems[currentMenuItemIndex].GetComponent<GenericMenuItem>().ShowHovered();
        
        if (currentMenuItemIndex == 0)
        {
            redCrowdAnim.SetTrigger("GameWin");
            blueCrowdAnim.SetTrigger("GameWin");
        }
    }

    void Update()
    {
        handleLeftStick();
    }

    IEnumerator waitToSwap()
    {
        yield return new WaitForSeconds(timeBetweenSwaps);
        canSwap = true;
    }

    IEnumerator waitToSlide()
    {
        yield return new WaitForSeconds(timeBetweenSlides);
        canSlide = true;
    }
}   
