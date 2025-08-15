using System.Collections.Generic;
using UnityEngine;

public class TutorialSys : MonoBehaviour
{
    public GameObject firstEnemy, firstBarrier, secondEnemy, secondBarrier, thirdEnemy, thirdBarrier;

    public List<GameObject> tripleEnemies;

    public GameObject fourthBarrier;

    public Typewriter text;
    public GameObject clickIndicator;

    public PlayerStats stats;
    void Start()
    {
        text.UpdateText("Welcome to the tutorial!");
    }


    bool started = false;
    bool ropeCollected = false;

    public GameObject triggerOne;
    bool triggerOneFinished;
    bool dummyKilled;
    public GameObject triggerTwo;
    bool triggerTwoFinished = false;

    bool secondEnemyKilled;

            public GameObject triggerThree;
        bool triggerThreeFinished = false;
        bool thirdEnemyKilled = false;
    void TutorialDialogue()
    {
        if (text.text.text == "Welcome to the tutorial!" || text.text.text == "Watchguards can end entire missions if they set off an alarm, always take them out quietly.")
        {
            clickIndicator.SetActive(true);
        }
        else
        {
            clickIndicator.SetActive(false);
        }
        if (text.text.text == "Welcome to the tutorial!" && Input.GetMouseButtonDown(0))
        {
            text.UpdateText("Grab that rope over there with E");
        }
        if (stats.ropeAmnt == 1 && !started)
        {
            text.UpdateText("Great! Now place it near the wall with R");
            started = true;
            ropeCollected = true;
        }
        if (ropeCollected && stats.ropeAmnt == 0)
        {
            text.UpdateText("Attach with Q and release by hitting R again");
            ropeCollected = false;
        }
        if (!triggerOne.activeInHierarchy && !triggerOneFinished)
        {
            triggerOneFinished = true;
            text.UpdateText("This guy's just a dummy so don't worry, walk up and attack!");
        }
        if (!dummyKilled && firstEnemy == null)
        {
            text.UpdateText("Nice job! Next we'll take out a real guard");
            dummyKilled = true;
        }
        if (!triggerTwo.activeInHierarchy && !triggerTwoFinished)
        {
            triggerTwoFinished = true;
            text.UpdateText("Either sneak up behind him or fight him head on!");
        }
        if (!secondEnemyKilled && secondEnemy == null)
        {
            secondEnemyKilled = true;
            text.UpdateText("Great work agent! In the next room you'll take out a watchguard");
        }


        if (!triggerThree.activeInHierarchy && !triggerThreeFinished)
        {
            triggerThreeFinished = true;
            text.UpdateText("Grab these knives with E and throw them with F!");
        }


        if (!thirdEnemyKilled && thirdEnemy == null)
        {
            thirdEnemyKilled = true;
            text.UpdateText("Watchguards can end entire missions if they set off an alarm, always take them out quietly.");
        }
        if (text.text.text == "Watchguards can end entire missions if they set off an alarm, always take them out quietly." && Input.GetMouseButtonDown(0))
        {
            thirdBarrier.SetActive(false);
            text.UpdateText("Take out these three guards by any method to finish the tutorial");
            foreach (GameObject e in tripleEnemies)
            {
                e.GetComponent<Enemy>().currentDetection = e.GetComponent<Enemy>().detectionLimit;
            }
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
