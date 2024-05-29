using System.Collections;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] private GameObject TockenToEnable, Tocken2Enable;
    [SerializeField] private Animator TockenAn, TockenAn2;
    [SerializeField] private Connect4Game connect4Game;
    [SerializeField] private TokenGridSpot LastSpot;
    [SerializeField] private Connect4AI connect4AI;

    public IEnumerator AIPlays()
    {
        connect4Game.Droping = true;
        Tocken2Enable.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Select");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DroppingTocken(Tocken2Enable, TockenAn2));
    }

    void OnMouseEnter()
    {
        if (!connect4Game.Droping)
        {
            if (!connect4Game.Turn)
            {
                TockenToEnable.SetActive(true);
                FindObjectOfType<AudioManager>().Play("Select");
            }
            else if (!connect4Game.VsCloudy)
            {
                Tocken2Enable.SetActive(true);
                FindObjectOfType<AudioManager>().Play("Select");
            }
        }
    }

    void OnMouseExit()
    {
        if (!connect4Game.Droping)
        {
            TockenToEnable.SetActive(false);
            Tocken2Enable.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!connect4Game.Droping)
        {
            if (!LastSpot.IsSpotTaken)
            {
                if (!connect4Game.Turn)
                {
                    if(TockenToEnable.activeSelf == true)
                    {
                        StartCoroutine(DroppingTocken(TockenToEnable, TockenAn));
                    }
                }
                else if (!connect4Game.VsCloudy)
                {
                    StartCoroutine(DroppingTocken(Tocken2Enable, TockenAn2));
                }
            }
        }
    }

    IEnumerator DroppingTocken(GameObject TockenToEnable, Animator TockenAn)
    {
        connect4Game.Droping = true;
        FindObjectOfType<AudioManager>().Play("TockenThrow");
        TockenAn.Play("TockenDropP1", 0, 0);
        connect4Game.Turn = !connect4Game.Turn;
        yield return new WaitForSeconds(0.6f);
        TockenToEnable.SetActive(false);
        connect4Game.Droping = false;
        if (connect4Game.Turn)
        {
            if (connect4Game.VsCloudy)
            {
                StartCoroutine(connect4AI.MakeMove());
            }
        }
    }
}
