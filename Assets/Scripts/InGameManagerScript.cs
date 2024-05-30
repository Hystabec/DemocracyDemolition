using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

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

    [HideInInspector, SerializeField]
    TextMeshProUGUI roundText, roundTypeText;

    //[HideInInspector, SerializeField]
    //TextMeshProUGUI[] playersAmmoText;

    //Feel free to unhide anything, just wanted to tidy up the inspector a bit
    [HideInInspector, SerializeField]
    Animator redAnim, blueAnim, camAnim, canvasAnim, rematchAnim, menuButtonAnim, fightAnim, buildAnim, roundTextAnim, roundTypeAnim, timerAnim, redCrowdAnim, blueCrowdAnim, ammoTextAnim;

    [SerializeField]
    GameObject[] elementToHideWhenGameEnds, EndUIButtons;

    [SerializeField]
    GameObject[] pauseMenuUIElements, objectsToShowOnPause;

    bool gameIsPause = false;

    [SerializeField]
    float endButtonOffset = 266.0f;

    public Image redProgressBar;
    public Image blueProgressBar;

    [SerializeField]
    AudioSource igmASource;

    [SerializeField]
    AudioClip crowdCheer, playerHit;

    [SerializeField]
    private GameObject buildIcons;

    [SerializeField]
    private GameObject fightIcons;

    bool gameHasEnded = false;

    [HideInInspector, SerializeField]
    private GameObject redConfetti, redFireworks, blueConfetti, blueFireworks, redWinText, blueWinText, redFireCentre, blueFireCentre;

    [HideInInspector, SerializeField]
    ParticleSystem redCrowdConfetti, redCrowdStreamers, blueCrowdConfetti, blueCrowdStreamers;

    [SerializeField]
    private screenShake screenShakeScript;

    playerScript pauseMenuOnwer = null;

    bool Player1OutOfAmmoNoActiveProjecties = false;
    bool Player2OutOfAmmoNoActiveProjecties = false;
       
    //bool betweenRounds = false;
    //bool pauseNotHandled = false;

    public void playerOutOfAmmo(GameObject callerGO)
    {
        if(callerGO == player1)
        {
            Player1OutOfAmmoNoActiveProjecties = true;
        }
        else if(callerGO == player2) 
        {
            Player2OutOfAmmoNoActiveProjecties = true;
        }

        if(Player1OutOfAmmoNoActiveProjecties && Player2OutOfAmmoNoActiveProjecties)
        {
            endRound();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rbs = GetComponent<randomBlockSpawn>();
        timerScript.AddListener(RoundTimeEnd);
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
        //StopCoroutine(RoundTime());

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
        redAnim.SetTrigger("Idle");
        blueAnim.SetTrigger("Idle");

        redConfetti.SetActive(false);
        blueConfetti.SetActive(false);
        redFireworks.SetActive(false);
        blueFireworks.SetActive(false);
        redWinText.SetActive(false);
        blueWinText.SetActive(false);

        cameraAnimationStatus CAS = mainCam.GetComponent<cameraAnimationStatus>();

        yield return new WaitUntil(() => CAS.Started() == true);
        yield return new WaitUntil(() => CAS.isAnimating() == false);
        
        CAS.Reset();

        player1.GetComponent<Animator>().SetBool("ResetGame", true);
        player2.GetComponent<Animator>().SetBool("ResetGame", true);

        timerScript.timerReset();

        player1.GetComponent<playerScript>().SwitchToPlayMode();
        player2.GetComponent<playerScript>().SwitchToPlayMode();

        foreach(GameObject go in elementToHideWhenGameEnds)
        {
            go.SetActive(true);
        }
        
        //foreach(GameObject go in EndUIButtons)
        //{
        //    go.SetActive(false);
        //}

        redCrowdAnim.SetTrigger("Idle");
        blueCrowdAnim.SetTrigger("Idle");

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
        FindFirstObjectByType<soundManager>().PlayOnce(playerHit, 0.8f);
        FindFirstObjectByType<soundManager>().PlayOnce(crowdCheer, 0.5f);

        if (callingPlayer == player1)
        {
            p2Score++;
            blueAnim.SetTrigger("RoundWin");

            if (p2Score < 4)
            {
                blueCrowdAnim.SetTrigger("RoundWin");
            }

            blueCrowdConfetti.Play();
            blueCrowdConfetti.Play();

            redAnim.SetTrigger("Hit");
            canvasAnim.SetTrigger("BlueRoundWin");
            blueProgressBar.fillAmount = blueProgressBar.fillAmount + 0.25f;
        }
        else
        {
            p1Score++;
            redAnim.SetTrigger("RoundWin");

            if (p1Score < 4)
            {
                redCrowdAnim.SetTrigger("RoundWin");
            }

            redCrowdConfetti.Play();
            redCrowdStreamers.Play();

            blueAnim.SetTrigger("Hit");
            canvasAnim.SetTrigger("RedRoundWin");
            redProgressBar.fillAmount = redProgressBar.fillAmount + 0.25f;
        }

        /*var p1Script = player1.GetComponent<playerScript>();

        if (p1Script.thrownProjectiles.Count > 0)
        {
            List<GameObject> thrownProjectiles = p1Script.thrownProjectiles;

            foreach (GameObject pooledProj in thrownProjectiles)
            {
                p1Script.despawnProjectile(pooledProj.gameObject, true);
            }

        }

        var p2Script = player2.GetComponent<playerScript>();

        if (p2Script.thrownProjectiles.Count > 0)
        {
            List<GameObject> thrownProjectiles2 = p2Script.thrownProjectiles;
            foreach (GameObject pooledProj in thrownProjectiles2)
            {
                p2Script.despawnProjectile(pooledProj.gameObject, true);
            }
        }*/

        callingPlayer.GetComponent<playerScript>().ProjInHandVisible(false);

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

        //StopCoroutine("RoundTime");

        endRound();
       
    }

    BAT blockTrapSplit(int total, int maxBlock, int maxTrap)
    {
        int MinBlocks = total - maxTrap;

        int returnBlock = UnityEngine.Random.Range(MinBlocks, maxBlock+1);

        int returnTrap = total - returnBlock;

        return new BAT(returnBlock, returnTrap);
    }
    void startRound()
    {
        BAT p1returnBat = new();
        BAT p2returnBat = new();
        igmASource.Play();

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

        if (currentRound == 1)
        {
            player1.GetComponent<Animator>().SetBool("ResetGame", false);
            player2.GetComponent<Animator>().SetBool("ResetGame", false);
        }


        StartCoroutine(RoundText());

        player1.GetComponent<playerScript>().OnRoundStart();
        player2.GetComponent<playerScript>().OnRoundStart();

        player1.GetComponent<playerScript>().thrownProjectiles.Clear();
        player2.GetComponent<playerScript>().thrownProjectiles.Clear();

        //timerScript.StartTime();
        StartCoroutine(endOfFrame());

        //StartCoroutine(TimeBeforeFighting());
        TimeBeforeFightingPartOne();
        timerScript.AddedEvent(timeBeforeFighting, TimeBeforeFightingPartTwo);

        timerScript.animTriggered = false;
    }

    public bool IsRoundPause()
    {
        return gameIsPause;
    }

    public void PauseRound(playerScript caller)
    {
        if (gameIsPause)
            return;

        gameIsPause = true;
        //pause time
        timerScript.StopTimer();

        player1.GetComponent<playerScript>().SwitchToUIMode();
        player2.GetComponent<playerScript>().SwitchToUIMode();

        foreach(GameObject go in objectsToShowOnPause)
        {
            go.SetActive(true);
        }

        //find all projectiles and freeze there velocitys
        
        var projectiles = FindObjectsOfType<pooledProjectileScript>();

        foreach (pooledProjectileScript p in projectiles)
        {
            p.Pause();
        }

        //activate ui elements
        foreach (GameObject go in pauseMenuUIElements)
        {
            go.SetActive(true);
        }

        //assing the calling player the UI elements
        caller.GiveUIElements(new List<GameObject>(pauseMenuUIElements));

        pauseMenuOnwer = caller;
    }

    public playerScript GetPauseMenuOwner()
    {
        return pauseMenuOnwer;
    }

    public void UnpauseRound()
    {
        if (!gameIsPause)
            return;

        gameIsPause = false;

        timerScript.ResumeTimer();

        player1.GetComponent<playerScript>().SwitchToPlayMode();
        player2.GetComponent<playerScript>().SwitchToPlayMode();

        foreach (GameObject go in objectsToShowOnPause)
        {
            go.SetActive(false);
        }

        var projectiles = FindObjectsOfType<pooledProjectileScript>();

        foreach (pooledProjectileScript p in projectiles)
        {
            p.UnPause();
        }

        //deactivate ui elements
        foreach (GameObject go in pauseMenuUIElements)
        {
            go.SetActive(false);
        }

        //clear the calling players ui elements
        pauseMenuOnwer.ClearUIElements();

        pauseMenuOnwer = null;
    }

    void endRound()
    {
        //despawn projectiles
        var p1Script = player1.GetComponent<playerScript>();

        if (p1Script.thrownProjectiles.Count > 0)
        {
            List<GameObject> thrownProjectiles = p1Script.thrownProjectiles;

            foreach (GameObject pooledProj in thrownProjectiles)
            {
                p1Script.despawnProjectile(pooledProj.gameObject, true);
            }

        }

        var p2Script = player2.GetComponent<playerScript>();

        if (p2Script.thrownProjectiles.Count > 0)
        {
            List<GameObject> thrownProjectiles2 = p2Script.thrownProjectiles;
            foreach (GameObject pooledProj in thrownProjectiles2)
            {
                p2Script.despawnProjectile(pooledProj.gameObject, true);
            }
        }

        playerScript ps = player1.GetComponent<playerScript>();
        ps.clearAndDeleteBlockList();
        ps.resetAmmo();
        ps.CanFight(false);
        Player1OutOfAmmoNoActiveProjecties = false;

        ps = player2.GetComponent<playerScript>();
        ps.clearAndDeleteBlockList();
        ps.resetAmmo();
        ps.CanFight(false);
        Player2OutOfAmmoNoActiveProjecties = false;


        timerScript.StopTimer();

        if (!gameHasEnded)
        {
            currentRound++;

            //add a wait time between rounds
            DelayBetweenRoundPartOne();
        }
    }

    void DelayBetweenRoundPartOne()
    {
        //betweenRounds = true;
        igmASource.Stop();
        timerScript.StartBetweenRounds(2.0f, DelayBetweenRoundPartTwo);
        timerScript.ResumeTimer();
    }

    void DelayBetweenRoundPartTwo()
    {
        timerScript.timerReset();
        //betweenRounds = false;
        startRound();
    }

    IEnumerator DelayBetweenRound()
    {
        //betweenRounds = true;
        igmASource.Stop();
        yield return new WaitForSeconds(2);
        timerScript.timerReset();
        //betweenRounds = false;
        startRound();
    }

    void EndGame(GameObject winningPlayer)
    {
        //DEBUG - ends playmode when winner is found
        //Debug.Log(winningPlayer.name + " wins");

        var player1Script = player1.GetComponent<playerScript>();
        var player2Script = player2.GetComponent<playerScript>();

        player1.GetComponent<playerScript>().ProjInHandVisible(false);
        player2.GetComponent<playerScript>().ProjInHandVisible(false);


        player1.GetComponent<playerScript>().tlScript.SetInvisible();
        player2.GetComponent<playerScript>().tlScript.SetInvisible();

        //when game ends player go to UI mode
        player1Script.SwitchToUIMode();
        player1Script.ProjInHandVisible(false);
        player1Script.gameEnded = true;


        player2Script.SwitchToUIMode();
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

            redCrowdAnim.SetTrigger("GameWin");

            winner = 1;
            camAnim.SetTrigger("GameWinRed");
        }
        else if(winningPlayer == player2) 
        {
            blueConfetti.SetActive(true);
            blueFireworks.SetActive(true);

            blueCrowdAnim.SetTrigger("GameWin");

            winner = 2;
            camAnim.SetTrigger("GameWinBlue");
        }
        else 
        {
            //catch all - if you got here something is VERY wrong
            Debug.Log("No winner, when endgame called");
        }

        igmASource.Stop();
        FindFirstObjectByType<soundManager>().PlayOnce(crowdCheer, 0.5f);
        StartCoroutine(waitForEndCamPan(winner));
    }

    IEnumerator waitForEndCamPan(int winner)
    {

        var CAS = mainCam.GetComponent<cameraAnimationStatus>();

        yield return new WaitUntil(() => CAS.Started() == true);
        yield return new WaitUntil(() => CAS.isAnimating() == false);

        CAS.Reset();

        float finalOffset = 0;

        if (winner == 1)
        {
            redWinText.SetActive(true);
            finalOffset = endButtonOffset;

            redAnim.SetTrigger("GameWin");
            redFireCentre.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(winner == 2)
        {
            blueWinText.SetActive(true);
            finalOffset = -endButtonOffset;

            blueAnim.SetTrigger("GameWin");
            blueFireCentre.transform.rotation = Quaternion.Euler(-180, 180, 0);
        }

        //yield return new WaitUntil(() => camAnim.GetCurrentAnimatorStateInfo(0).IsName(triggerName));
        //yield return new WaitForSeconds(camAnim.GetCurrentAnimatorClipInfo(0).Length);

        //had to hard code the anim length - no ideal didnt work with the other ways i tried
        //yield return new WaitForSeconds(4.5f);



        //showButtons
        foreach (GameObject go in EndUIButtons)
        {
            go.SetActive(true);
            go.transform.localPosition = new Vector3(finalOffset, go.transform.localPosition.y, go.transform.localPosition.z);
        }

        var players = FindObjectsOfType<playerScript>();

        foreach(playerScript player in players)
        {
            if (player.GetAssignedControllerIndex() == 0)
            {
                player.GiveUIElements(new List<GameObject>(EndUIButtons));
                break;
            }
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

    void TimeBeforeFightingPartOne()
    {
        StartCoroutine(RoundTypeText("Build!"));
        buildIcons.SetActive(true);
        fightIcons.SetActive(false);
        buildAnim.SetTrigger("Build");
        timerAnim.SetTrigger("Pop");
        canvasAnim.SetTrigger("BuildFlash");
        player1.GetComponent<playerScript>().CanFight(false);
        player2.GetComponent<playerScript>().CanFight(false);

        if (currentRound > 1)
        {
            //ammoTextAnim.SetTrigger("FadeOut");
        }
    }

    void TimeBeforeFightingPartTwo()
    {
        player1.GetComponent<playerScript>().ProjInHandVisible(true);
        player2.GetComponent<playerScript>().ProjInHandVisible(true);

        player1.GetComponent<playerScript>().tlScript.SetVisible();
        player2.GetComponent<playerScript>().tlScript.SetVisible();

        player1.GetComponent<playerScript>().CanFight(true);
        player2.GetComponent<playerScript>().CanFight(true);
        StartCoroutine(RoundTypeText("Fight!"));
        canvasAnim.SetTrigger("BuildFlash");
        timerAnim.SetTrigger("Pop");
        fightIcons.SetActive(true);
        buildIcons.SetActive(false);
        fightAnim.SetTrigger("Fight");
        //ammoTextAnim.SetTrigger("FadeIn");
        roundTextAnim.SetTrigger("Fight");

        player1.GetComponent<playerScript>().ammoUiScript.OnFightStage();
        player2.GetComponent<playerScript>().ammoUiScript.OnFightStage();

        timerScript.SwitchToFight();

        timerScript.RemoveEvent(TimeBeforeFightingPartTwo, timeBeforeFighting);
        //StartCoroutine("RoundTime");
    }

    private IEnumerator TimeBeforeFighting()
    {
        StartCoroutine(RoundTypeText("Build!"));
        buildIcons.SetActive(true);
        fightIcons.SetActive(false);
        buildAnim.SetTrigger("Build");
        timerAnim.SetTrigger("Pop");
        canvasAnim.SetTrigger("BuildFlash");
        player1.GetComponent<playerScript>().CanFight(false);
        player2.GetComponent<playerScript>().CanFight(false);

        if (currentRound > 1)
        {
            //ammoTextAnim.SetTrigger("FadeOut");
        }

        yield return new WaitForSecondsRealtime(timeBeforeFighting);

        player1.GetComponent<playerScript>().ProjInHandVisible(true);
        player2.GetComponent<playerScript>().ProjInHandVisible(true);

        player1.GetComponent<playerScript>().CanFight(true);
        player2.GetComponent<playerScript>().CanFight(true);
        StartCoroutine(RoundTypeText("Fight!"));
                canvasAnim.SetTrigger("BuildFlash");
        timerAnim.SetTrigger("Pop");
        fightIcons.SetActive(true);
        buildIcons.SetActive(false);
        fightAnim.SetTrigger("Fight");
        //ammoTextAnim.SetTrigger("FadeIn");
        roundTextAnim.SetTrigger("Fight");

        player1.GetComponent<playerScript>().ammoUiScript.OnFightStage();
        player2.GetComponent<playerScript>().ammoUiScript.OnFightStage();

        timerScript.SwitchToFight();

        //StartCoroutine("RoundTime");
    }

    //private IEnumerator RoundTime()
    //{
    //    yield return new WaitForSecondsRealtime(roundTime);
    //    //Debug.Log("Rounded ended by IEnumerator timer");
    //    endRound();
    //}

    IEnumerator endOfFrame()
    {
        yield return new WaitForEndOfFrame();
        timerScript.StartTime();
    }

    void RoundTimeEnd()
    {
        //Debug.Log("Round ended by timer");
        endRound();
    }
}