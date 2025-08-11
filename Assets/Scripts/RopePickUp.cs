using UnityEngine;

public class RopePickUp : MonoBehaviour, IInteractable
{
    public string interactPrompt => throw new System.NotImplementedException();

    public bool Interact(InteractSystem interactor)
    {
        interactor.gameObject.GetComponent<PlayerStats>().ropeAmnt++;
        Destroy(gameObject);
        return true;
    }

}
