using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUi : MonoBehaviour
{

    [SerializeField]
    private GameObject[] coconutImages;

    [SerializeField]
    private bool rightToLeft;

    public List<GameObject> coconutImagesList;

    public List<Image> coolDownBarsActive;

    int ammoUsed = 2;

    private bool onCooldown;

    float moveTime = 0;

    bool added = false;


    void Start()
    {
        
    }

    public void OnRoundStart()
    {
        coconutImagesList = new List<GameObject>(coconutImages);
       
        foreach (var image in coconutImagesList)
        {
            image.GetComponent<SpriteRenderer>().enabled = false;

            if (image.transform.GetChild(0).gameObject.activeSelf != false)
            {
                image.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        if (rightToLeft)
        {
            ammoUsed = 2;
        }

        else
        {
            ammoUsed = 0;
        }
       
        if(coolDownBarsActive != null)
        {
            foreach(var image in coolDownBarsActive)
            {
                image.fillAmount = 0;
                moveTime = 0;
            }
        }


        onCooldown = false;
    }

    public void OnFightStage()
    {
        foreach (var image in coconutImagesList)
        {
            image.GetComponent<SpriteRenderer>().enabled = true;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CooldownBar();
    }

    public void Thrown()
    {
        coconutImagesList[ammoUsed].GetComponent<SpriteRenderer>().enabled = false;
        coconutImagesList.Remove(coconutImagesList[ammoUsed]);

        if (rightToLeft)
        {
            ammoUsed--;
        }

        else
            ammoUsed++;
    }

    public void DisabledThrowing()
    {
        if (coconutImagesList.Count > 0)
        {

            foreach (var image in coconutImagesList)
            {
                if (image.transform.GetChild(0).gameObject.activeSelf != true)
                {
                    image.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            onCooldown = true;
        }
    }

    public void EnabledThrowing()
    {
        if (coconutImagesList.Count > 0)
        {
            foreach (var image in coconutImagesList)
            {
                if (image.transform.GetChild(0).gameObject.activeSelf != false)
                {
                    image.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            onCooldown = false;
        }
    }

    void CooldownBar()
    {
        if (coconutImagesList.Count > 0)
        {
            var nextCoconut = coconutImagesList[ammoUsed];
            var cooldownBar = nextCoconut.transform.GetChild(1).GetComponent<Image>();

            if (onCooldown)
            {
                moveTime += Time.deltaTime * 0.1f;

                cooldownBar.fillAmount = Mathf.Lerp(cooldownBar.fillAmount, 1, moveTime);
               
                if (!added)
                {
                    added = true;
                    coolDownBarsActive.Add(cooldownBar);
                }
            }

            if (!onCooldown)
            {
                if (cooldownBar.fillAmount > 0)
                {
                    cooldownBar.fillAmount = 0;
                    moveTime = 0;
                    coolDownBarsActive.Remove(cooldownBar);
                    added = false;
                }
            }
        }
    }
    
}
