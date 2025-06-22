using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class UIManager : MonoBehaviour
{
    #region Input + Declarations
    
    //Panels
    [Header("ScriptReferences")]
    [SerializeField] private PlayerController player;
    [SerializeField] private EndOfRun messenger;
    
    [Header("Panels + Buttons")]
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject LosePanel;
    
    [SerializeField] private Button StartButton;

    [Header("Objects + Position Targets")]
    [SerializeField] private GameObject hubWall;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject collectible;
    [SerializeField] private GameObject teleportTarget;
    [SerializeField] private GameObject target1Position;
    [SerializeField] private GameObject target2Position;
    [SerializeField] private GameObject target3Position;
    [SerializeField] private GameObject target4Position;
    [SerializeField] private GameObject target5Position;
    [SerializeField] private GameObject collectiblePosition;
    
    //Countdown
    [Header("Countdown")]
    [SerializeField] private bool countdownActive;
    [SerializeField] private float countdown;
    private int countdownSeconds;
    [SerializeField] private TextMeshProUGUI textCountdown;
    [SerializeField] private GameObject Countdown_obj;
    
    //Score
    [Header("Stopwatch")]
    [SerializeField] private bool timerActive;
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] public float tmpScore;
    
    //Collectibles
    [Header("Collectibles")]
    [SerializeField] public int targetCounter;
    [SerializeField] public int secretCounter;
    
    [SerializeField] private TextMeshProUGUI textTargetCount;
    [SerializeField] private TextMeshProUGUI textSecretCount;
    
    [Header("DeathTime")]
    [SerializeField] private float deathTime;
    [SerializeField] public bool resetWorld;
    
    [Header("Camera")]
    [SerializeField] private GameObject camera;
    //[SerializeField] private GameObject CineMachine;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private GameObject cameraTargetPlayer;
    [SerializeField] private GameObject cameraPosition;
    
    #endregion
    
    #region Start + Update
    private void Start()
    {
        LoadGame();
        
        timer = 0;
        Countdown_obj.SetActive(false);
    }
    
    //contains timer + countdown function
    void Update()
    {
        //timer + countdown
        #region timer + countdown
        //timer to keep track of how quick the player is
        if (timerActive)
        {
            timer += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(timer);
        int fractional = time.Milliseconds / 10;
        textTimer.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
        
        //countdown stop the player from moving before the timer starts
        if (countdownActive)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                StopCountdown();
            }
        }
        textCountdown.text = countdown.ToString("F2");
        #endregion
    }
    #endregion
    
    #region Panels

    public void LoadGame()
    {
        StartPanel.SetActive(true);
        GamePanel.SetActive(false);
        LosePanel.SetActive(false);
        player.canMove = false;
        StartButton.onClick.AddListener(HideUI);
        cameraFixedPosition();
    }
    public void HideUI()
    {
        StartPanel.SetActive(false);
        GamePanel.SetActive(false);
        LosePanel.SetActive(false);
        player.canMove = true;
        cameraFixedPosition();
    }
    public void StartParkour()
    {
        StartPanel.SetActive(false);
        GamePanel.SetActive(true);
        LosePanel.SetActive(false);
        
        player.transform.position = teleportTarget.transform.position;
        cameraOnPlayer();
        SpawnObjects();
        
        hubWall.SetActive(false);
        
        timer = 0;
        targetCounter = 0;
        UpdateTextTargetCount(targetCounter);
        secretCounter = 0;
        UpdateTextSecretCount(targetCounter);
        
    }
    public void ShowLosePanel()
    {
        StartPanel.SetActive(false);
        GamePanel.SetActive(false);
        LosePanel.SetActive(true);
        player.canMove = false;
        timerActive = false;
        StartCoroutine(DeathTime());
    }
    
    IEnumerator DeathTime()
    {
        yield return new WaitForSecondsRealtime(deathTime);
        HideUI();
        player.transform.position = Vector3.zero;
        timer = 0;
        targetCounter = 0;
        secretCounter = 0;
    }
    
    #endregion

    #region Countdown + Timer
    
    //starts countdown until player can move, moves camera to player, moves player to starting line
    public void StartCountdown()
    {
        countdownActive = true;
        player.canMove = false;
        Countdown_obj.SetActive(true);
        StartParkour();
    }

    // stops timer
    public void StopCountdown()
    {
        countdown = 0;
        countdownActive = false;
        Countdown_obj.SetActive(false);
        StartTimer();
    }
    
    //start stopwatch time and allow player to move
    public void StartTimer()
    {
        countdown = 3;
        timer = 0;
        timerActive = true;
        player.canMove = true;
    }

    // stops timer, set camera position to level, save time to tmpScore
    public void StopTimer()
    {
        timerActive = false;
        HideUI();
        cameraFixedPosition();
        tmpScore = timer;
        messenger.endOfRun();
    }
    
    #endregion
    
    #region Collectible
    public void AddTarget()
    {
        targetCounter++;
        UpdateTextTargetCount(targetCounter);
    }
    public void AddSecret()
    {
        secretCounter++;
        UpdateTextSecretCount(secretCounter);
    }
    
    public void UpdateTextTargetCount(int newTargetCount)
    {
        textTargetCount.text = newTargetCount.ToString();
    }

    public void UpdateTextSecretCount(int newSecretCount)
    {
        textSecretCount.text = newSecretCount.ToString();
    }
    #endregion

    #region Camera Target

    //sets camera to fixed position in world
    private void cameraFixedPosition()
    {
        cameraTarget.transform.SetParent(cameraPosition.transform);
        cameraTarget.transform.position = cameraPosition.transform.position;
        //CineMachine.gameObject.SetActive(false);
        camera.transform.position = new Vector3( 0, 3.5f, camera.transform.position.z);
    }

    //sets camera to player
    private void cameraOnPlayer()
    {
        cameraTarget.transform.SetParent(cameraTargetPlayer.transform);
        cameraTarget.transform.position = cameraTargetPlayer.transform.position;
        //CineMachine.gameObject.SetActive(true);
    }

    #endregion
    
    #region spawn target+collectible
    void SpawnObjects()
    {
        //duplicate and set position of targets and collectibles, set position to targetXPosition
        GameObject target1 = Instantiate(target); target1.transform.position = target1Position.transform.position;
        GameObject target2 = Instantiate(target); target2.transform.position = target2Position.transform.position;
        GameObject target3 = Instantiate(target); target3.transform.position = target3Position.transform.position;
        GameObject target4 = Instantiate(target); target4.transform.position = target4Position.transform.position;
        GameObject target5 = Instantiate(target); target5.transform.position = target5Position.transform.position;
        
        GameObject collectible1 = Instantiate(collectible); collectible1.transform.position = collectiblePosition.transform.position;
        resetWorld = true;
    }

    public void LockHub()
    {
        hubWall.SetActive(true);
    }
    
    #endregion
}