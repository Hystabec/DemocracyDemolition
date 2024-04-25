using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockScript : MonoBehaviour
{
    Color defaultColour;

    [SerializeField]
    Color cantPlaceColor = new Color(255, 0, 0);

    [SerializeField]
    int TotalBlockHealth = 3;
    int CurrentBlockHealth = 3;
    [SerializeField]
    bool breakable;

    private GameObject Player;
    private playerScript chosenPlayerScript;

    Collider2D col;

    bool isHovered = false;
    private bool canPlaceBlock = true;

    [SerializeField]
    private GameObject outline;

    [SerializeField]
    private AudioClip blockHitSound;

    [SerializeField]
    private AudioSource aSource;


    private void Awake()
    {
        //works as long as outline is first child
        outline = transform.GetChild(0).gameObject;

        //HERE FOR TESTING
        //outline.GetComponent<SpriteRenderer>().color = Color.red;
        outline.GetComponent<SpriteRenderer>().color = Color.red;

        defaultColour = gameObject.GetComponent<SpriteRenderer>().color;

        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }

    public void Selected()
    {
        isHovered = true;
    }

    public void Deselected()
    {
        isHovered = false;
    }

    public void placed()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBlockHealth = TotalBlockHealth;

        col = gameObject.GetComponent<Collider2D>();
       // col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChosePlayer(GameObject playerobj)
    {
        Player = playerobj;
        chosenPlayerScript = Player.GetComponent<playerScript>();
        chosenPlayerScript.AddBlocks(gameObject);
       // col.enabled = true;
    }

    public void assignPlayer(GameObject owningPlayer)
    {
        Player = owningPlayer;
        chosenPlayerScript = Player.GetComponent<playerScript>();
    }
    public void ShowOutline(bool shouldShow)
    {
        outline.SetActive(shouldShow);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("projectile"))
        {
            aSource.PlayOneShot(blockHitSound);
            if (breakable)
            {
                CurrentBlockHealth--;

                if (CurrentBlockHealth <= 0)
                {
                    CurrentBlockHealth = TotalBlockHealth;
                    Debug.Log(gameObject.name + " destroyed");
                    gameObject.SetActive(false);
                }
            }
        }


        //early return so placeing checks arent done;
        if (!isHovered)
            return;

        //Sets it in playerScript so the player cannot place the current block if its colliding with another block or the player

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            if (Player)
            {
                canPlaceBlock = false;
                chosenPlayerScript.CanPlaceBlock(false);

                gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;
            }
        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            if (Player)
            {
                canPlaceBlock = false;
                chosenPlayerScript.CanPlaceBlock(false);

                gameObject.GetComponent<SpriteRenderer>().color = cantPlaceColor;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //early return so placeing checks arent done;
        if (!isHovered)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            if (Player)
            {
                canPlaceBlock = true;
                chosenPlayerScript.CanPlaceBlock(true);

                gameObject.GetComponent<SpriteRenderer>().color = defaultColour;
            }
        }

        if (other.gameObject.CompareTag("playerArea"))
        {
            if (Player)
            {
                canPlaceBlock = true;
                chosenPlayerScript.CanPlaceBlock(true);

                gameObject.GetComponent<SpriteRenderer>().color = defaultColour;
            }
        }
    }
}
