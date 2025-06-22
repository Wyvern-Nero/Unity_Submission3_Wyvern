using UnityEngine;

public class EndOfRun : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    public void endOfRun()
    {
        scoreManager.updatePB();
        Debug.Log(message:"End of Run");
    }
}
