using UnityEngine;

public class TokenGridSpot : MonoBehaviour
{
    [SerializeField] private GameObject P1Token, P2Token;

    public bool IsSpotTaken;
    public bool IsPlayer1;

    public void UpdateTokenDisplay()
    {
        if (IsSpotTaken)
        {
            if (IsPlayer1)
            {
                P1Token.SetActive(true);
                P2Token.SetActive(false);
            }
            else
            {
                P1Token.SetActive(false);
                P2Token.SetActive(true);
            }
        }
        else
        {
            P1Token.SetActive(false);
            P2Token.SetActive(false);
        }
    }
}
