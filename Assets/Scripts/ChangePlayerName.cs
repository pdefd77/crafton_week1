using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        nameText.text = DataManager.Instance.playerData.playerName;
    }

    public void ChangeName(string name)
    {
        transform.GetComponent<TMP_InputField>().text = "";

        DataManager.Instance.playerData.playerName = name;
        nameText.text = DataManager.Instance.playerData.playerName;
    }
}
