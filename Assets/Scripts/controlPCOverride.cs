using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controlPCOverride : MonoBehaviour
{
    [SerializeField]
    GameObject stickVisualArea;

    [SerializeField]
    InGameManagerScript igm;
    [SerializeField]
    playerScript pScript;
    [SerializeField]
    Camera mainCam;
    Vector3 mousePos;
    [SerializeField]
    GameObject fireMarker;

    [SerializeField]
    float posMinAngle, posMaxAngle, negMinAngle, negMaxAngle;

    static Vector2 leftStick = Vector2.zero;

    [SerializeField]
    Vector2 rightStickVisualMarkerLcation;

    Vector2 centreStick;
    Vector2 rightStickScreenPos;
    Vector2 lastRightStickScreenPos;
    float stickRadius;

    private void Start()
    {
        pScript = gameObject.GetComponent<playerScript>();

        //pScript.PCOverride = true;
        Debug.Log("PC control override");

        pScript.SwitchToPlayMode();
        igm.pcControlOverride();

        rightStickScreenPos = mainCam.WorldToScreenPoint(rightStickVisualMarkerLcation);
        lastRightStickScreenPos = rightStickScreenPos;
        centreStick = rightStickScreenPos;

        var temp = Instantiate(stickVisualArea, new Vector3(rightStickVisualMarkerLcation.x, rightStickVisualMarkerLcation.y, 0), new());
        Mouse.current.WarpCursorPosition(rightStickScreenPos);

        stickRadius = temp.GetComponent<CircleCollider2D>().radius*temp.transform.localScale.x;
    }

    private void Update()
    {
        if (pScript == null)
            return;

        rightStickScreenPos = Input.mousePosition;

        if (Input.GetKeyUp(KeyCode.F))
        {
            //A
            pScript.A();
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            //B
            pScript.B();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            //LB
            pScript.LB();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            //RB
            pScript.RB();
        }

        if (Input.GetMouseButtonUp(0))
        {
            //RT
            pScript.RT();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            leftStick += new Vector2(0, 1);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            leftStick += new Vector2(0, -1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            leftStick += new Vector2(-1, 0);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            leftStick += new Vector2(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            leftStick += new Vector2(0, -1);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            leftStick += new Vector2(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            leftStick += new Vector2(1, 0);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            leftStick += new Vector2(-1, 0);
        }

        pScript.fakeLS(leftStick);

        //below mimics the right contoller stick
        if(rightStickScreenPos != lastRightStickScreenPos) 
        {
            if(Vector2.Distance(mainCam.ScreenToWorldPoint(rightStickScreenPos), mainCam.ScreenToWorldPoint(centreStick)) <= stickRadius) 
            {
                Vector2 diff = centreStick - rightStickScreenPos;
                diff = diff.normalized;

                pScript.fakeRS(-diff);
            }
        }

        lastRightStickScreenPos = rightStickScreenPos;
    }
}
