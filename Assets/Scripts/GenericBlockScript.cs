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
    Color cantPlaceColor = new Color(255, 0, 0);

    bool placed = false;

    bool isHovered = false;
    private bool canPlaceBlock = true;

    private GameObject outline = null;

    [SerializeField]
    UnityEvent<Collision2D> onCollisionEnterFunc, onCollisionExitFunc;
    [SerializeField]
    UnityEvent<Collider2D> onTriggerEnterFunc, onTriggerExitFunc;

    ParticleSystem placeEffect = null;

    private bool isPlacing;

    void Awake()
    {
        outline = gameObject.transform.Find("outline")?.gameObject;
        placeEffect = gameObject.transform.Find("PlaceEffect")?.gameObject.GetComponent<ParticleSystem>();

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
            foreach (var collider in colliders)
            {
                collider.enabled = true;
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

        if (placeEffect != null)
        {
            placeEffect.Play();
        }


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
        if(onCollisionEnterFunc != null)
            onCollisionEnterFunc.Invoke(other);

        if (placed)
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
                other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(onCollisionExitFunc != null)
            onCollisionExitFunc.Invoke(other);

        if (placed)
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
            }
        }

      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(onTriggerEnterFunc != null)
            onTriggerEnterFunc.Invoke(other);

        if (placed)
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
            Debug.Log("playerareaT");
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

            if (other.gameObject.GetComponent<SpriteRenderer>())
            {
                other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (placed)
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
            Debug.Log("playerareaT");
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

            if (other.gameObject.GetComponent<SpriteRenderer>())
            {
                other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

    }


    void OnTriggerExit2D(Collider2D other)
    {
        if(onTriggerExitFunc != null)
            onTriggerExitFunc.Invoke(other);

        if (placed)
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
            }
        }
    }
}
