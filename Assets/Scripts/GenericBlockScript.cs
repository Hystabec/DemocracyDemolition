using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericBlockScript : MonoBehaviour
{
    [SerializeField]
    float RotationAmount = 90.0f;

    [SerializeField]
    Collider2D[] colliders;

    Color defaultColour;

    [SerializeField]
    LayerMask placedLayer;

    [SerializeField]
    Color cantPlaceColor = new Color(255, 0, 0);

    bool placed = false;

    bool isHovered = false;
    private bool canPlaceBlock = true;

    private GameObject outline = null;

    [SerializeField]
    UnityEvent<Collision2D> onCollisionEnterFunc, onCollisionExitFunc, onCollisionStayFunc;
    [SerializeField]
    UnityEvent<Collider2D> onTriggerEnterFunc, onTriggerExitFunc, onTriggerStayFunc;

    ParticleSystem redPlaceEffect = null;
    ParticleSystem bluePlaceEffect = null;

    [HideInInspector]
    public bool isPlacing;

    SpriteRenderer affectPlayArea = null;

    void Awake()
    {
        outline = gameObject.transform.Find("outline")?.gameObject;
        redPlaceEffect = gameObject.transform.Find("RedPlaceEffect")?.gameObject.GetComponent<ParticleSystem>();
        bluePlaceEffect = gameObject.transform.Find("bluePlaceEffect")?.gameObject.GetComponent<ParticleSystem>();

        defaultColour = gameObject.GetComponent<SpriteRenderer>().color;

        colliders = GetComponents<Collider2D>();

        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public float GetRotationAmount()
    {
        return RotationAmount;
    }

    public void CurrentlyPlacing(bool placing)
    {
        isPlacing = placing;

        if (placing == true)
        {
            //this is a problem
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }
        }
        else if(placing == false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;

            if (affectPlayArea != null)
            {
                affectPlayArea.enabled = false;
                affectPlayArea = null;  
            }
        }
    }

    public bool IsPlaced()
    {
        return placed;
    }

    public void Selected()
    {
        isHovered = true;
    }

    public void Deselected()
    {
        isHovered = false;
    }

    public bool GetIsHovered()
    {
        return isHovered;
    }

    public void Placed()
    {
        foreach(var collider in colliders)
        {
            collider.enabled = true;
        }

        if (redPlaceEffect != null)
        {
            redPlaceEffect.Play();
        }

        //convers the layerMask into a usable layer int
        gameObject.layer = (int)Mathf.Log(placedLayer.value, 2);

        placed = true;
    }

    public bool CanPlaceBlock()
    {
        return canPlaceBlock;
    }

    public void ShowOutline(bool showOutline)
    {
        if (outline != null)
            outline.SetActive(showOutline);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        onCollisionEnterFunc?.Invoke(other);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        onCollisionStayFunc?.Invoke(other);

        if (placed)
            return;

        if (!isPlacing)
            return;

        //Sets it in playerScript so the player cannot place the current block if its colliding with another block or the player
        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;
            

            if (other.gameObject.GetComponent<SpriteRenderer>())
            {
                var temp = other.gameObject.GetComponent<SpriteRenderer>();
                temp.enabled = true;
                affectPlayArea = temp;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        onCollisionExitFunc?.Invoke(other);

        if (placed)
            return;

        if (!isPlacing)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;

        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;

            if (other.gameObject.GetComponent<SpriteRenderer>())
            {
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                affectPlayArea = null;
            }
        }

      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnterFunc?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        onTriggerStayFunc?.Invoke(other);

        if (placed)
            return;

        if (!isPlacing)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            //Debug.Log("playerareaT");

            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

            if (other.gameObject.GetComponent<SpriteRenderer>())
            {
                var temp = other.gameObject.GetComponent<SpriteRenderer>();
                temp.enabled = true;
                affectPlayArea = temp;
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        onTriggerExitFunc?.Invoke(other);

        if (placed)
            return;

        if (!isPlacing)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;

        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;

            if (other.gameObject.GetComponent<SpriteRenderer>())
            {
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                affectPlayArea = null;
            }
        }
    }
}
