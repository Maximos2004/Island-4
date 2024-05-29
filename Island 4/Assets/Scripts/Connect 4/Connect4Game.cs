using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Connect4Game : MonoBehaviour
{
    public int[,] grid = new int[6, 7]; // 6 rows, 7 columns
    public bool Turn; // True is Player1, False is Player2
    public bool VsCloudy; // True is vs the AI

    [SerializeField] private List<GameObject> GridCells = new List<GameObject>();

    public TextMeshProUGUI winCounterP1, winCounterP2;
    private int winCount1, winCount2;
    [SerializeField] private GameObject P1Wins, P2Wins;

    [SerializeField] private List<Token> Collumes = new List<Token>();

    [HideInInspector] public bool Droping;

    public void AIPlays(int Placement)
    {
        Debug.Log("AI is attempting to play in column: " + Placement);

        // Check if the Placement is within the range of the list
        if (Placement >= 0 && Placement < Collumes.Count)
        {
            Debug.Log("Placement is within range.");

            // Get the Token at the specified Placement
            Token token = Collumes[Placement];

            if (token != null)
            {
                Debug.Log("Token retrieved successfully. Starting AIPlays coroutine.");
                // Start the coroutine on the Token
                StartCoroutine(token.AIPlays());
            }
            else
            {
                Debug.LogError("Token retrieval failed. Token is null.");
            }
        }
        else
        {
            Debug.LogError("Placement out of range: " + Placement);
        }
    }


    public void AddToWinCounterP1()
    {
        winCount1++;
        winCounterP1.text = winCount1.ToString();
        StartCoroutine(ResetGame());
    }
    public void AddToWinCounterP2()
    {
        winCount2++;
        winCounterP2.text = winCount2.ToString();
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = 0;
            }
        }

        foreach (GameObject gridCell in GridCells)
        {
            TokenGridSpot tokenGridSpot = gridCell.GetComponent<TokenGridSpot>();
            tokenGridSpot.IsSpotTaken = false;
            tokenGridSpot.IsPlayer1 = false;
            tokenGridSpot.UpdateTokenDisplay();
        }
    }

    public void PvPWins(GameObject PlayerWinScreen)
    {
        PlayerWinScreen.SetActive(true);
    }

    public void ResetCounter()
    {
        winCount2 = 0;
        winCount1 = 0;
        winCounterP2.text = winCount2.ToString();
        winCounterP1.text = winCount2.ToString();
        StartCoroutine(ResetGame());
    }


    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                grid[i, j] = 0; // 0 for empty, 1 for Player1, 2 for Player2
            }
        }
    }

    public bool CheckWinCondition(int row, int column)
    {
        int player = grid[row, column];
        return (CheckDirection(row, column, 1, 0, player) + CheckDirection(row, column, -1, 0, player) >= 3 ||
                CheckDirection(row, column, 0, 1, player) + CheckDirection(row, column, 0, -1, player) >= 3 ||
                CheckDirection(row, column, 1, 1, player) + CheckDirection(row, column, -1, -1, player) >= 3 ||
                CheckDirection(row, column, 1, -1, player) + CheckDirection(row, column, -1, 1, player) >= 3);
    }

    int CheckDirection(int row, int column, int dRow, int dCol, int player)
    {
        int count = 0;
        for (int i = 1; i < 4; i++)
        {
            int newRow = row + i * dRow;
            int newCol = column + i * dCol;
            if (newRow < 0 || newRow >= 6 || newCol < 0 || newCol >= 7 || grid[newRow, newCol] != player)
            {
                break;
            }
            count++;
        }
        return count;
    }
}
