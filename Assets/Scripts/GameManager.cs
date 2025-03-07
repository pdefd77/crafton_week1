using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // ΩÃ±€≈Ê

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartScene()
    {
        SoundManager.Instance.PlayDisplaySound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
