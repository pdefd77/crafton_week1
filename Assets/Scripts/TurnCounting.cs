using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnCounting : MonoBehaviour
{
    [SerializeField] private LevelUpEffect levelUpEffect;
    public int turnCount;
    public int limitTurn;
    private int firstLimitTurn;
    public int goalScore;
    private int firstGoalScore;
    private int increaseMultiplier = 2;

    private int level = 1;

    [SerializeField] private RerollButton rerollButton;
    [SerializeField] private TextMeshProUGUI limitTurnText;
    [SerializeField] private TextMeshProUGUI goalScoreText;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        firstLimitTurn = limitTurn;
        firstGoalScore = goalScore;
        UpdateText();
    }

    // 씬이 로드될 때 변수 초기화
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIElements(); // 텍스트누락방지 요소 할당
        ResetVariables(); // 변수를 기본값으로 초기화
        UpdateText();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 변수 초기화 메서드
    private void ResetVariables()
    {
        turnCount = 0;
        level = 1;
        limitTurn = firstLimitTurn;
        goalScore = firstGoalScore;
        increaseMultiplier = 2;
    }

    private void AssignUIElements()
    {
        limitTurnText = GameObject.Find("TurnText")?.GetComponent<TextMeshProUGUI>();
        goalScoreText = GameObject.Find("GoalText")?.GetComponent<TextMeshProUGUI>();
    }

    public void CheckTrunAndGoal()
    {
        UpdateText();

        if (turnCount >= limitTurn)
        {
            if(BoardCheck.score < goalScore)
            {
                //game over
                BoardCheck.gameover = true;
            }
            else
            {
                //갱신
                limitTurn += firstLimitTurn;
                goalScore += firstGoalScore * increaseMultiplier;
                if (increaseMultiplier < 30)
                {
                    increaseMultiplier += 1;
                }

                levelUpEffect.CrackerShoot(level);
                level++;
                rerollButton.AddRerollCount();
                SoundManager.Instance.PlayLevelUpSound();
                UpdateText();
            }
        }
    }

    //텍스트 갱신
    private void UpdateText()
    {
        limitTurnText.text = "턴 수 " + turnCount + " / " + limitTurn;
        goalScoreText.text = "다음 목표 " + goalScore;
    }
}
