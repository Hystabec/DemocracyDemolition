using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericBlockScript : MonoBehaviour
{
    [SerializeField]
    Collider2D[] colliders;

    Color defaultColour;

    [SerializeField]
    Color cantPlaceColor = new Color(255, 0, 0);

    bool isHovered = false;
    private bool canPlaceBlock = true;

    [SerializeField]
    private GameObject outline;

    [SerializeField]
    UnityEvent<Collision2D> onCollisionEnterFunc, onCollisionExitFunc;
    [SerializeField]
    UnityEvent<Collider2D> onTriggerEnterFunc, onTriggerExitFunc;

    void Awake()
    {
        outline = gameObject.transform.Find("outline")?.gameObject;

        defaultColour = gameObject.GetComponent<SpriteRenderer>().color;

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

    public void Selected()
    {
        isHovered = true;
    }

    public void Deselected()
    {
        isHovered = false;
    }

    public void Placed()
    {
        foreach(var collider in colliders)
        {
            collider.enabled = true;
        }
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

        if (!isHovered)
            return;

        //Sets it in playerScript so the player cannot place the current block if its colliding with another block or the player

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;

        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            canPlaceBlock = false;
            gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(onCollisionExitFunc != null)
            onCollisionExitFunc.Invoke(other);

        if (!isHovered)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;
        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            canPlaceBlock = true;
            gameObject.GetComponent<SpriteRenderer>().color = defaultColour;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(onTriggerEnterFunc != null)
            onTriggerEnterFunc.Invoke(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(onTriggerExitFunc != null)
            onTriggerExitFunc.Invoke(other);
    }
}
