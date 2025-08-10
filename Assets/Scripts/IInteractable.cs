using UnityEngine;

public interface IInteractable
{
    public string interactPrompt {get;}
    public bool Interact(InteractSystem interactor);
}
