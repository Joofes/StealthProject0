using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public TextMeshPro text;

    private Coroutine typingCoroutine;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }
      public void UpdateText(string newText)
    {
        // Stop the current typing coroutine if it's running
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(newText));
    }

    public IEnumerator TypeText(string newText)
    {
        text.text = "";
        foreach (char c in newText)
        {
            text.text += c;
            yield return new WaitForSeconds(0.01f); // Adjust delay as needed
        }
            
         typingCoroutine = null; // clear reference when done
    }
}
