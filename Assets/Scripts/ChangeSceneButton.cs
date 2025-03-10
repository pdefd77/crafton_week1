using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void GotoScene(int idx)
    {
        GameManager.Instance.GotoScene(idx);
    }
}
