using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RerollButton : MonoBehaviour
{
    [SerializeField] private TileGenerator tileGenerator;
    private int rerollCount;

    private void Awake()
    {
        rerollCount = 3;
    }

    public void AddRerollCount()
    {
        rerollCount++;

        transform.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll\n(" + rerollCount + "회 남음)";
    }

    public void Reroll()
    {
        if(rerollCount == 0)
        {
            SoundManager.Instance.PlayForbidSound();
        }
        else
        {
            SoundManager.Instance.PlayDisplaySound();
            rerollCount--;

            tileGenerator.Reroll();

            if(rerollCount == 0)
            {
                transform.GetComponent<Image>().color = Color.red;
            }
        }

        transform.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll\n(" + rerollCount + "회 남음)";
    }
}
