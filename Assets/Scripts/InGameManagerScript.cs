using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class BAT
{
    public BAT()
    {
        numBlock = 0;
        numTraps = 0;
    }

    public BAT(int block, int trap)
    {
        numBlock = block;
        numTraps = trap;
    }

    public int numBlock;
    public int numTraps;
}


public class InGameManagerScript : MonoBehaviour
{
    /// <summary>
    /// when placing a block its collider should be disabled, once placed the collider should be enabled.
    /// when not selected block should have its outline hidden, when selected enable outline
    /// </summary>


    int numNeededPlayers = 2;
    int numJoinedPlayers = 0;

    [SerializeField]
    float roundTime = 20.0f;

    [SerializeField]
    float timeBeforeFighting = 5.0f;

    int currentRound = 1;

    int p1Score = 0, p2Score = 0;

    [SerializeField]
    int roundsNeededToWin = 4;

    [SerializeField]
    GameObject player1, player2;

    randomBlockSpawn rbs;

    public Animator redAnim;
    public Animator blueAnim;


    // Start is called before the first frame update
    void Awake()
    {
        rbs = GetComponent<randomBlockSpawn>();
    }

    public void pcControlOverride()
    {
        //debug function
        startRound();
    }

    public void AddPlayer()
    {
        numJoinedPlayers++;


        if(numJoinedPlayers >= numNeededPlayers)
            startRound();
    }

    public void playerKilled(GameObject callingPlayer)
    {
        Debug.Log(callingPlayer.name + " died");

        if (callingPlayer == player1)
        {
            p2Score++;
            blueAnim.SetTrigger("RoundWin");
            redAnim.SetTrigger("Hit");
        }

        else
        {
            p1Score++;
            redAnim.SetTrigger("RoundWin");
            blueAnim.SetTrigger("Hit");
        }
            
  


        if (p1Score >= roundsNeededToWin)
            EndGame(player1);
        else if(p2Score >= roundsNeededToWin)
            EndGame(player2);

        StopCoroutine("RoundTime");

        endRound();
    }


    BAT blockTrapSplit(int total, int maxBlock, int maxTrap)
    {
        int MinBlocks = total - maxTrap;

        int returnBlock = Random.Range(MinBlocks, maxBlock+1);

        int returnTrap = total - returnBlock;

        return new BAT(returnBlock, returnTrap);
    }

    void startRound()
    {
        BAT p1returnBat = new();
        BAT p2returnBat = new();

        switch (currentRound)
        {
            case 1:
                p1returnBat = blockTrapSplit(1, 1, 0);
                p2returnBat = blockTrapSplit(1, 1, 0);
                break;
            case 2:
                p1returnBat = blockTrapSplit(1, 1, 1);
                p2returnBat = blockTrapSplit(1, 1, 1);
                break;
            case 3:
                p1returnBat = blockTrapSplit(2, 1, 1);
                p2returnBat = blockTrapSplit(2, 1, 1);
                break;
            case 4:
                p1returnBat = blockTrapSplit(2, 1, 1);
                p2returnBat = blockTrapSplit(2, 1, 1);
                break;
            case 5:
                p1returnBat = blockTrapSplit(3, 2, 2);
                p2returnBat = blockTrapSplit(3, 2, 2);
                break;
            case 6:
                p1returnBat = blockTrapSplit(3, 2, 2);
                p2returnBat = blockTrapSplit(3, 2, 2);
                break;
            case 7:
                p1returnBat = blockTrapSplit(3, 2, 2);
                p2returnBat = blockTrapSplit(3, 2, 2);
                break;
            case 8:
                p1returnBat = blockTrapSplit(3, 2, 2);
                p2returnBat = blockTrapSplit(3, 2, 2);
                break;
            case 9:
                p1returnBat = blockTrapSplit(3, 2, 2);
                p2returnBat = blockTrapSplit(3, 2, 2);
                break;
            default:
                p1returnBat = blockTrapSplit(3, 2, 2);
                p2returnBat = blockTrapSplit(3, 2, 2);
                break;
        }

        if(!(p1returnBat.numBlock == 0 && p1returnBat.numTraps == 0))
        {
            rbs.spawnBlockIn(player1, p1returnBat.numBlock);
            rbs.spawnBlockIn(player2, p2returnBat.numBlock);

            rbs.spawnTrapIn(player1, p1returnBat.numTraps);
            rbs.spawnTrapIn(player2, p2returnBat.numTraps);
        }

        player1.GetComponent<playerScript>().OnRoundStart();
        player2.GetComponent<playerScript>().OnRoundStart();

       StartCoroutine(TimeBeforeFighting());
        StartCoroutine("RoundTime");
    }

    void endRound()
    {
        playerScript ps = player1.GetComponent<playerScript>();
        ps.clearAndDeleteBlockList();
        ps.resetAmmo();

        ps = player2.GetComponent<playerScript>();
        ps.clearAndDeleteBlockList();
        ps.resetAmmo();


        currentRound++;

        //add a wait time between rounds

        startRound();
    }

    void EndGame(GameObject winningPlayer)
    {
        //DEBUG - ends playmode when winner is found
        Debug.Log(winningPlayer.name + " wins");
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private IEnumerator TimeBeforeFighting()
    {
        player1.GetComponent<playerScript>().CanFight(false);
        player2.GetComponent<playerScript>().CanFight(false);
        yield return new WaitForSecondsRealtime(timeBeforeFighting);
        Debug.Log("Start fighting");
        player1.GetComponent<playerScript>().CanFight(true);
        player2.GetComponent<playerScript>().CanFight(true);
    }


    private IEnumerator RoundTime()
    {
        yield return new WaitForSecondsRealtime(roundTime);
        Debug.Log("Rounded ended by timer");
        endRound();
    }
}
