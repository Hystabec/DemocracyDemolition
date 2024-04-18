using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyOverlay : MonoBehaviour
{
    int playersJoined = 1;
    
    [SerializeField]
    GameObject player1Button, player2Button;
    
    [SerializeField]
    GameObject player1, player2;

    public bool bothPlayersJoined = false;
    



    public void PlayerJoined()
    {
        if (playersJoined == 1)
        {
            Destroy(player1Button);
            Debug.Log("Player 1 Joined");
            Player1Joined();
        }
        
        else if (playersJoined == 2)
        {
            Destroy(player2Button);
            Debug.Log("Player 2 Joined");
            Player2Joined();
            BothPlayersJoined();
        }

        else
        {
            Debug.Log("Too many controllers");
        }
            
        playersJoined = playersJoined + 1; 
    }

    public void Player1Joined()
    {

    }

    public void Player2Joined()
    {

    }

    public void BothPlayersJoined()
    {
        bothPlayersJoined = true;
        player1.GetComponent<playerScript>().switchMode();
        player2.GetComponent<playerScript>().switchMode();
    }
}
