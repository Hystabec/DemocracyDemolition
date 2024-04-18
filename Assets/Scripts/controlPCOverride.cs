using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlPCOverride : MonoBehaviour
{
    [SerializeField]
    playerScript pScript;
    [SerializeField]
    Camera mainCam;
    Vector3 mousePos;
    [SerializeField]
    GameObject fireMarker;

    private void Awake()
    {
        //pScript.PCOverride = true;
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

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - fireMarker.transform.position;

        float zrotation = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        fireMarker.transform.rotation = Quaternion.Euler(0, 0, zrotation);
    }
}
