using TMPro;
using UnityEngine;

public class DisplayRecord : MonoBehaviour
{
    void Start()
    {
        Display();
    }

    public void Display()
    {
        transform.GetComponent<TextMeshProUGUI>().text = "플레이어 \'" + DataManager.Instance.playerData.playerName + "\'의 기록\n\n"
            + "최고 점수: " + DataManager.Instance.playerData.maxScore + "\n"
            + "플레이 타임: 미구현\n"
            + "누적 점수: " + DataManager.Instance.playerData.totalScore + "\n"
            + "누적 타일 수: " + DataManager.Instance.playerData.totalTile + "\n"
            + "누적 턴 수: " + DataManager.Instance.playerData.totalTurn;
    }
}
