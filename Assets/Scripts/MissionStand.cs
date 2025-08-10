using System.Collections.Generic;
using UnityEngine;

public class MissionStand : MonoBehaviour, IInteractable
{
    public string interactPrompt => throw new System.NotImplementedException();

    public GameObject missionsMenu;


    public List<Mission> missions = new List<Mission>();

    PlayerStats stats;
    void Awake(){
        stats = FindObjectOfType<PlayerStats>();
    }
    public bool Interact(InteractSystem interactor)
    {
        missionsMenu.SetActive(true);
        LoadMissions();
        return true;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            missionsMenu.SetActive(false);
        }
    }

    public void LoadMissions()
    {
        foreach(Mission i in missions)
        {
            if(stats.level > i.levelRequirement)
            {
                //load level menu
            }
        }
    }
}
