using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private GameObject DialogueContener;
    [SerializeField] private GameObject Prologue;
    [SerializeField] private GameObject FirstGameDialogue;
    [SerializeField] private GameObject WelcomeBackDialogue;
    [SerializeField] private GameObject FirstFriendDialogue;
    [SerializeField] private GameObject LetsPlayDialogue;
    [SerializeField] private GameObject LetsTryAgainDialogue;
    [SerializeField] private GameObject FirstWinDialogue;
    [SerializeField] private GameObject WinDialogue;
    [SerializeField] private GameObject LoserDialogue;

    //Values for Save System
    [HideInInspector] public int Streak;
    [HideInInspector] public int WinStreak;
    [HideInInspector] public bool FirstGame;
    [HideInInspector] public bool FirstWin;
    [HideInInspector] public bool FirstFriend;
    [HideInInspector] public bool FirstMeeting;

    [Header("Animations")]

    [SerializeField] private Animator CameraAn;
    [SerializeField] private Animator ChatAn, OptionsAn, IslandUIAn, Connect4UIAn;

    [Header("UI")]
    [SerializeField] private GameObject PauseButtonUI;
    [SerializeField] private GameObject IslandUI, PauseMenu, PauseContainer, Options, Connect4UI, Connect4UICoudy, Connect4UILeaveButton, BackButtonOptionsOld, BackButtonOptionsNew, Score, SelectBuildingUI;
    [SerializeField] private TMP_Text WinStreakText;

    [Header("Connect4")]
    [SerializeField] private Connect4Game connect4Game;
    [SerializeField] private List<BoxCollider> Collumes = new List<BoxCollider>();

    private bool InGame;

    IEnumerator Start()
    {
        //Load Buildings

        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            Streak = data.Streak;
            WinStreak = data.WinStreak;
            FirstGame = data.FirstGame;
            FirstWin = data.FirstWin;
            FirstFriend = data.FirstFriend;
            FirstMeeting = data.FirstMeeting;
        }

        yield return new WaitForSeconds(2.6f);

        BackButtonOptionsOld.SetActive(false);
        BackButtonOptionsNew.SetActive(true);
        PauseButtonUI.SetActive(true);

        if (FirstMeeting == false)
        {
            DialogueContener.SetActive(true);
            FindObjectOfType<AudioManager>().Play("DialPop");
            ChatAn.SetBool("ChatDisapears", false);
            yield return new WaitForSeconds(1f);
            Prologue.SetActive(true);

            yield return new WaitUntil(() => Prologue.activeSelf == false);
            DialogueContener.SetActive(false);
            FirstMeeting = true;
            SaveSystem.SaveGame(this);
        }
        else
        {
            DialogueContener.SetActive(true);
            FindObjectOfType<AudioManager>().Play("DialPop");
            ChatAn.SetBool("ChatDisapears", false);
            yield return new WaitForSeconds(1f);
            WelcomeBackDialogue.SetActive(true);

            yield return new WaitUntil(() => WelcomeBackDialogue.activeSelf == false);
            DialogueContener.SetActive(false);
        }
        if(FirstGame == false)
        {
            CameraAn.SetBool("Connect 4", true);
            InGame = true;

            yield return new WaitForSeconds(3f);

            Connect4UIAn.SetBool("Disappear", false);
            Connect4UI.SetActive(true);
            Connect4UICoudy.SetActive(true);
            Connect4UILeaveButton.SetActive(false);
            Score.SetActive(false);
            connect4Game.VsCloudy = true;

            DialogueContener.SetActive(true);
            FindObjectOfType<AudioManager>().Play("DialPop");
            ChatAn.SetBool("ChatDisapears", false);
            yield return new WaitForSeconds(1f);
            FirstGameDialogue.SetActive(true);

            yield return new WaitUntil(() => FirstGameDialogue.activeSelf == false);
            DialogueContener.SetActive(false);

            foreach (BoxCollider collumes in Collumes)
            {
                collumes.enabled = true;
            }

            yield return new WaitUntil(() => FirstGame == true);
        }
        foreach (BoxCollider collumes in Collumes)
        {
            collumes.enabled = true;
        }
        IslandUIAn.SetBool("Disappear", false);
        IslandUI.SetActive(true);
    }

    public IEnumerator WowYouLosThe1stMatch()
    {
        StartCoroutine(WowYouLosThe1stMatch());
        foreach (BoxCollider collumes in Collumes)
        {
            collumes.enabled = false;
        }

        DialogueContener.SetActive(true);
        FindObjectOfType<AudioManager>().Play("DialPop");
        ChatAn.SetBool("ChatDisapears", false);
        yield return new WaitForSeconds(1f);
        LetsTryAgainDialogue.SetActive(true);

        yield return new WaitUntil(() => LetsTryAgainDialogue.activeSelf == false);
        DialogueContener.SetActive(false);

        foreach (BoxCollider collumes in Collumes)
        {
            collumes.enabled = true;
        }
    }

    #region Button Functions

    public void PlayWithCloudyButton()
    {
        StartCoroutine(StartGame(false));
    }
    public void PlayWithFriendButton()
    {
        StartCoroutine(StartGame(true));
    }
    IEnumerator StartGame(bool Friend)
    {
        if (DialogueContener.activeSelf == false)
        {
            if (Friend && !FirstFriend)
            {
                DialogueContener.SetActive(true);
                FindObjectOfType<AudioManager>().Play("DialPop");
                ChatAn.SetBool("ChatDisapears", false);
                yield return new WaitForSeconds(1f);
                FirstFriendDialogue.SetActive(true);

                yield return new WaitUntil(() => FirstFriendDialogue.activeSelf == false);
                DialogueContener.SetActive(false);
                FirstFriend = true;
                SaveSystem.SaveGame(this);
            }
            if (!Friend)
            {
                DialogueContener.SetActive(true);
                FindObjectOfType<AudioManager>().Play("DialPop");
                ChatAn.SetBool("ChatDisapears", false);
                yield return new WaitForSeconds(1f);
                LetsPlayDialogue.SetActive(true);

                yield return new WaitUntil(() => LetsPlayDialogue.activeSelf == false);
                DialogueContener.SetActive(false);
            }

            CameraAn.SetBool("Connect 4", true);
            InGame = true;
            IslandUIAn.SetBool("Disappear", true);
            StartCoroutine(DisableGameobjectOnRightTime(IslandUI));
            if (Friend)
            {
                connect4Game.VsCloudy = false;
            }
            else
            {
                connect4Game.VsCloudy = true;
            }

            yield return new WaitForSeconds(3f);

            Connect4UIAn.SetBool("Disappear", false);
            Connect4UI.SetActive(true);
            if (Friend)
            {
                Connect4UICoudy.SetActive(false);
                Score.SetActive(true);
                Connect4UILeaveButton.SetActive(true);
            }
            else
            {
                Connect4UICoudy.SetActive(true);
                Score.SetActive(false);
                Connect4UILeaveButton.SetActive(true);
            }
        }
    }

    public void BackToIslandButton()
    {
        CameraAn.SetBool("Connect 4", false);
        InGame = false;
        IslandUIAn.SetBool("Disappear", false);
        IslandUI.SetActive(true);

        Connect4UIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(Connect4UI));

        ResetConnect4();
    }

    public void OptionsButton()
    {
        PauseContainer.SetActive(false);
        Options.SetActive(true);
        OptionsAn.SetBool("Disappear", false);
    }

    public void OptionsBackButton()
    {
        PauseContainer.SetActive(true);
        OptionsAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(Options));
    }

    public void ResumeButton()
    {
        PauseButtonUI.SetActive(true);
        if (!InGame)
        {
            IslandUIAn.SetBool("Disappear", false);
            IslandUI.SetActive(true);
        }
        PauseMenu.SetActive(false);
        foreach (BoxCollider collumes in Collumes)
        {
            collumes.enabled = true;
        }
    }

    public void PauseButton()
    {
        PauseButtonUI.SetActive(false);
        IslandUIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(IslandUI));
        PauseMenu.SetActive(true);
        foreach (BoxCollider collumes in Collumes)
        {
            collumes.enabled = false;
        }
    }

    IEnumerator DisableGameobjectOnRightTime(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    #endregion Button Functions

    public void ResetConnect4()
    {
        connect4Game.ResetCounter();
        connect4Game.Turn = false;
    }
    public IEnumerator Winer()
    {
        StartCoroutine(Winer());
        CameraAn.SetBool("Connect 4", false);
        InGame = false;

        Connect4UIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(Connect4UI));

        ResetConnect4();

        DialogueContener.SetActive(true);
        FindObjectOfType<AudioManager>().Play("DialPop");
        ChatAn.SetBool("ChatDisapears", false);
        yield return new WaitForSeconds(1f);
        WinDialogue.SetActive(true);

        yield return new WaitUntil(() => WinDialogue.activeSelf == false);
        DialogueContener.SetActive(false);

        SelectBuildingUI.SetActive(true);
    }
    public IEnumerator Lost()
    {
        StartCoroutine(Lost());
        CameraAn.SetBool("Connect 4", false);
        InGame = false;
        IslandUIAn.SetBool("Disappear", false);
        IslandUI.SetActive(true);

        Connect4UIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(Connect4UI));

        ResetConnect4();

        DialogueContener.SetActive(true);
        FindObjectOfType<AudioManager>().Play("DialPop");
        ChatAn.SetBool("ChatDisapears", false);
        yield return new WaitForSeconds(1f);
        LoserDialogue.SetActive(true);

        yield return new WaitUntil(() => LoserDialogue.activeSelf == false);
        DialogueContener.SetActive(false);
    }

    public IEnumerator Win1stTime()
    {
        StartCoroutine(Win1stTime());
        CameraAn.SetBool("Connect 4", false);
        InGame = false;
        IslandUIAn.SetBool("Disappear", false);
        IslandUI.SetActive(true);

        Connect4UIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(Connect4UI));

        ResetConnect4();

        DialogueContener.SetActive(true);
        FindObjectOfType<AudioManager>().Play("DialPop");
        ChatAn.SetBool("ChatDisapears", false);
        yield return new WaitForSeconds(1f);
        FirstWinDialogue.SetActive(true);

        yield return new WaitUntil(() => FirstWinDialogue.activeSelf == false);
        DialogueContener.SetActive(false);

        IslandUIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(IslandUI));

        yield return new WaitForSeconds(0.5f);

        DialogueContener.SetActive(true);
        FindObjectOfType<AudioManager>().Play("DialPop");
        ChatAn.SetBool("ChatDisapears", false);
        yield return new WaitForSeconds(1f);
        WinDialogue.SetActive(true);

        yield return new WaitUntil(() => WinDialogue.activeSelf == false);
        DialogueContener.SetActive(false);

        SelectBuildingUI.SetActive(true);

        yield return new WaitUntil(() => IslandUI.activeSelf == true);

        FirstGame = true;
        SaveSystem.SaveGame(this);
    }

    public void AddStreakCount()
    {
        WinStreak++;
        SaveSystem.SaveGame(this);
        FindObjectOfType<AudioManager>().Play("Win");
    }

    public void ResetStreakCount()
    {
        WinStreak = 0;
        SaveSystem.SaveGame(this);
        FindObjectOfType<AudioManager>().Play("Lose");
    }

    private void Update()
    {
        WinStreakText.text = WinStreak.ToString();
    }
}
