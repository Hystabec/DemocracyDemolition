using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    
    [SerializeField]
    Timer timerScript;

    [SerializeField]
    Animator redAnim, blueAnim, camAnim, progressBarAnim;

    [SerializeField]
    Animator[] redAnimArray, blueAnimArray;

    [SerializeField]
    GameObject[] elementToHideWhenGameEnds, EndUIButtons;

    [SerializeField]
    float endButtonOffset = 266.0f;

    public Image redProgressBar;
    public Image blueProgressBar;

    bool gameHasEnded = false;

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
    public void bothPlayersJoined()
    {
        startRound();
    }

    public void ResetGame()
    {
        player1.GetComponent<playerScript>().ResetData();
        player2.GetComponent<playerScript>().ResetData();

        currentRound = 1;
        p1Score = 0;
        p2Score = 0;
        gameHasEnded = false;

        foreach(GameObject go in elementToHideWhenGameEnds)
        {
            go.SetActive(true);
        }

        foreach(GameObject go in EndUIButtons)
        { 
            go.SetActive(false);
        }
    }

    public void playerKilled(GameObject callingPlayer)
    {

        if (callingPlayer == player1)
        {
            p2Score++;
            blueAnim.SetTrigger("RoundWin");

            foreach (Animator ani in blueAnimArray)
            {
                ani.SetTrigger("RoundWin");
            }

            redAnim.SetTrigger("Hit");
            progressBarAnim.SetTrigger("BlueRoundWin");
            blueProgressBar.fillAmount = blueProgressBar.fillAmount + 0.25f;
        }
        else
        {
            p1Score++;
            redAnim.SetTrigger("RoundWin");

            foreach (Animator ani in redAnimArray)
            {
                ani.SetTrigger("RoundWin");
            }

            blueAnim.SetTrigger("Hit");
            progressBarAnim.SetTrigger("RedRoundWin");
            redProgressBar.fillAmount = redProgressBar.fillAmount + 0.25f;
        }
            

        if (p1Score >= roundsNeededToWin)
        {
            EndGame(player1);
            gameHasEnded = true;
        } 
        else if(p2Score >= roundsNeededToWin)
        {
            EndGame(player2);
            gameHasEnded = true;
        }  

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
       timerScript.StartTime();
    }

    void endRound()
    {
        playerScript ps = player1.GetComponent<playerScript>();
        ps.clearAndDeleteBlockList();
        ps.resetAmmo();

        ps = player2.GetComponent<playerScript>();
        ps.clearAndDeleteBlockList();
        ps.resetAmmo();

        timerScript.StopTimer();

        if (!gameHasEnded)
        {
            currentRound++;

            //add a wait time between rounds
            
            timerScript.timerReset();

            startRound();
        }
    }

    void EndGame(GameObject winningPlayer)
    {
        //DEBUG - ends playmode when winner is found
        //Debug.Log(winningPlayer.name + " wins");

        //when game ends player go to UI mode
        player1.GetComponent<playerScript>().switchMode(modes.UI);
        player2.GetComponent<playerScript>().switchMode(modes.UI);

        int winner = -1;

        foreach(GameObject go in elementToHideWhenGameEnds) 
        {
            go.SetActive(false);
        }

        if(winningPlayer == player1)
        {
            redAnim.SetTrigger("GameWin");

            foreach (Animator ani in redAnimArray)
            {
                ani.SetTrigger("GameWin");
            }
            winner = 1;
            camAnim.SetTrigger("GameWinRed");
        }
        else if(winningPlayer == player2) 
        {
            blueAnim.SetTrigger("GameWin");

            foreach(Animator ani in blueAnimArray)
            {
                ani.SetTrigger("GameWin");
                
            }
            winner = 2;
            camAnim.SetTrigger("GameWinBlue");
        }
        else 
        {
            //catch all - if you got here something is VERY wrong
            Debug.Log("No winner, when endgame called");
        }

        StartCoroutine(waitForEndCamPan(winner));
    }

    IEnumerator waitForEndCamPan(int winner)
    {
        float finalOffset = 0;

        if (winner == 1)
        {
            finalOffset = endButtonOffset;
        }
        else if(winner == 2)
        {
            finalOffset = -endButtonOffset;
        }

        //yield return new WaitUntil(() => camAnim.GetCurrentAnimatorStateInfo(0).IsName(triggerName));
        //yield return new WaitForSeconds(camAnim.GetCurrentAnimatorClipInfo(0).Length);

        //had to hard code the anim length - no ideal didnt work with the other ways i tried
        yield return new WaitForSeconds(4.2f);

        //showButtons
        foreach(GameObject go in EndUIButtons)
        {
            go.SetActive(true);
            go.transform.localPosition = new Vector3(finalOffset, go.transform.localPosition.y, go.transform.localPosition.z);
        }
    }

    private IEnumerator TimeBeforeFighting()
    {
        player1.GetComponent<playerScript>().CanFight(false);
        player2.GetComponent<playerScript>().CanFight(false);
        yield return new WaitForSecondsRealtime(timeBeforeFighting);

        player1.GetComponent<playerScript>().CanFight(true);
        player2.GetComponent<playerScript>().CanFight(true);
        StartCoroutine("RoundTime");
    }

    private IEnumerator RoundTime()
    {
        yield return new WaitForSecondsRealtime(roundTime);
        //Debug.Log("Rounded ended by timer");
        endRound();
    }
}
