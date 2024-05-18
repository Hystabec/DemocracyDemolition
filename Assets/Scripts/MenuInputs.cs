using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputs : MonoBehaviour
{
    int requestedPlayerIndex = 0;
    int playerIndex = -1;
    Vector2 leftStickMoveVector = Vector2.zero;

    [SerializeField]
    List<GameObject> selectableMenuItems = new();
    int currentMenuItemIndex = 0;

    bool canSwap = true;
    [SerializeField]
    float timeBetweenSwaps = 0.5f;

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
        if (selectableMenuItems.Count > 0)
            selectableMenuItems[currentMenuItemIndex].GetComponent<GenericUIButton>().ActivateButton();
    }

    public void LS(InputValue val)
    {
        leftStickMoveVector = val.Get<Vector2>();
    }

    public void DPad(Vector2 val)
    {
        leftStickMoveVector += val;
    }

    void handleLeftStick()
    {
        if (leftStickMoveVector.y != 0)
        {
            if(canSwap)
            {
                if (leftStickMoveVector.y > 0)
                {
                    //move up
                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericUIButton>().HideHovered();

                    currentMenuItemIndex--;

                    if (currentMenuItemIndex < 0)
                        currentMenuItemIndex = selectableMenuItems.Count - 1;

                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericUIButton>().ShowHovered();
                }
                else if(leftStickMoveVector.y < 0)
                {
                    //move down
                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericUIButton>().HideHovered();

                    currentMenuItemIndex++;

                    if (currentMenuItemIndex >= selectableMenuItems.Count)
                        currentMenuItemIndex = 0;

                    selectableMenuItems[currentMenuItemIndex].GetComponent<GenericUIButton>().ShowHovered();
                }

                canSwap = false;
                StartCoroutine(waitToSwap());
            }
        }

        if(leftStickMoveVector.x != 0)
        {
            //slide handling goes here
        }
    }

    void FixedUpdate()
    {
        handleLeftStick();
    }

    IEnumerator waitToSwap()
    {
        yield return new WaitForSeconds(timeBetweenSwaps);
        canSwap = true;
    }
}   
