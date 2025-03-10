using UnityEngine;
using UnityEngine.UI;

public class DisplayAcheivement : MonoBehaviour
{
    void Start()
    {
        Display();
    }

    public void Display()
    {
        string str = "도전과제\n\n10줄 이상의 도로를 완성하세요\n";
        if (DataManager.Instance.playerData.achievement[0])
        {
            str += DataManager.Instance.playerData.achievementTime[0] + "에 달성";
        }
        else
        {
            str += "(미달성)";
        }
        str += "\n\n판이 가득 차 게임오버 당하세요\n";
        if (DataManager.Instance.playerData.achievement[1])
        {
            str += DataManager.Instance.playerData.achievementTime[1] + "에 달성";
        }
        else
        {
            str += "(미달성)";
        }
        str += "\n\n10000점을 달성하세요\n";
        if (DataManager.Instance.playerData.achievement[2])
        {
            str += DataManager.Instance.playerData.achievementTime[2] + "에 달성";
        }
        else
        {
            str += "(미달성)";
        }

        transform.GetComponent<Text>().text = str;
    }
}
