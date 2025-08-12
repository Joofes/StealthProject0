using UnityEngine;

public class TutorialSys : MonoBehaviour
{
    public GameObject firstEnemy, firstBarrier, secondEnemy, secondBarrier, thirdEnemy, thirdBarrier;

    void Update()
    {
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
    }
}
