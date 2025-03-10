using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.Instance.GotoScene(1); // 1 = Game
    }
}
