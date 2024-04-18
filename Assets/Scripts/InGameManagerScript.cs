using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManagerScript : MonoBehaviour
{
    /// <summary>
    /// when placing a block its collider should be disabled, once placed the collider should be enabled.
    /// when not selected block should have its outline hidden, when selected enable outline
    /// </summary>


    int currentRound = 1;

    int p1Score = 0, p2Score = 0;

    [SerializeField]
    GameObject player1, player2;

    randomBlockSpawn rbs;

    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponent<randomBlockSpawn>();
    }

    public void playerKilled(GameObject callingPlayer)
    {
        Debug.Log(callingPlayer.name + " died");

        if (callingPlayer == player1)
            p2Score++;
        else
            p1Score++;

        endRound();
    }

    void startRound()
    {
        switch (currentRound)
        {
            case 1:
                rbs.spawnBlockIn(player1, 1);
                rbs.spawnBlockIn(player2, 1);
                return;
            case 2:

                return;
            case 3:

                return;
            case 4:

                return;
            case 5:

                return;
            case 6:

                return;
            case 7:

                return;
            case 8:

                return;
            case 9:

                return;
            default:
                return;
        }
    }

    void endRound()
    {
        //despawn all held blocks
        currentRound++;
        //add a wait time between rounds
        startRound();
    }
}
