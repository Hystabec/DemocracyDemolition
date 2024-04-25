using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlPCOverride : MonoBehaviour
{
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

    private void Start()
    {
        pScript = gameObject.GetComponent<playerScript>();

        //pScript.PCOverride = true;
        Debug.Log("PC control override");

        pScript.switchMode();
        igm.pcControlOverride();
    }

    private void Update()
    {
        if (pScript == null)
            return;

        if(Input.GetKeyUp(KeyCode.F))
        {
            //A
            pScript.A();
        }

        if(Input.GetKeyUp(KeyCode.V))
        {
            //B
            pScript.B();
        }

        if(Input.GetKeyUp(KeyCode.Q))
        {
            //LB
            pScript.LB();
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            //RB
            pScript.RB();
        }

        if(Input.GetMouseButtonUp(0))
        {
            //RT
            pScript.RT();
        }

        if(Input.GetKeyDown(KeyCode.W))
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

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - fireMarker.transform.position;

        float zrotation = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

       if((zrotation > posMinAngle) && (zrotation < posMaxAngle))
                fireMarker.transform.rotation = Quaternion.Euler(0, 0, zrotation);

        else if ((zrotation < negMinAngle) && (zrotation > negMaxAngle))
        {
            fireMarker.transform.rotation = Quaternion.Euler(0, 0, zrotation);
        }
    }
}
