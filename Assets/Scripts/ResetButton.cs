using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public void Reset()
    {
        GameManager.Instance.RestartScene();
    }
}