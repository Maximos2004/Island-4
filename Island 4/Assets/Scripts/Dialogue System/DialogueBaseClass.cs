using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }

        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, Color textColor, TMP_FontAsset textFont, float delay, AudioClip sound, float delayBetweenLines)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                SoundForDialogue.instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }

            //yield return new WaitForSeconds(delayBetweenLines);
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }
    }
}