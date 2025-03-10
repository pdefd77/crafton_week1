using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class BoardCheck : MonoBehaviour
{
    [SerializeField] private GameObject[] boardSlot;
    [SerializeField] private TurnCounting turnCounting;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI gameOverTxt;
    [SerializeField] private TextMeshProUGUI maxScoreTxt;
    public static int score = 0;
    public static bool gameover = false;
    private int displayedTileCount = 0;

    public static bool[,] adj2 = new bool[26, 26];

    private void Awake()
    {
        gameover = false;
        score = 0;
        scoreTxt.text = "현재 점수 " + score;
        GameObject boardInventory = GameObject.Find("BoardInventory");
        for (int i = 0; i < 25; i++)
        {
            boardSlot[i] = boardInventory.transform.GetChild(i).gameObject;
        }

        adj2 = new bool[26, 26];
        for (int i = 1; i <= 5; i++)
        {
            adj2[0, i] = true;
            adj2[0, 5 * i] = true;
            adj2[0, 19 + i] = true;
            adj2[0, 5 * i - 4] = true;
        }
    }

    private void Start()
    {
        DisplayMaxScore();
    }

    public void Check(int idx, int tileType)
    {
        displayedTileCount++;

        if ((tileType & 1) > 0) //위쪽과 이어짐
        {
            if (idx == 1 || idx == 2 || idx == 3 || idx == 4 || idx == 5) adj2[idx, 0] = true;
            else adj2[idx, idx - 5] = true;
        }
        if ((tileType & 2) > 0) // 오른쪽과 이어짐
        {
            if (idx == 5 || idx == 10 || idx == 15 || idx == 20 || idx == 25) adj2[idx, 0] = true;
            else adj2[idx, idx + 1] = true;
        }
        if ((tileType & 4) > 0) //아래쪽과 이어짐
        {
            if (idx == 21 || idx == 22 || idx == 23 || idx == 24 || idx == 25) adj2[idx, 0] = true;
            else adj2[idx, idx + 5] = true;
        }
        if ((tileType & 8) > 0) // 오른쪽과 이어짐
        {
            if (idx == 1 || idx == 6 || idx == 11 || idx == 16 || idx == 21) adj2[idx, 0] = true;
            else adj2[idx, idx - 1] = true;
        }

        bool[] visited = new bool[26];
        bool[] cycleList = new bool[26];
        Queue<int> cycleQueue = new();
        int cycleLength = 0;

        if(CornerCheck(idx, tileType, ref cycleList, ref cycleQueue) || Dfs(0, -1, ref visited, ref cycleList, ref cycleQueue))
        {
            while(cycleQueue.Count > 0)
            {
                for (int i = 0; i <= 25; i++) visited[i] = cycleList[i];
                if (cycleQueue.Peek() != 0) Dfs2(cycleQueue.Peek(), -1, ref visited, ref cycleList, ref cycleQueue);
                cycleQueue.Dequeue();
            }
            
            for(int i = 1; i <= 25; i++)
            {
                if (!cycleList[i]) continue;

                cycleLength++;
                DestroyTile(i / 5 + 1, i % 5);
            }
        }

        DataManager.Instance.playerData.totalTile += cycleLength;
        score += cycleLength * cycleLength * cycleLength;
        DataManager.Instance.playerData.totalScore += cycleLength * cycleLength * cycleLength;
        if (cycleLength >= 10 && !DataManager.Instance.playerData.achievement[0])
        {
            DataManager.Instance.playerData.achievement[0] = true;
            DataManager.Instance.playerData.achievementTime[0] = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss"));
        }

        //턴 증가
        turnCounting.turnCount++;
        DataManager.Instance.playerData.totalTurn++;
        //턴에 해당하는 점수 충족 여부 확인 및 게임 종료 결정
        turnCounting.CheckTrunAndGoal();

        if (displayedTileCount >= 25)
        {
            gameover = true;

            if (!DataManager.Instance.playerData.achievement[1])
            {
                DataManager.Instance.playerData.achievement[1] = true;
                DataManager.Instance.playerData.achievementTime[1] = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss"));
            }
        }
        if (gameover)
        {
            SoundManager.Instance.PlayGameOverSound();
            gameOverTxt.gameObject.SetActive(true);
            gameOverTxt.text = "당신의 점수는 " + score + "점입니다.";
        }

        scoreTxt.text = "현재 점수 " + score;
        DataManager.Instance.playerData.maxScore = Math.Max(DataManager.Instance.playerData.maxScore, score);
        DisplayMaxScore();
        if (score>=10000 && !DataManager.Instance.playerData.achievement[2])
        {
            DataManager.Instance.playerData.achievement[2] = true;
            DataManager.Instance.playerData.achievementTime[2] = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss"));
        }
    }

    // 외곽-외곽 경로 확인
    private bool Dfs(int now, int prev, ref bool[] visited, ref bool[] cycleList, ref Queue<int> cycleQueue)
    {
        visited[now] = true;
        cycleList[now] = true;

        for(int i = 25; i >= 0; i--)
        {
            if (i == prev) continue;
            if (!adj2[now, i] || !adj2[i, now]) continue;

            if (!visited[i] && Dfs(i, now, ref visited, ref cycleList, ref cycleQueue))
            {
                cycleQueue.Enqueue(now);
                return true;
            }
            else if (i == 0)
            {
                cycleQueue.Enqueue(now);
                return true;
            }
        }

        cycleList[now] = false;
        return false;
    }

    // 내부 추가 경로 확인
    private bool Dfs2(int now, int prev, ref bool[] visited, ref bool[] cycleList, ref Queue<int> cycleQueue)
    {
        visited[now] = true;
        cycleList[now] = true;

        for (int i = 25; i >= 0; i--)
        {
            if (i == prev) continue;
            if (!adj2[now, i] || !adj2[i, now]) continue;
            if (prev == -1 && cycleList[i]) continue;

            if (!visited[i] && Dfs2(i, now, ref visited, ref cycleList, ref cycleQueue))
            {
                cycleQueue.Enqueue(now);
                return true;
            }
            else if (cycleList[i])
            {
                cycleQueue.Enqueue(now);
                return true;
            }
        }

        if (prev != -1) cycleList[now] = false;
        return false;
    }

    // 가장자리끼리 이어지는 엣지케이스 체크
    private bool CornerCheck(int idx, int tileType, ref bool[] cycleList, ref Queue<int> cycleQueue)
    {
        if (idx == 1 && (tileType & 1) > 0 && (tileType & 8) > 0) // 왼쪽위
        {
            cycleList[1] = true;
            cycleQueue.Enqueue(1);
        }
        else if(idx == 5 && (tileType & 1) > 0 && (tileType & 2) > 0) // 오른쪽위
        {
            cycleList[5] = true;
            cycleQueue.Enqueue(5);
        }
        else if (idx == 21 && (tileType & 4) > 0 && (tileType & 8) > 0) // 왼쪽아래
        {
            cycleList[21] = true;
            cycleQueue.Enqueue(21);
        }
        else if (idx == 25 && (tileType & 4) > 0 && (tileType & 2) > 0) // 오른쪽아래
        {
            cycleList[25] = true;
            cycleQueue.Enqueue(25);
        }
        else
        {
            return false;
        }

        return true;
    }

    private void DestroyTile(int y, int x)
    {
        for (int i = 0; i <= 25; i++) adj2[5 * y - 5 + x, i] = false;
        displayedTileCount--;
        if (boardSlot[5 * y + x - 6].transform.childCount > 0)
        {
            boardSlot[5 * y + x - 6].transform.GetChild(0).GetComponent<TileDestroy>().StartBreak();
        }
    }

    private void DisplayMaxScore()
    {
        maxScoreTxt.text = "최고 점수 " + DataManager.Instance.playerData.maxScore;
    }
}
