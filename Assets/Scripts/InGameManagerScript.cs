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
    Camera mainCam;

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
    TextMeshProUGUI roundText, roundTypeText;

    [SerializeField]
    TextMeshProUGUI[] playersAmmoText;

    [HideInInspector, SerializeField]
    Animator redAnim, blueAnim, camAnim, progressBarAnim, rematchAnim, menuButtonAnim, fightAnim, buildAnim, roundTextAnim, roundTypeAnim, timerAnim;

    [SerializeField]
    Animator[] redAnimArray, blueAnimArray;

    [SerializeField]
    GameObject[] elementToHideWhenGameEnds, EndUIButtons;


    [SerializeField]
    float endButtonOffset = 266.0f;

    public Image redProgressBar;
    public Image blueProgressBar;

    [SerializeField]
    AudioSource roundVictorySound;

    [SerializeField]
    AudioClip crowdCheer, playerHit;

    [SerializeField]
    private GameObject buildIcons;

    [SerializeField]
    private GameObject fightIcons;

    bool gameHasEnded = false;

    [HideInInspector, SerializeField]
    private GameObject redConfetti, redFireworks, blueConfetti, blueFireworks;

    [HideInInspector, SerializeField]
    ParticleSystem redCrowdConfetti, redCrowdStreamers, blueCrowdConfetti, blueCrowdStreamers;

    [SerializeField]
    private screenShake screenShakeScript;

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

    public void CallResetGame()
    {
        StartCoroutine(ResetGame());
    }



    public IEnumerator ResetGame()
    {
        player1.GetComponent<playerScript>().ResetData();
        player2.GetComponent<playerScript>().ResetData();

        currentRound = 1;
        p1Score = 0;
        p2Score = 0;
        gameHasEnded = false;

        redProgressBar.fillAmount = 0;
        blueProgressBar.fillAmount = 0;

        //yield return new WaitForSeconds(4f);
        //yield return new WaitWhile(() => mainCam.GetComponent<cameraAnimationStatus>().isAnimating() == false);

        rematchAnim.SetTrigger("RematchFadeOut");
        menuButtonAnim.SetTrigger("MenuButtonFadeOut");
        camAnim.SetTrigger("ReturnCamera");

        cameraAnimationStatus CAS = mainCam.GetComponent<cameraAnimationStatus>();

        yield return new WaitUntil(() => CAS.Started() == true);
        yield return new WaitUntil(() => CAS.isAnimating() == false);
        
        CAS.Reset();

        player1.GetComponent<Animator>().SetBool("ResetGame", true);
        player2.GetComponent<Animator>().SetBool("ResetGame", true);

        timerScript.timerReset();

        player1.GetComponent<playerScript>().switchMode(modes.Play);
        player2.GetComponent<playerScript>().switchMode(modes.Play);

        foreach(GameObject go in elementToHideWhenGameEnds)
        {
            go.SetActive(true);
        }
        
        //foreach(GameObject go in EndUIButtons)
        //{
        //    go.SetActive(false);
        //}

        foreach(Animator ani in redAnimArray)
        {
            ani.SetTrigger("Idle");
        }
        
        foreach (Animator ani in blueAnimArray)
        {
            ani.SetTrigger("Idle");
        }


        if (redConfetti.activeSelf == true)
            redConfetti.SetActive(false);

        if (redFireworks.activeSelf == true)
            redFireworks.SetActive(false);

        if (blueConfetti.activeSelf == true)
            blueConfetti.SetActive(false);

        if (blueFireworks.activeSelf == true)
            blueFireworks.SetActive(false);

        startRound();
    }



    public void playerKilled(GameObject callingPlayer)
    {
        screenShakeScript.TriggerShake();

        if (callingPlayer == player1)
        {
            p2Score++;
            blueAnim.SetTrigger("RoundWin");

            foreach (Animator ani in blueAnimArray)
            {
                ani.SetTrigger("RoundWin");
            }

            blueCrowdConfetti.Play();
            blueCrowdConfetti.Play();

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

            redCrowdConfetti.Play();
            redCrowdStreamers.Play();

            blueAnim.SetTrigger("Hit");
            progressBarAnim.SetTrigger("RedRoundWin");
            redProgressBar.fillAmount = redProgressBar.fillAmount + 0.25f;
        }

        var p1Script = player1.GetComponent<playerScript>();

        if (p1Script.thrownProjectiles.Count > 0)
        {
            List<GameObject> thrownProjectiles = p1Script.thrownProjectiles;

            foreach (GameObject pooledProj in thrownProjectiles)
            {
                p1Script.despawnProjectile(pooledProj.gameObject);
            }

        }

        var p2Script = player2.GetComponent<playerScript>();

        if (p2Script.thrownProjectiles.Count > 0)
        {
            List<GameObject> thrownProjectiles2 = p2Script.thrownProjectiles;
            foreach (GameObject pooledProj in thrownProjectiles2)
            {
                p2Script.despawnProjectile(pooledProj.gameObject);
            }

        }


        roundVictorySound.PlayOneShot(crowdCheer);
        roundVictorySound.PlayOneShot(playerHit);

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


        StartCoroutine(RoundText());

        player1.GetComponent<playerScript>().OnRoundStart();
        player2.GetComponent<playerScript>().OnRoundStart();

        player1.GetComponent<playerScript>().thrownProjectiles.Clear();
        player2.GetComponent<playerScript>().thrownProjectiles.Clear();

        StartCoroutine(TimeBeforeFighting());

        timerScript.StartTime();

        timerScript.animTriggered = false;


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

        var player1Script = player1.GetComponent<playerScript>();
        var player2Script = player2.GetComponent<playerScript>();

        //when game ends player go to UI mode
        player1Script.switchMode(modes.UI);
        player1Script.ProjInHandVisible(false);
        player1Script.gameEnded = true;


        player2Script.switchMode(modes.UI);
        player2Script.ProjInHandVisible(false);
        player2Script.gameEnded = true;

        int winner = -1;

        foreach(GameObject go in elementToHideWhenGameEnds) 
        {
            go.SetActive(false);
        }

        if (winningPlayer == player1)
        {
            redConfetti.SetActive(true);
            redFireworks.SetActive(true);

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
            blueConfetti.SetActive(true);
            blueFireworks.SetActive(true);

            blueAnim.SetTrigger("GameWin");

            foreach (Animator ani in blueAnimArray)
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
        //yield return new WaitForSeconds(4.5f);

        var CAS = mainCam.GetComponent<cameraAnimationStatus>();

        yield return new WaitUntil(() => CAS.Started() == true);
        yield return new WaitUntil(() => CAS.isAnimating() == false);

        CAS.Reset();

        //showButtons
        foreach (GameObject go in EndUIButtons)
        {
            go.SetActive(true);
            go.transform.localPosition = new Vector3(finalOffset, go.transform.localPosition.y, go.transform.localPosition.z);
        }

        rematchAnim.SetTrigger("RematchFadeIn");
        menuButtonAnim.SetTrigger("MenuButtonFadeIn");
    }

    private IEnumerator RoundText()
    {
        roundText.alpha = 1;
        roundText.SetText("Round " + currentRound.ToString());
        roundTextAnim.SetTrigger("RoundBegin");
        yield return new WaitForSeconds(5.25f);
        roundText.alpha = 0;
    }
    private IEnumerator RoundTypeText(string text)
    {
        roundTypeText.alpha = 1;
        roundTypeText.SetText(text);
        yield return new WaitForSeconds(2f);
      //  roundTypeText.alpha = 0;
    }



    private IEnumerator TimeBeforeFighting()
    {
        StartCoroutine(RoundTypeText("Build!"));
        roundTypeAnim.SetTrigger("BuildBegin");
        buildIcons.SetActive(true);
        fightIcons.SetActive(false);
        buildAnim.SetTrigger("Build");
        timerAnim.SetTrigger("Pop");
        player1.GetComponent<playerScript>().CanFight(false);
        player2.GetComponent<playerScript>().CanFight(false);
        
        foreach (TextMeshProUGUI text in playersAmmoText)
        {
            text.gameObject.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(timeBeforeFighting);

        player1.GetComponent<playerScript>().ProjInHandVisible(true);
        player2.GetComponent<playerScript>().ProjInHandVisible(true);

        player1.GetComponent<playerScript>().CanFight(true);
        player2.GetComponent<playerScript>().CanFight(true);
        StartCoroutine(RoundTypeText("Fight!"));
        roundTypeAnim.SetTrigger("FightBegin");
        timerAnim.SetTrigger("Pop");
        fightIcons.SetActive(true);
        buildIcons.SetActive(false);
        fightAnim.SetTrigger("Fight");
        
        foreach (TextMeshProUGUI text in playersAmmoText)
        {
            text.gameObject.SetActive(true);
        }

        StartCoroutine("RoundTime");

    }

    private IEnumerator RoundTime()
    {
        yield return new WaitForSecondsRealtime(roundTime);
        //Debug.Log("Rounded ended by timer");
        endRound();
    }
}
