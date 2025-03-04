using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class pdefd77_TileGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform InventorySlot1;
    [SerializeField]
    private Transform InventorySlot2;
    [SerializeField]
    private Transform InventorySlot3;

    public GameObject tile;

    private int tileCount = 0;

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
        GameObject newTile = Instantiate(tile, slot);
        newTile.transform.SetParent(slot);
        TextMeshProUGUI road = newTile.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        int randNum = Random.Range(1, 7);
        int newType;

        switch (randNum)
        {
            case 1:
                newType = 10;
                road.text = "����\n����\n����";
                break;
            case 2:
                newType = 5;
                road.text = "����\n����\n����";
                break;
            case 3:
                newType = 6;
                road.text = "����\n����\n����";
                break;
            case 4:
                newType = 12;
                road.text = "����\n����\n����";
                break;
            case 5:
                newType = 9;
                road.text = "����\n����\n����";
                break;
            case 6:
                newType = 3;
                road.text = "����\n����\n����";
                break;
            default:
                newType = 0;
                break;
        }

        newTile.GetComponent<pdefd77_TileDraggable>().tileType = newType;
    }
}
