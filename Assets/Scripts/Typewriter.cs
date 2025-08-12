using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public TextMeshPro text;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }
    public void UpdateText(string newText)
    {
        StartCoroutine(TypeText(newText));
    }
    public IEnumerator TypeText(string newText)
        {
            text.text = "";
            foreach (char c in newText)
            {
                text.text += c;
                yield return new WaitForSeconds(0.05f); // Adjust delay as needed
            }
        }
}
