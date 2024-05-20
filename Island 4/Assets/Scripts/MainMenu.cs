using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu, Options, GameManager;
    [SerializeField] private Animator MainMenuAn, OptionsAn, CameraAn;

    public void PlayButton()
    {
        MainMenuAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(mainMenu));

        CameraAn.enabled = true;

        FindObjectOfType<AudioManager>().Play("Play");
        FindObjectOfType<AudioManager>().Play("Transition");
        FindObjectOfType<AudioManager>().Play("Game Music");

        gameObject.GetComponent<AudioSource>().enabled = false;

        GameManager.SetActive(true);
    }

    public void OptionsButton()
    {
        Options.SetActive(true);
        MainMenuAn.SetBool("Disappear", true);
        OptionsAn.SetBool("Disappear", false);
        StartCoroutine(DisableGameobjectOnRightTime(mainMenu));
    }

    public void BackOptionsButton()
    {
        mainMenu.SetActive(true);
        MainMenuAn.SetBool("Disappear", false);
        OptionsAn.SetBool("Disappear", true);
        StartCoroutine(DisableGameobjectOnRightTime(Options));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator DisableGameobjectOnRightTime(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
