using System.Collections.Generic;
using UnityEngine;

public class TutorialSys : MonoBehaviour
{
    public GameObject firstEnemy, firstBarrier, secondEnemy, secondBarrier, thirdEnemy, thirdBarrier;

    public List<GameObject> tripleEnemies;

    public GameObject fourthBarrier;

    public Typewriter text;

    public PlayerStats stats;
    void Start()
    {
        text.UpdateText("Welcome to the tutorial!");
    }


    bool started = false;
    void TutorialDialogue()
    {
        if (text.text.text == "Welcome to the tutorial!" && Input.GetMouseButtonDown(0))
        {
            text.StopAllCoroutines();
            text.UpdateText("Grab that rope over there with E");
        }
        
        if (stats.ropeAmnt == 1 && !started)
        {
            text.text.text = "";
            text.StopCoroutine(text.TypeText(""));
            started = true;
            text.StartCoroutine(text.TypeText("Great! Now place it near the wall with R"));
            Debug.Log("started new cor");
        }
    }

    void Update()
    {
        TutorialDialogue();
        if (firstEnemy == null)
        {
            firstBarrier.SetActive(false);
        }
        if (secondEnemy == null)
        {
            secondBarrier.SetActive(false);
        }
        if (thirdEnemy == null)
        {
            thirdBarrier.SetActive(false);
        }
        bool allDestroyed = true;
        foreach (GameObject i in tripleEnemies)
        {
            if (i != null)
            {
            allDestroyed = false;
            break;
            }
        }
        if (allDestroyed)
        {
            fourthBarrier.SetActive(false);
        }
    }
}
