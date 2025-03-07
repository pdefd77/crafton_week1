using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // 싱글톤

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip displaySound;
    [SerializeField] private AudioClip levelUpSound;
    [SerializeField] private AudioClip forbidSound;
    [SerializeField] private AudioClip gameOverSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 사운드 재생 메서드
    public void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null || audioSource == null) return;

        audioSource.PlayOneShot(clip,volume);

    }

    public void PlayDisplaySound()
    {
        PlaySound(displaySound,1.0f);
    }

    public void PlayLevelUpSound()
    {
        PlaySound(levelUpSound,1.0f);
    }

    public void PlayForbidSound()
    {
        PlaySound(forbidSound, 0.5f);
    }

    public void PlayGameOverSound()
    {
        PlaySound(gameOverSound, 0.6f);
    }
}
