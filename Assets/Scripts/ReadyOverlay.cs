using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyOverlay : MonoBehaviour
{
    int playersJoined = 1;

    public void PlayerJoined()
    {
        if (playersJoined == 1)
        {
            Debug.Log("Player 1 Joined");
        }
        
        else if (playersJoined == 2)
        {
            Debug.Log("Player 2 Joined");
        }

        else
        {
            Debug.Log("Too many controllers");
        }
            
        playersJoined = playersJoined + 1; 
    }
}
