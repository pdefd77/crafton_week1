using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System;

public class BoardCheck : MonoBehaviour
{
    [SerializeField] private GameObject[] boardSlot;
    [SerializeField] private TurnCounting turnCounting;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI gameOverTxt;
    public static int score = 0;
    public static bool gameover = false;
    public int displayedTileCount = 0;
    private int[] uf = new int[49];

    private readonly int[] checkNum = new int[] { 1, 2, 3, 4, 5, 7, 13, 14, 20, 21, 27, 28, 34, 35, 41, 43, 44, 45, 46, 47 };

    public static int[,] adj = new int[7, 7];
    public static bool[,] adj2 = new bool[26, 26];

    private void Awake()
    {
        adj = new int[7, 7] { { 0, 4, 4, 4, 4, 4, 0 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 2, 0, 0, 0, 0, 0, 8 }, { 0, 1, 1, 1, 1, 1, 0 } };
        gameover = false;
        score = 0;
        scoreTxt.text = "Score : " + score;
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

    public void CheckEx(int idx, int tileType)
    {
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

        if(Dfs(0, -1, ref visited, ref cycleList, ref cycleQueue))
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

                DestroyTile(i / 5 + 1, i % 5);
            }
        }

        //턴 증가
        //turnCounting.turnCount++;
        //턴에 해당하는 점수 충족 여부 확인 및 게임 종료 결정
        turnCounting.CheckTrunAndGoal();

        if (displayedTileCount >= 25) // gameover
        {
            gameover = true;
        }
        if (gameover)
        {
            SoundManager.Instance.PlayGameOverSound();
            gameOverTxt.gameObject.SetActive(true);
            gameOverTxt.text = "Your Score is " + score;
        }

        scoreTxt.text = "Score : " + score;
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

    public void Check()
    {
        for (int i = 0; i < 49; i++) uf[i] = i;
        int cycleCheck = 0;

        // 연결하기
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                if ((adj[i, j] & 1) > 0 && (adj[i - 1, j] & 4) > 0) // 도로와 위쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j - 7);
                }
                if ((adj[i, j] & 2) > 0 && (adj[i, j + 1] & 8) > 0) // 도로와 오른쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j + 1);
                }
                if ((adj[i, j] & 4) > 0 && (adj[i + 1, j] & 1) > 0) // 도로와 아래쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j + 7);
                }
                if ((adj[i, j] & 8) > 0 && (adj[i, j - 1] & 2) > 0) // 도로와 왼쪽이 이어져 있는지
                {
                    UfMerge(7 * i + j, 7 * i + j - 1);
                }
            }
        }

        // 연결 확인
        for(int i = 0; i < 7; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                if (i > 0 && i < 6 && j > 0 && j < 6) continue;

                if (uf[7 * i + j] != 7 * i + j)
                {
                    cycleCheck = uf[7 * i + j];
                }
            }
        }

        if (cycleCheck > 0)
        {
            GetScore(cycleCheck);
        }

        //턴 증가
        turnCounting.turnCount++;
        //턴에 해당하는 점수 충족 여부 확인 및 게임 종료 결정
        turnCounting.CheckTrunAndGoal();

        if (displayedTileCount >= 25) // gameover
        {
            gameover = true;
        }
        if(gameover)
        {
            SoundManager.Instance.PlayGameOverSound();
            gameOverTxt.gameObject.SetActive(true);
            gameOverTxt.text = "Your Score is " + score;
        }

        scoreTxt.text = "Score : " + score;
    }

    private void UfMerge(int a, int b)
    {
        a = UfFind(a);
        b = UfFind(b);

        if (Array.Exists(checkNum, x => x == a))
        {
            uf[b] = a;
        }
        else
        {
            uf[a] = b;
        }
    }

    private int UfFind(int x)
    {
        if (uf[x] == x)
        {
            return x;
        }
        else
        {
            uf[x] = UfFind(uf[x]);
            return uf[x];
        }
    }

    private void GetScore(int num)
    {
        int len = 0;

        for(int i = 1; i <= 5; i++)
        {
            for(int j = 1; j <= 5; j++)
            {
                if (UfFind(uf[7 * i + j]) == num)
                {
                    DestroyTile(i, j);
                    len++;
                }
            }
        }

        // 점수 계산 : 배율 정해서. 이부분은 쉽게 수정되게. 배율변수 빼기.
        //displayedTileCount -= len;
        score += len * len * len;
    }

    private void DestroyTile(int y, int x)
    {
        adj[y, x] = 0;
        for (int i = 0; i <= 25; i++) adj2[5 * y - 5 + x, i] = false;
        displayedTileCount--;
        if (boardSlot[5 * y + x - 6].transform.childCount > 0)
        {
            boardSlot[5 * y + x - 6].transform.GetChild(0).GetComponent<TileDestroy>().StartBreak();
        }
    }
}
