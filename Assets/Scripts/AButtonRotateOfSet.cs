using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AButtonRotateOfSet : MonoBehaviour
{
    Quaternion lastParentRotation;
    GameObject outline;

    void Start()
    {
        lastParentRotation = transform.root.localRotation;
        outline = transform.root.Find("outline").gameObject;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(outline.activeSelf)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        

        transform.localRotation = Quaternion.Inverse(transform.root.localRotation) * lastParentRotation * transform.localRotation;

        lastParentRotation = transform.root.localRotation;
    }
}
