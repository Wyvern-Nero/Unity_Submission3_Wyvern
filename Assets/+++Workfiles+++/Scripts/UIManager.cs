using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Input + Declarations
    
    //Panels
    [Header("Panels + Buttons")]
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject LosePanel;
    
    [SerializeField] private Button StartButton;

    [Header("Objects + Position Targets")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject collectible;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private GameObject cameraPosition;
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
    [SerializeField] private float savedScore;
    
    //Collectibles
    [Header("Collectibles")]
    [SerializeField] private int targetCounter = 5;
    [SerializeField] private int secretCounter = 1;
    
    [SerializeField] private TextMeshProUGUI textTargetCount;
    [SerializeField] private int savedTargetCount;
    [SerializeField] private TextMeshProUGUI textSecretCount;
    [SerializeField] private int savedSecretCount;
    
    [Header("DeathTime")]
    [SerializeField] private float deathTime;
    [SerializeField] public bool resetWorld;
    
    #endregion
    
    #region Start + Update
    private void Start()
    {
        savedScore = PlayerPrefs.GetFloat("savedScore");
        savedTargetCount = PlayerPrefs.GetInt("savedTargetCount");
        savedSecretCount = PlayerPrefs.GetInt("savedSecretCount");
        //ERASE ONCE TESTING IS DONE
        //ERASE ONCE TESTING IS DONE
        //ERASE ONCE TESTING IS DONE
        savedScore = 0;
        savedSecretCount = 0;
        savedTargetCount = 0;
        
        LoadGame();
        
        timer = 0;
        Countdown_obj.SetActive(false);
    }

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
        targetCounter = 0;
        secretCounter = 0;
        player.transform.position = Vector3.zero;
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

    // stops timer and saves it as savedScore if lower than previous savedScore / also set camera position to level
    public void StopTimer()
    {
        timerActive = false;
        HideUI();
        cameraFixedPosition();
        SaveScore();
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
    
    #region Saving
    private void SaveScore()
    {
        //savedScore = timer;
        //PlayerPrefs.SetFloat("savedScore", savedScore);
        //Debug.Log(message:"saving score = " + savedScore);
        //savedTargetCount = targetCounter;
        //PlayerPrefs.SetInt("targetCounter", targetCounter);
        //Debug.Log(message:"saving targets = " + savedTargetCount);
        //savedSecretCount = secretCounter;
        //PlayerPrefs.SetInt("secretCounter", secretCounter);
        //Debug.Log(message:"saving secrets = " + savedSecretCount);
    }
    #endregion

    #region Camera Target

    //sets camera to fixed position in world
    private void cameraFixedPosition()
    {
        cameraTarget.transform.SetParent(cameraPosition.transform);
        cameraTarget.transform.position = cameraPosition.transform.position;
    }

    //sets camera to player
    private void cameraOnPlayer()
    {
        cameraTarget.transform.SetParent(player.transform);
        cameraTarget.transform.position = player.transform.position;
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
    #endregion
}