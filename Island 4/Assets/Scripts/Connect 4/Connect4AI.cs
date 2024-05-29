using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Connect4AI : MonoBehaviour
{
    [SerializeField] private Connect4Game connect4Game;
    public int maxDepth = 5; // Adjust the depth for difficulty

    [Header("Coudy")]
    [SerializeField] private Animator CloudyAn;
    [SerializeField] private Image Cloudy;
    [SerializeField] private Sprite CloudyIdle, CloudySurprised, CloudySmug, CloudyThinking;

    private bool x;

    public IEnumerator MakeMove()
    {
        maxDepth = Random.Range(1, 7);

        Cloudy.sprite = CloudyThinking;
        CloudyAn.Play("Cloudy Pop", 0, 0);

        yield return new WaitForSeconds(Random.Range(0.2f, 2f));

        int bestMove = GetBestMove();
        Debug.Log("Best move calculated: " + bestMove);
        if (bestMove != -1)
        {
            connect4Game.AIPlays(bestMove);
        }
        else
        {
            Debug.LogError("Invalid move calculated: " + bestMove);
            Debug.Log("Making Random Move");
            connect4Game.AIPlays(Random.Range(0, 7));
        }

        
        CloudyAn.Play("Cloudy Going Idle", 0, 0);
    }

    private int GetBestMove()
    {
        int bestScore = int.MinValue;
        int bestColumn = -1;

        for (int col = 0; col < 7; col++)
        {
            int row = GetNextOpenRow(col);
            if (row != -1)
            {
                connect4Game.grid[row, col] = 2;
                int score = Minimax(connect4Game.grid, maxDepth, int.MinValue, int.MaxValue, false);
                connect4Game.grid[row, col] = 0;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestColumn = col;
                }
            }
        }

        return bestColumn;
    }

    private int Minimax(int[,] grid, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        if (depth == 0 || IsTerminalNode(grid))
        {
            return EvaluateGrid(grid);
        }

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            for (int col = 0; col < 7; col++)
            {
                int row = GetNextOpenRow(col);
                if (row != -1)
                {
                    grid[row, col] = 2;
                    int eval = Minimax(grid, depth - 1, alpha, beta, false);
                    grid[row, col] = 0;
                    maxEval = Mathf.Max(maxEval, eval);
                    alpha = Mathf.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            for (int col = 0; col < 7; col++)
            {
                int row = GetNextOpenRow(col);
                if (row != -1)
                {
                    grid[row, col] = 1;
                    int eval = Minimax(grid, depth - 1, alpha, beta, true);
                    grid[row, col] = 0;
                    minEval = Mathf.Min(minEval, eval);
                    beta = Mathf.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            return minEval;
        }
    }

    private bool IsTerminalNode(int[,] grid)
    {
        // Check for a win or a full grid
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (grid[row, col] != 0 && connect4Game.CheckWinCondition(row, col))
                {
                    return true;
                }
            }
        }

        for (int col = 0; col < 7; col++)
        {
            if (grid[0, col] == 0)
            {
                return false;
            }
        }

        return true;
    }
    
    private int EvaluateGrid(int[,] grid)
    {
        x = false;
        int score = 0;
        // Debugging: Check for immediate win or block opportunities
        for (int col = 0; col < 7; col++)
        {
            int row = GetNextOpenRow(col);
            if (row != -1)
            {
                grid[row, col] = 2;
                if (connect4Game.CheckWinCondition(row, col))
                {
                    Debug.Log("AI can win by playing in column " + col);
                    Cloudy.sprite = CloudySmug;
                    grid[row, col] = 0;
                    x = true;
                    score = int.MaxValue;
                }
                grid[row, col] = 0;

                grid[row, col] = 1;
                if (connect4Game.CheckWinCondition(row, col))
                {
                    Debug.Log("Opponent can win; AI should block column " + col);
                    Cloudy.sprite = CloudySurprised;
                    grid[row, col] = 0;
                    x = true;
                    score = int.MinValue;
                }
                grid[row, col] = 0;
            }
        }
        if (!x)
        {
            // Simple evaluation function to score the grid
            score = 0;
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (grid[row, col] == 2)
                    {
                        score += GetScoreForCell(grid, row, col, 2);
                        Cloudy.sprite = CloudyIdle;
                    }
                    else if (grid[row, col] == 1)
                    {
                        score -= GetScoreForCell(grid, row, col, 1);
                        Cloudy.sprite = CloudyIdle;
                    }
                }
            }
        }
        else
        {
            x = false;
        }
        return score;
    }

    private int GetScoreForCell(int[,] grid, int row, int col, int player)
    {
        int score = 0;
        score += CountConsecutiveTokens(grid, row, col, 1, 0, player);
        score += CountConsecutiveTokens(grid, row, col, 0, 1, player);
        score += CountConsecutiveTokens(grid, row, col, 1, 1, player);
        score += CountConsecutiveTokens(grid, row, col, 1, -1, player);
        return score;
    }

    private int CountConsecutiveTokens(int[,] grid, int row, int col, int dRow, int dCol, int player)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            int newRow = row + i * dRow;
            int newCol = col + i * dCol;
            if (newRow < 0 || newRow >= 6 || newCol < 0 || newCol >= 7 || grid[newRow, newCol] != player)
            {
                break;
            }
            count++;
        }
        return count;
    }

    private int GetNextOpenRow(int col)
    {
        for (int row = 5; row >= 0; row--)
        {
            if (connect4Game.grid[row, col] == 0)
            {
                return row;
            }
        }
        return -1;
    }
}
