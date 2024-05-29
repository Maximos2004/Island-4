using System.Collections;
using UnityEngine;

public class TokenPlacer : MonoBehaviour
{
    private GameObject previousObject;
    [SerializeField] private Connect4Game connect4Game;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject P1Wins, P2Wins;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grid") || other.CompareTag("Last Grid"))
        {
            TokenGridSpot tokenGridSpot = other.GetComponent<TokenGridSpot>();
            if (tokenGridSpot != null)
            {
                if (tokenGridSpot.IsSpotTaken) // If the spot already has a token
                {
                    if (previousObject != null && !previousObject.GetComponent<TokenGridSpot>().IsSpotTaken)
                    {
                        PlaceTokenInSpot(previousObject.GetComponent<TokenGridSpot>());
                    }
                }
                previousObject = other.gameObject;
            }
        }

        // If it's the last Grid Cell that means it's the last cell anyway so add a token (only if a token doesn't exist there)
        if (other.CompareTag("Last Grid"))
        {
            TokenGridSpot tokenGridSpot = other.GetComponent<TokenGridSpot>();
            if (tokenGridSpot != null && !tokenGridSpot.IsSpotTaken)
            {
                PlaceTokenInSpot(tokenGridSpot);
            }
        }
    }

    private void PlaceTokenInSpot(TokenGridSpot tokenGridSpot)
    {
        if (connect4Game.Turn) // Checking who's turn it is
        {
            tokenGridSpot.IsPlayer1 = true;
        }
        else
        {
            tokenGridSpot.IsPlayer1 = false;
        }
        tokenGridSpot.IsSpotTaken = true;
        tokenGridSpot.UpdateTokenDisplay();

        // Update game state in Connect4Game
        int column = GetColumnIndex(tokenGridSpot.gameObject);
        int row = GetRowIndex(tokenGridSpot.gameObject);
        connect4Game.grid[row, column] = connect4Game.Turn ? 1 : 2;
        if (connect4Game.CheckWinCondition(row, column))
        {
            if (connect4Game.Turn)
            {
                //Player 1 Wins
                if (!connect4Game.VsCloudy)
                {
                    connect4Game.PvPWins(P1Wins);
                    connect4Game.AddToWinCounterP1();
                }
                else
                {
                    if (!gameManager.FirstGame)
                    {
                        StartCoroutine(gameManager.Win1stTime());
                    }
                    else
                    {
                        StartCoroutine(gameManager.Winer());
                    }
                    connect4Game.ResetCounter();
                    gameManager.AddStreakCount();
                }
            }
            else
            {
                //Player 2 Wins
                if (!connect4Game.VsCloudy)
                {
                    connect4Game.PvPWins(P2Wins);
                    connect4Game.AddToWinCounterP2();
                }
                else
                {
                    if (!gameManager.FirstGame)
                    {
                        StartCoroutine(gameManager.WowYouLosThe1stMatch());
                    }
                    else
                    {
                        StartCoroutine(gameManager.Lost());
                    }
                    connect4Game.ResetCounter();
                    gameManager.ResetStreakCount();
                }
            }
        }
    }

    private int GetColumnIndex(GameObject gridObject)
    {
        string name = gridObject.name;
        int index = int.Parse(name.Replace("Grid (", "").Replace(")", ""));
        return (index - 1) % 7;
    }

    private int GetRowIndex(GameObject gridObject)
    {
        string name = gridObject.name;
        int index = int.Parse(name.Replace("Grid (", "").Replace(")", ""));
        return (index - 1) / 7;
    }
}
