using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Input + Declarations
    
    //Panels
    [Header("Panels")]
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;
    
    //Buttons
    [Header("Buttons)")]
    [SerializeField] private Button StartButton;

    [Header("Player")]
    [SerializeField] private PlayerController player;
    
    //Score
    [SerializeField] private bool timerActive;
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private float savedScore;
    
    //Collectibles
    [SerializeField] private int targetCounter = 0;
    [SerializeField] private int secretCounter = 0;
    
    [SerializeField] private TextMeshProUGUI textTargetCount;
    [SerializeField] private int savedTargetCount;
    [SerializeField] private TextMeshProUGUI textSecretCount;
    [SerializeField] private int savedSecretCount;
    
    #endregion
    
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
        
        StartPanel.SetActive(true);
        GamePanel.SetActive(false);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        player.canMove = false;
        
        StartButton.onClick.AddListener(StartGame);

        timer = 0;
    }

    void Update()
    {
        if (timerActive)
        {
            timer = timer + Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(timer);
        
        textTimer.text = time.Minutes.ToString() + "." + time.Seconds.ToString() + "." + time.Milliseconds.ToString();
        
    }
    
    #region Panels
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        StartPanel.SetActive(false);
        GamePanel.SetActive(true);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        player.canMove = true;
    }
    public void ShowWinPanel()
    {
        StartPanel.SetActive(false);
        GamePanel.SetActive(false);
        WinPanel.SetActive(true);
        LosePanel.SetActive(false);
    }
    
    public void ShowLosePanel()
    {
        StartPanel.SetActive(false);
        GamePanel.SetActive(false);
        WinPanel.SetActive(false);
        LosePanel.SetActive(true);
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

    #region Timer
    
    public void StartTimer()
    {
        timer = 0;
        timerActive = true;
    }

    // stops timer and saves it as savedScore if lower than previous savedScore
    public void StopTimer()
    {
        timerActive = false;
        
        //if (timer < savedScore)
        //{
        // SaveScore();   
        //}
        SaveScore();
    }
    
    #endregion
    
    private void SaveScore()
    {
        savedScore = timer;
        PlayerPrefs.SetFloat("savedScore", savedScore);
        Debug.Log(message:"saving score = " + savedScore);
        savedTargetCount = targetCounter;
        PlayerPrefs.SetInt("targetCounter", targetCounter);
        Debug.Log(message:"saving targets = " + savedTargetCount);
        savedSecretCount = secretCounter;
        PlayerPrefs.SetInt("secretCounter", secretCounter);
        Debug.Log(message:"saving secrets = " + savedSecretCount);
    }
}