using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform InventorySlot1;
    [SerializeField]
    private Transform InventorySlot2;
    [SerializeField]
    private Transform InventorySlot3;
    [SerializeField]
    private GameObject[] TileSet;

    public Toggle RerollToggle;
    public Image RerollBackground;
    public int RerollCount;

    public Toggle CrashToggle;
    public Image CrashBackground;
    public int CrashCount;

    private int tileCount = 0;

    void Start()
    {
        RerollToggle.onValueChanged.AddListener(Reroll);
        CrashToggle.onValueChanged.AddListener(OnToggleChanged);
        RerollCount = 3;
        CrashCount = 3;
    }

    public void Update()
    {
        if (tileCount == 0)
        {
            Generate();
        }

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    //���� ����� �������� �� �������� ���� 3�� �������. ī��Ʈ ���� �ҽǽ� ���� ������ �ٲ��
    public void Reroll(bool isOn)
    {
        if (RerollCount == 0)
        {
            Debug.Log("���� ����!");
        }
        else if (RerollCount == 1)
        {
            RerollCount--;

            deleteTile(InventorySlot1);
            deleteTile(InventorySlot2);
            deleteTile(InventorySlot3);

            Generate();

            RerollBackground.color = Color.red;
        }
        else
        {
            RerollCount--;

            deleteTile(InventorySlot1);
            deleteTile(InventorySlot2);
            deleteTile(InventorySlot3);

            Generate();
        }
    }


    //ũ���� ����� ���������� �������� ������ �Ķ������� ���� �� ����Ǿ��ִ� ���� ���� Ÿ���� ���� �� ���ο� Ÿ�� ����
    public void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("����� �������ϴ�!");
        }
        else
        {
            Debug.Log("����� �������ϴ�!");
        }
    }

    public void deleteTile(Transform slot)
    {
        Transform tile;

        if (slot.childCount > 0)
        {
            tile = slot.GetChild(0);

            minusTileCount();

            Destroy(tile.gameObject);
        }
    }

    public void minusTileCount()
    {
        tileCount -= 1;
    }

    public void Generate()
    {
        tileCount = 3;
        TileGenerate(InventorySlot1);
        TileGenerate(InventorySlot2);
        TileGenerate(InventorySlot3);
    }

    public void TileGenerate(Transform slot)
    {
        int randNum = Random.Range(1, 7);
        int newType;

        GameObject newTile = Instantiate(TileSet[randNum-1], slot);
        newTile.transform.SetParent(slot);

        switch (randNum)
        {
            case 1:
                newType = 10;
                break;
            case 2:
                newType = 5;
                break;
            case 3:
                newType = 6;
                break;
            case 4:
                newType = 12;
                break;
            case 5:
                newType = 9;
                break;
            case 6:
                newType = 3;
                break;
            default:
                newType = 0;
                break;
        }

        newTile.GetComponent<TileDraggable>().tileType = newType;
    }
}
