using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using UnityEngine.Animations;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public Cinemachine.CinemachineConfiner confiner;
    [SerializeField] PolygonCollider2D cameraBox;

    [SerializeField] float levelTime = 120f;
    private float levelTimer = 0;



    //For showing dialouge box and providing upgrades
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] MessageBox messageBox;
    public DialogueEffect[] dialogues;
    public DialogueEffect victoryDialouge;
    public DialogueEffect failDialouge;
    public bool skipDialouge = false;
    public int preStartDialouges = 1;
    public int[] dialougePointTriggers;
    private int currDialogue = -1;

    //For Modifying player Stats
    private PlayerAttack playerAtk;
    private PlayerMovement playerMove;

    //Used for UI
    private bool gameOver = false;
    private bool gameStarted = false;
    private bool panic = false;
    [SerializeField] GameObject ObliterateUI;
    [SerializeField] GameObject BreakdownUI;
    [SerializeField] GameObject PlayerHUD;
    [SerializeField] GameObject pauseUI;
    private bool paused = false;
    private bool boxOverride = false;

    //Score
    [SerializeField] public bool doCountWalls = false;
    private int playerScore = 0;
    private int objsToDestroy = 0;
    private int numWalls = 0;
    private int numDestroyed = 0;
    private int maxScore = 0;

    private void Update()
    {


        if (!gameOver)
        {
            if (gameStarted)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("Pressing ESC");
                    if (!paused)
                    {
                        PauseGame();
                    }
                    else
                    {
                        UnPauseGame();
                    }
                }

                levelTimer -= Time.deltaTime;
                timeText.text = ((int)levelTimer).ToString();
                if (levelTimer <= 0)
                {
                    StartCoroutine(GameOver());
                }
                else if (levelTimer <= 10 && !panic)
                {
                    FindObjectOfType<ClockBlink>().ChangeColor();
                    panic = true;
                }
            }

        }
    }

    public void Awake()
    {
        Time.timeScale = 1;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        //Intialize all script references
        confiner = FindObjectOfType<Cinemachine.CinemachineConfiner>();
        playerAtk = FindObjectOfType<PlayerAttack>();
        playerMove = FindObjectOfType<PlayerMovement>();


        //Dont let the player move until dialouge is over
        RestrictPlayer();

        PlayerHUD.SetActive(true);
        messageBox.HideBox();


    }

    public void Start()
    {
        //Start the game
        StartCoroutine(playOpeningDialogues());

    }

    //Resets the palyer's score and determines the max score based on all the objects in the scene
    public void StartGame()
    {
        AudioManager.instance.PlayMusic("LevelTheme");
        gameStarted = true;
        PlayerHUD.SetActive(true);
        playerScore = 0;
        scoreText.text = playerScore.ToString();
        numDestroyed = 0;
        levelTimer = levelTime;
        IntializeCounters();
        FreePlayer();
        Debug.Log("Max Score Possible: " + maxScore.ToString());
    }

    //Add score from outside the gameManager
    public void AddScore(int amt)
    {
        playerScore += amt;
        numDestroyed += 1;
        scoreText.text = playerScore.ToString();

        if (playerScore >= maxScore)
        {
            //Signify the end of the level if the player breaks everything in the scene
            StartCoroutine(EndLevel(true));
        }

        if ( currDialogue+1 < (dialougePointTriggers.Length+preStartDialouges) && playerScore >= dialougePointTriggers[currDialogue + 1 - preStartDialouges])
        {
            ShowNextDiaglogue(true);
        }

    }

    //Called when specific walls are broken
    public void SwitchCameraConfiner()
    {
        if (cameraBox != null)
        {

            confiner.m_BoundingShape2D = cameraBox;
        }
    }

    //Upgrade the player's stats and display a message
    public void ShowNextDiaglogue(bool changeStats)
    {
        //Ensure no out of bounds errors for references
        if (currDialogue + 1 < dialogues.Length)
        {
            currDialogue += 1;
            
            //Update the message box
            DialogueEffect dialogue = dialogues[currDialogue];
            messageBox.ShowBox(dialogue);
            
            if (changeStats)
            {
                //Increase the player's stats
                playerAtk.IncreaseStats(dialogue.atkBoost, dialogue.atkSpeedBoost, dialogue.knockbackBoost);
                playerMove.ChangeSpeed(dialogue.speedBoost);
            }

            
        }

    }

    //Stop the game and pull up level over UI
    private IEnumerator GameOver()
    {
        //Stop the player from moving
        gameOver = true;
        RestrictPlayer();
        playerMove.TriggerBreakdown();

        AudioManager.instance.PlayMusic("Game Over");

        //Pause to show losing message
        messageBox.ShowBox(failDialouge);
        
        yield return new WaitForSeconds(failDialouge.duration);

        BreakdownUI.SetActive(true);
        string achieveText = "You broke " + numDestroyed + "/" +(objsToDestroy - numWalls).ToString() + " objects!";
        string timeText = "You ran out of time!";
        string scoreText = "Scored " + playerScore + "/" + maxScore + " points!";
        BreakdownUI.GetComponent<LevelFinishUI>().updateText(achieveText, timeText, scoreText);
    }

    //Determines the amount of points possible that the player could earn
    private void IntializeCounters()
    {
        int scorePossible = 0;
        BreakableObj[] objs =  FindObjectsOfType<BreakableObj>();
        objsToDestroy = objs.Length;

        for (int i = 0; i < objs.Length; i++)
        {
            if (!doCountWalls)
            {
                if (!(objs[i] is WallObj))
                {
                    scorePossible += objs[i].points;
                }
                else
                {
                    numWalls++;
                }
            }
            else
            {
                scorePossible += objs[i].points;
            }
        }

        maxScore = scorePossible;
    }

    public IEnumerator EndLevel(bool obliterated)
    {
        gameOver = true;

        AudioManager.instance.PlayMusic("Victory Theme");

        //Pause to show losing message
        messageBox.ShowBox(victoryDialouge);

        yield return new WaitForSeconds(victoryDialouge.duration);

        //Stop the player from moving
        RestrictPlayer();

        //If everything was destroyed, give a spefic UI
        if (obliterated)
        {
            ObliterateUI.SetActive(true);
            string achieveText = "You broke all " + (objsToDestroy - numWalls).ToString() + " objects!";
            string timeText = "It only took you " + ((int)(levelTime - levelTimer)).ToString() + " seconds!";
            string scoreText = "You scored all " + maxScore + " points!";

            ObliterateUI.GetComponent<LevelFinishUI>().updateText(achieveText, timeText,scoreText);

        }
        
    }

    public IEnumerator playOpeningDialogues()
    {
        if (!skipDialouge)
        {
            for (int i = 0; i < preStartDialouges; i++)
            {
                DialogueEffect currDia = dialogues[i];
                ShowNextDiaglogue(false);
                yield return new WaitForSeconds(currDia.duration + 0.1f);
            }
        }
        else
        {
            currDialogue += preStartDialouges;
        }


        StartGame();
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }

    public void UnPauseGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    private void RestrictPlayer()
    {
        playerMove.DisableMovement();
        playerAtk.DisableAtk();
    }

    private void FreePlayer()
    {
        playerMove.EnableMovement();
        playerAtk.EnableAtk();
    }
}
