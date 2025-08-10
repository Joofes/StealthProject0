

using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class Mission : ScriptableObject
{
    public int levelRequirement;

    public string missionName, stageID;

    public Image missionImage;
}
