using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName = "PLAYERNAME"; // 사용자 이름
    public int maxScore = 0; // 최고 점수
    public int totalScore = 0; // 누적 점수
    public int totalTurn = 0; // 누적 턴 수
    public int totalTile = 0; // 누적 부순 타일 수
    public bool[] achievement = new bool[] { false, false, false }; // 도전과제 / 0: 10개짜리 완성 / 1: 25개게임오버 / 2: 10000점 달성
    public string[] achievementTime = new string[3]; // 도전과제 달성시간
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; } // 싱글턴

    private string filePath;
    public PlayerData playerData;

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

        playerData = LoadData();

        playerData ??= new PlayerData();
    }

    public void OnApplicationQuit()
    {
        SaveData(playerData);
    }

    public void SaveData(PlayerData data)
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.bin");

        BinaryFormatter formatter = new();
        using FileStream stream = new(filePath, FileMode.Create);
        formatter.Serialize(stream, data);
    }

    private PlayerData LoadData()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.bin");

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(filePath, FileMode.Open);
            return (PlayerData)formatter.Deserialize(stream);
        }

        return null;
    }

    public void ResetData()
    {
        playerData = new PlayerData();
    }
}
