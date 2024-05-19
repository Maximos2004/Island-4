using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour
{
    [SerializeField] private Animator ButtonAn;
    [SerializeField] private Button Button;
    [SerializeField] private bool IsDisabled;

    public void Start()
    {
        if (IsDisabled)
        {
            Button.enabled = false;
            ButtonAn.SetBool("Selected", true);
        }
    }
    public void OnSelection()
    {
        if (!IsDisabled)
        {
            FindObjectOfType<AudioManager>().Play("Select");
            ButtonAn.SetBool("Selected", true);
        }
    }

    public void OnUnSelection()
    {
        if (!IsDisabled)
        {
            ButtonAn.SetBool("Selected", false);
        }
    }

    public void OnClick()
    {
        if (!IsDisabled)
        {
            FindObjectOfType<AudioManager>().Play("Click");
            StartCoroutine(ActivateClickAnimation());
        }
    }

    // Method to re-enable the button and reset the isDisabled flag
    public void EnableButton()
    {
        IsDisabled = false;
        Button.enabled = true;
        ButtonAn.SetBool("Selected", false);
    }

    public void DisableButton()
    {
        IsDisabled = true;
        Button.enabled = false;
        ButtonAn.SetBool("Selected", true);
    }

    IEnumerator ActivateClickAnimation()
    {
        ButtonAn.SetBool("Click", true);
        yield return new WaitForSeconds(0.1f);
        ButtonAn.SetBool("Click", false);
    }
}
