using UnityEngine;

public class CollectManager : MonoBehaviour
{
    [SerializeField] private int targetCounter = 0;
    [SerializeField] private int secretCounter = 0;
    [SerializeField] private UIManager uiManager;

    public void AddTarget()
    {
        targetCounter++;
        uiManager.UpdateTextTargetCount(targetCounter);
    }
    public void AddSecret()
    {
        secretCounter++;
        uiManager.UpdateTextSecretCount(secretCounter);
    }
}
