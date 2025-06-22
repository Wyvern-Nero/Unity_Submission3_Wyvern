using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   #region Definitions Score
   [Header("Variables")]
   [SerializeField] private float timePenaltyModifier;
   [SerializeField] private float timeRewardModifier;
   
   [Header("Other Components")]
   [SerializeField] private UIManager uiManager;
   
   [Header("Last Run Panel")]
   [SerializeField] private TextMeshPro textScore;
   [SerializeField] private TextMeshPro textScorePB;
   [SerializeField] private int targetCounter;
   [SerializeField] private TextMeshPro textTarget;
   [SerializeField] private TextMeshPro textPenalty;
   [SerializeField] private int secretCounter;
   [SerializeField] private TextMeshPro textSecret;
   [SerializeField] private TextMeshPro textReward;
   
   [Header("Debug")]
   [SerializeField] private float tmpScore;
   [SerializeField] private float modScore;
   [SerializeField] private float ScorePB;
   [SerializeField] private float timePenalty;
   [SerializeField] private float timeReward;
   public bool noModifer ;
   #endregion
   
   void Start()
   {
      noModifer = true;
      leaderBoardScores();
      scoreBoardPositions();
      ScorePB = 120;
   }

   void Update()
   {
      if (tmpScore != 0)
      {
         noModifer = false;
      }
      
      //scorePenalty();
      //scoreReward();
      //modifyScore();
      
      LastRunTime();
      lastRunTargets();
      lastRunSecret();
   }
   
   #region LastRunPanel
   
   private void LastRunTime()
   {
      if (noModifer)
      {
         TimeSpan time = TimeSpan.FromSeconds(0);
         int fractional = time.Milliseconds / 10;
         textScore.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);  
      }
      else
      {
         TimeSpan time = TimeSpan.FromSeconds(modScore);
         int fractional = time.Milliseconds / 10;
         textScore.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
      }
   }

   private void RunTimePB()
   {
      {
         TimeSpan time = TimeSpan.FromSeconds(ScorePB);
         int fractional = time.Milliseconds / 10;
         textScorePB.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
      }
   }
   
   private void lastRunTargets()
   {
      textTarget.text = targetCounter.ToString();

      if (noModifer)
      {
         TimeSpan time = TimeSpan.FromSeconds(0);
         int fractional = time.Milliseconds / 10;
         textPenalty.text = string.Format("+ {0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
      }
      else
      {
         TimeSpan time = TimeSpan.FromSeconds(timePenalty);
         int fractional = time.Milliseconds / 10;
         textPenalty.text = string.Format("+ {0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
      }
   }

   private void lastRunSecret()
   {
      textSecret.text = secretCounter.ToString();
      if (noModifer)
      {
         TimeSpan time = TimeSpan.FromSeconds(0);
         int fractional = time.Milliseconds / 10;
         textReward.text = string.Format("- {0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
      }
      else
      {
         TimeSpan time = TimeSpan.FromSeconds(timeReward);
         int fractional = time.Milliseconds / 10;
         textReward.text = string.Format("- {0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);  
      }
   }

   private void rankPlayertext()
   {
      TimeSpan time = TimeSpan.FromSeconds(ScorePB);
      int fractional = time.Milliseconds / 10;
      rankPlayerText.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   #endregion
   
   #region modifers
   private void scorePenalty()
   {
      timePenalty = (5-targetCounter)*timePenaltyModifier;
   }

   private void scoreReward()
   {
      timeReward = secretCounter * timeRewardModifier;
   }

   private void modifyScore()
   {
      modScore = tmpScore + timePenalty - timeReward;
      if (modScore < 0)
      {
         modScore = 0;
      }
   }
   #endregion
   
   public void updatePB()
   {
      tmpScore = uiManager.tmpScore;
      targetCounter = uiManager.targetCounter;
      secretCounter = uiManager.secretCounter;
      scorePenalty();
      scoreReward();
      modifyScore();
      Debug.Log(message:"updatePB_test" + ScorePB + " +modscore  " + modScore);
      if (modScore < ScorePB)
      {
         ScorePB = modScore;
         compareScore();
         Debug.Log(message:"updatePB" + ScorePB + " + modscore " + modScore);
      }
      RunTimePB();
      rankPlayertext();
   }
   
   #region Scoreboard

   #region scoreboard input
   [Header("ranking")]
   [SerializeField] private GameObject rank1;
   [SerializeField] private TextMeshPro rank1Text;
   [SerializeField] private float rank1_value;
   [SerializeField] private GameObject rank2;
   [SerializeField] private TextMeshPro rank2Text;
   [SerializeField] private float rank2_value;
   [SerializeField] private GameObject rank3;
   [SerializeField] private TextMeshPro rank3Text;
   [SerializeField] private float rank3_value;
   [SerializeField] private GameObject rank4;
   [SerializeField] private TextMeshPro rank4Text;
   [SerializeField] private float rank4_value;
   [SerializeField] private GameObject rank5;
   [SerializeField] private TextMeshPro rank5Text;
   [SerializeField] private float rank5_value;
   [SerializeField] private GameObject rank6;
   [SerializeField] private TextMeshPro rank6Text;
   [SerializeField] private float rank6_value;
   [SerializeField] private GameObject rank7;
   [SerializeField] private TextMeshPro rank7Text;
   [SerializeField] private float rank7_value;
   [SerializeField] private GameObject rank8;
   [SerializeField] private TextMeshPro rank8Text;
   [SerializeField] private float rank8_value;
   [SerializeField] private GameObject rank9;
   [SerializeField] private TextMeshPro rank9Text;
   [SerializeField] private float rank9_value;
   [SerializeField] private GameObject rank10;
   [SerializeField] private TextMeshPro rank10Text;
   [SerializeField] private float rank10_value;
   [SerializeField] private GameObject rankPlayer;
   [SerializeField] private TextMeshPro rankPlayerText;


   #endregion
   
   #region scoreboard positions
   private Vector2 rank1_position;
   private Vector2 rank2_position;
   private Vector2 rank3_position;
   private Vector2 rank4_position;
   private Vector2 rank5_position;
   private Vector2 rank6_position;
   private Vector2 rank7_position;
   private Vector2 rank8_position;
   private Vector2 rank9_position;
   private Vector2 rank10_position;
   
   private void scoreBoardPositions()
   {
      rank1_position = rank1Text.transform.position;
      rank2_position = rank2Text.transform.position;
      rank3_position = rank3Text.transform.position;
      rank4_position = rank4Text.transform.position;
      rank5_position = rank5Text.transform.position;
      rank6_position = rank6Text.transform.position;
      rank7_position = rank7Text.transform.position;
      rank8_position = rank8Text.transform.position;
      rank9_position = rank9Text.transform.position;
      rank10_position = rank10Text.transform.position;
   }
   
   #endregion
   
   #region rankPlayer

   private void compareScore()
   {
      if (ScorePB <= rank1_value)
      {
         rankPlayer.transform.position = rank1_position;
         rank1Text.transform.position = rank2_position;
         rank2Text.transform.position = rank3_position;
         rank3Text.transform.position = rank4_position;
         rank4Text.transform.position = rank5_position;
         rank5Text.transform.position = rank6_position;
         rank6Text.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"1");
      }
      if (ScorePB < rank2_value && ScorePB > rank1_value)
      {
         rankPlayer.transform.position = rank2_position;
         rank2Text.transform.position = rank3_position;
         rank3Text.transform.position = rank4_position;
         rank4Text.transform.position = rank5_position;
         rank5Text.transform.position = rank6_position;
         rank6Text.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"2");
      }
      if (ScorePB < rank3_value && ScorePB > rank2_value)
      {
         rankPlayer.transform.position = rank3_position;
         rank3Text.transform.position = rank4_position;
         rank4Text.transform.position = rank5_position;
         rank5Text.transform.position = rank6_position;
         rank6Text.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"3");
      } 
      if (ScorePB < rank4_value && ScorePB > rank3_value)
      {
         rankPlayer.transform.position = rank4_position;
         rank4Text.transform.position = rank5_position;
         rank5Text.transform.position = rank6_position;
         rank6Text.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"4");
      } 
      if (ScorePB < rank5_value && ScorePB > rank4_value)
      {
         rankPlayer.transform.position = rank5_position;
         rank5Text.transform.position = rank6_position;
         rank6Text.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"5");
      } 
      if (ScorePB < rank6_value && ScorePB > rank5_value)
      {
         rankPlayer.transform.position = rank6_position;
         rank6Text.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"6");
      } 
      if (ScorePB < rank7_value && ScorePB > rank6_value)
      {
         rankPlayer.transform.position = rank7_position;
         rank7Text.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"7");
      } 
      if (ScorePB < rank8_value && ScorePB > rank7_value)
      {
         rankPlayer.transform.position = rank8_position;
         rank8Text.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"8");
      } 
      if (ScorePB < rank9_value && ScorePB > rank8_value)
      {
         rankPlayer.transform.position = rank9_position;
         rank9Text.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"9");
      } 
      if (ScorePB < rank10_value && ScorePB > rank10_value)
      {
         rankPlayer.transform.position = rank10_position;
         rank10Text.gameObject.SetActive(false);
         Debug.Log(message:"10");
      }
   }
   
   #endregion
   
   #region rankBoard
   //there has to be a better way, but i don't know it
   private void leaderBoardScores()
   {
      rank1text();
      rank2text();
      rank3text();
      rank4text();
      rank5text();
      rank6text();
      rank7text();
      rank8text();
      rank9text();
      rank10text();
      rankPlayertext();
   }

   private void rank1text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank1_value);
      int fractional = time.Milliseconds / 10;
      rank1Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank2text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank2_value);
      int fractional = time.Milliseconds / 10;
      rank2Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank3text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank3_value);
      int fractional = time.Milliseconds / 10;
      rank3Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank4text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank4_value);
      int fractional = time.Milliseconds / 10;
      rank4Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank5text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank5_value);
      int fractional = time.Milliseconds / 10;
      rank5Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank6text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank6_value);
      int fractional = time.Milliseconds / 10;
      rank6Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank7text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank7_value);
      int fractional = time.Milliseconds / 10;
      rank7Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank8text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank8_value);
      int fractional = time.Milliseconds / 10;
      rank8Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank9text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank9_value);
      int fractional = time.Milliseconds / 10;
      rank9Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   private void rank10text()
   {
      TimeSpan time = TimeSpan.FromSeconds(rank10_value);
      int fractional = time.Milliseconds / 10;
      rank10Text.text = string.Format("{0}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, fractional);
   }
   #endregion
   
   #endregion
}
