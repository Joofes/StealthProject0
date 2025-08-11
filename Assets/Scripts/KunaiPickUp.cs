using UnityEngine;

public class KunaiPickUp : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string interactPrompt => throw new System.NotImplementedException();
    public ProjectileSO kunai;
    public bool Interact(InteractSystem interactor)
    {
        kunai.count++;
        Destroy(gameObject);
        return true;
    }
}
