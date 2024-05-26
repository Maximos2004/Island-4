using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private GameObject DialogueContener;
    [SerializeField] private GameObject Prologue;
    [SerializeField] private GameObject FirstGameDialogue;
    [SerializeField] private GameObject WelcomeBackDialogue;
    [SerializeField] private GameObject FirstFriendDialogue;
    [SerializeField] private GameObject LetsPlayDialogue;

    //Values for Save System
    [HideInInspector] public int Streak;
    [HideInInspector] public bool FirstGame;
    [HideInInspector] public bool FirstWin;
    [HideInInspector] public bool FirstFriend;
    [HideInInspector] public bool FirstMeeting;

    [Header("Animations")]

    [SerializeField] private Animator CameraAn;
    [SerializeField] private Animator ChatAn, OptionsAn, IslandUIAn, Connect4UIAn;

    [Header("UI")]
    [SerializeField] private GameObject PauseButtonUI;
    [SerializeField] private GameObject IslandUI, PauseMenu, PauseContainer, Options, Connect4UI, Connect4UICoudy, Connect4UILeaveButton, BackButtonOptionsOld, BackButtonOptionsNew, Score;

    [Header("Connect4")]
    [SerializeField] private Connect4Game connect4Game;

    private bool InGame;

    IEnumerator Start()
    {
        //Load Buildings

        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            Streak = data.Streak;
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

            DialogueContener.SetActive(true);
            FindObjectOfType<AudioManager>().Play("DialPop");
            ChatAn.SetBool("ChatDisapears", false);
            yield return new WaitForSeconds(1f);
            FirstGameDialogue.SetActive(true);

            yield return new WaitUntil(() => FirstGameDialogue.activeSelf == false);
            DialogueContener.SetActive(false);

            //Wait Until Game Ends

            FirstGame = true;
            SaveSystem.SaveGame(this);
        }
        IslandUIAn.SetBool("Disappear", false);
        IslandUI.SetActive(true);
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
        if(Friend && !FirstFriend)
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
    }

    public void PauseButton()
    {
        PauseButtonUI.SetActive(false);
        IslandUIAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(IslandUI));
        PauseMenu.SetActive(true);
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
}
